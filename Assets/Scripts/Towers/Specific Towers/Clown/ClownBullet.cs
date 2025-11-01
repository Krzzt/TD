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
    public int moveSpeedMinusPercentage;
    public ClownTower ClownScript;
    public bool RemovesAura;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cloudRange = gameObject.GetComponent<CircleCollider2D>();
        bulletCollider = gameObject.GetComponent<PolygonCollider2D>();
        BulletParent = gameObject.transform.parent.gameObject.GetComponent<TowerTest>();
        ClownScript = gameObject.transform.parent.gameObject.GetComponent<ClownTower>();
        remainingPierce = BulletParent.Pierce;
        BulletLifeTime = BulletParent.BulletLifeTime;
        canHitAura = BulletParent.CanReadAura;
        BottleSize = ClownScript.BottleSize;
        moveSpeedMinusPercentage = ClownScript.BottleSlowPercentage;
        RemovesAura = ClownScript.BottleRemoveAura;
    }




    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy currEnemyScript = collision.gameObject.GetComponent<Enemy>();
            if (!currEnemyScript.isAffectedByBottle)
            {
                currEnemyScript.SetMoveSpeedPercentage(currEnemyScript.moveSpeedPercentage - (float)moveSpeedMinusPercentage / 100f); //make enemy slow
            }

            currEnemyScript.isAffectedByBottle = true;

            if (currEnemyScript.Aura)
            {
                if (RemovesAura)
                {
                    currEnemyScript.Aura = false;
                }
                if (canHitAura)
                {
                    remainingPierce--;
                    currEnemyScript.TakeDamage(BulletParent.Damage);

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
                currEnemyScript.TakeDamage(BulletParent.Damage);

                if (remainingPierce <= 0)
                {

                    rb.velocity = Vector3.zero;
                    Activate();
                }
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().isAffectedByBottle)
            {
                collision.gameObject.GetComponent<Enemy>().SetMoveSpeedPercentage(collision.gameObject.GetComponent<Enemy>().moveSpeedPercentage + (float)moveSpeedMinusPercentage / 100f); //make enemy normal speed again

            }
            collision.gameObject.GetComponent<Enemy>().isAffectedByBottle = false;
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
