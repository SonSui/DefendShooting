using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction = Vector2.right;

    public int damage = 1;
    public int penetration = 0;
    public int MaxDamageBonus = 2;
    public int MaxPenetrationBonus = 1;

    public Vector2 leftTopEdge = new Vector2(-10f, 6f);
    public Vector2 rightTopEdge = new Vector2(10f, 6f);
    public Vector2 leftBottomEdge = new Vector2(-10f, -6f);
    public Vector2 rightBottomEdge = new Vector2(10f, -6f);

    public GameObject trail;

    public GameObject explosionPrefab;

    public void Initialize(Vector2 dir, int dmg, int pen)
    {
        direction = dir.normalized;
        damage = dmg;
        penetration = pen;
        if (dmg < 10) trail.SetActive(false);
        else
        {
            trail.SetActive(true);
            damage += MaxDamageBonus;
            penetration += MaxPenetrationBonus;
        }
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (transform.position.x < leftBottomEdge.x || transform.position.x > rightBottomEdge.x ||
            transform.position.y < leftBottomEdge.y || transform.position.y > leftTopEdge.y)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Enemy")
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                Damage d = new Damage { dmg = damage };
                enemy.TakeDamage(d);
                if (explosionPrefab != null)
                {
                    Vector2 contactPoint = other.ClosestPoint(transform.position);
                    Instantiate(explosionPrefab, contactPoint, Quaternion.identity);
                    
                }
                penetration --;
                if (penetration <= 0)
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }

}
