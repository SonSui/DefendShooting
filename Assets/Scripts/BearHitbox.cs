using UnityEngine;

public class BearHitbox : MonoBehaviour
{
    public Damage damage;
    public float Lifetime = 0.5f;
    private float timer;
    public GameObject explosionPrefab;

    public void SetDamage(Damage dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        timer = Lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }

}
