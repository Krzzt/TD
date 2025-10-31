using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class ClownBullet : Bullet
{
    private Rigidbody2D rb;
    private CircleCollider2D cloudRange;
    private PolygonCollider2D bulletCollider;
    public Sprite BottleSprite;
    public float BottleSize;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cloudRange = gameObject.GetComponent<CircleCollider2D>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
        BulletParent = gameObject.transform.parent.gameObject.GetComponent<TowerTest>();
        remainingPierce = BulletParent.Pierce;
        BulletLifeTime = BulletParent.BulletLifeTime;
        canHitAura = BulletParent.CanReadAura;
        Debug.Log("Instantiated");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().Aura)
            {
                if (canHitAura)
                {
                    remainingPierce--;
                    collision.gameObject.GetComponent<Enemy>().TakeDamage(BulletParent.Damage);

                    if (remainingPierce <= 0)
                    {
                        
                        rb.velocity = Vector3.zero;
                        Activate();
                        Debug.Log("boom");
                    }
                }
                //it only does nothing if the enemy has aura and we cant see it.
            }
            else
            {
                remainingPierce--;
                collision.gameObject.GetComponent<Enemy>().TakeDamage(BulletParent.Damage);

                if (remainingPierce <= 0)
                {

                    rb.velocity = Vector3.zero;
                    Activate();
                    Debug.Log("boom");
                }
            }

        }
    }


    public void Activate()
    {
        bulletCollider.enabled = false;
        cloudRange.enabled = true;
        
        //change Sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = BottleSprite;
        gameObject.transform.localScale = new Vector3(BottleSize,BottleSize,0);
    }
}
