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
            Enemy currEnemyScript = collision.gameObject.GetComponent<Enemy>(); //we get the enemy hit for later

            if (!currEnemyScript.isAffectedByBottle) //we slow the enemy only if he isnt already affected by another bottle (so the bottles cannot stack)
            {
                currEnemyScript.SetMoveSpeedPercentage(currEnemyScript.moveSpeedPercentage - (float)moveSpeedMinusPercentage / 100f);
                //we need this so it still stacks additively with other slow effects
            }

            currEnemyScript.isAffectedByBottle = true;
            //the enemy is now affected by a bottle so we set it to true to prevent other bottles from slowing it even further

            if (currEnemyScript.Aura)
            {
                if (RemovesAura)
                {
                    currEnemyScript.Aura = false;
                }
                if (canHitAura)
                { 
                        Activate();
                    
                }
                //it only does nothing if the enemy has aura and we cant see it.
            }
            else //if the enemy does not have aura we can always hit it
            {
                    Activate();
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy currEnemy = collision.gameObject.GetComponent<Enemy>();
            if (currEnemy.isAffectedByBottle)
            {
                currEnemy.SetMoveSpeedPercentage(currEnemy.moveSpeedPercentage + (float)moveSpeedMinusPercentage / 100f); //make enemy normal speed again

            }
            currEnemy.isAffectedByBottle = false;
        }
    }


    public void Activate()
    {
        rb.velocity = Vector3.zero; //the bottle stops
        bulletCollider.enabled = false; //the bullet collision vanishes
        cloudRange.enabled = true; //the cloud collision gets activated
        
        //change Sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = BottleSprite;
        gameObject.transform.localScale = new Vector3(BottleSize,BottleSize,0);
    }

    public new void BulletHit()
    {

    }

   
}
