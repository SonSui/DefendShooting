using UnityEngine;
using System;
public struct Damage
{
    public int dmg;
    public Damage(int damage = 1)
    {
        dmg = damage;
    }
    

}
public abstract class BaseEnemy : MonoBehaviour
{
    public float speed = 1f;
    public int healthMax = 10;
    protected int health;
    public float leftEdge = -4f;
    public float attackInterval = 2f;
    protected float currentInterval = 0f;
    public Damage damage = new Damage(1);

    public int scoreValue = 1;

    public GameObject explosionPrefab;

    public event Action<BaseEnemy, int> OnHPChanged; //HPのUI
    public event Action<BaseEnemy> OnDead; //死亡処理
    public event Action<int> OnDefeated;　 //敵を倒した時のスコア加算

    protected Animator animator;
    private int sortingOrderOffset = 200;

   
        
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        health = healthMax;
        OnHPChanged?.Invoke(this, health);　 // 初期HPをUIに通知

        SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
        if (mainRenderer != null)
        {
            int sortingOrder = Mathf.RoundToInt(-transform.position.y * 100 + sortingOrderOffset);
            mainRenderer.sortingOrder = sortingOrder;

            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
            foreach (var renderer in childRenderers)
            {
                if (renderer != mainRenderer)
                {
                    renderer.sortingOrder = sortingOrder - 1;
                }
            }
        }
    }

    public virtual void TakeDamage(Damage amount)
    {
        health -= amount.dmg;
        OnHPChanged?.Invoke(this, health);　 // HPのUIを更新

        if (health <= 0)
        {
            OnDead?.Invoke(this);　 // 死亡処理を通知
            OnDefeated?.Invoke(scoreValue);　 // 敵を倒した時のスコア加算
            Die();
        }
    }
    public virtual void SetEnemyStatus(int healthMax, float speed)
    {
        this.healthMax = healthMax;
        this.speed = speed;
        health = healthMax;
        OnHPChanged?.Invoke(this, health);　 // 初期HPをUIに通知
    }

    protected virtual void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public int GetHealth() => health;
    public int GetHealthMax() => healthMax;
    public float GetSpeed() => speed;
}