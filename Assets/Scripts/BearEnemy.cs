using UnityEngine;

public class BearEnemy : BaseEnemy
{
    public GameObject bearHitbox;
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (transform.position.x > leftEdge)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            if (currentInterval <= 0f)
            {
                animator.SetTrigger("Attack");
                currentInterval = attackInterval;
            }
            else
            {
                currentInterval -= Time.deltaTime;
            }
        }
    }
    public void OnEnemyAttack()
    {
        Vector3 offset = new Vector3(-0.5f, 0f, 0f);
        Vector3 spawnPosition = transform.position+offset;
        GameObject h = Instantiate(bearHitbox,spawnPosition,Quaternion.identity);
        BearHitbox hitbox = h.GetComponent<BearHitbox>();
        if (hitbox != null)
        {
            hitbox.SetDamage(damage);
        }
    }
}
