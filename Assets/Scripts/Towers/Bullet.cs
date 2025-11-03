using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TowerTest BulletParent;
    public int remainingPierce;
    public float BulletLifeTime;
    public bool canHitAura;

    private Enemy EnemyHit;

    private void Awake()
    {
        BulletParent = gameObject.transform.parent.gameObject.GetComponent<TowerTest>();
        remainingPierce = BulletParent.Pierce;
        BulletLifeTime = BulletParent.BulletLifeTime;
        canHitAura = BulletParent.CanReadAura;

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
        gameObject.transform.SetParent(GameObject.FindWithTag("GameMap").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHit = collision.gameObject.GetComponent<Enemy>();
            if (collision.gameObject.GetComponent<Enemy>().Aura)
            {
                if (canHitAura)
                {
                    BulletHit();
                }
                //it only does nothing if the enemy has aura and we cant see it.
            }
            else
            {
                BulletHit();
            }

        }
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(BulletLifeTime);
        Destroy(gameObject);
    }

    public void BulletHit()
    {
        remainingPierce--;
        EnemyHit.TakeDamage(BulletParent.Damage);

        if (remainingPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
