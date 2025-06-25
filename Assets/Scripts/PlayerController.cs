using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    Normal,
    None,
    Dead
};
public class PlayerController : MonoBehaviour
{
    public GameObject shootPrefab;

    public int healthMax = 50;
    public int health = 50;
    public int score = 0;

    public float buttonTimeMax = 3f;
    private float buttonTime = 0f;

    public int damageMax = 10;
    private int currentDamage = 1;

    public int penetrationMax = 5;
    private int currentPenetration = 0;

    public PlayerState playerState = PlayerState.Normal;


    public Image healthBar;
    public Image attackBar;
    private void Start()
    {
        healthBar.fillAmount = 1f;
        attackBar.fillAmount = 0f;
    }
    void Update()
    {
        if (playerState == PlayerState.Dead)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            buttonTime = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            buttonTime += Time.deltaTime;
            buttonTime = Mathf.Clamp(buttonTime, 0, buttonTimeMax);
            attackBar.fillAmount = buttonTime / buttonTimeMax;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float chargeRatio = buttonTime / buttonTimeMax;
            currentDamage = Mathf.Max(Mathf.RoundToInt(damageMax * chargeRatio),1);
            currentPenetration = Mathf.RoundToInt(penetrationMax * chargeRatio);

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position);

            GameObject bullet = Instantiate(shootPrefab, transform.position, Quaternion.identity);
            PlayerShoot bulletScript = bullet.GetComponent<PlayerShoot>();
            bulletScript.Initialize(direction.normalized, currentDamage, currentPenetration);
            attackBar.fillAmount = 0f;
        }
    }
    public void TakeDamage(Damage dmg)
    {
        if (playerState == PlayerState.Dead)
        {
            return;
        }
        health -= dmg.dmg;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        health = Mathf.Clamp(health, 0, healthMax);
        healthBar.fillAmount = (float)health / healthMax;
    }
    private void Die()
    {
        playerState = PlayerState.Dead;
        GameManager.instance?.GameOver();
    }
}
