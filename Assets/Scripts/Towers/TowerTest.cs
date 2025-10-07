using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerTest : MonoBehaviour
{
    public int TowerID;
    
    public List<GameObject> EnemiesInRange; //a list of every Enemy in range
    public List<float> EnemyProgression;
    public float AttackSpeed; //how long an attack takes in seconds
    public bool AttackReady; //checks if an attack is ready
    public int Damage; //the damage the tower does
    public float ShotSpeed; //how fast the bullets travel
    public float BulletLifeTime;



    public int targeting; //--> different number means different targeting: 0 = first, 1 = last, 2 = strong

    public GameObject BulletPrefab; //the Bullet as a Prefab
    public Transform BulletPoint;

    public GameObject TargetEnemy;
    public float distance;

    public int Pierce;

    public GameObject RangeIndicator;



    public string TowerName;
    public List<string> UpgradeNames;
    public List<string> UpgradeCosts;
    public List<Sprite> UpgradeSprites;

    public bool isClicked;
    public bool isHovering;

    public TMP_Text UpgradeTowerNameText;
    public TMP_Text UpgradeNameText;
    public TMP_Text UpgradeCostText;
    public Image UpgradeImage;

    public GameManager gm;

    public AddOn[] equippedAddOns;

    public bool CanReadAura;
    

    private void Awake()
    {
        AttackReady = true;


        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        equippedAddOns = new AddOn[2];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (EnemiesInRange.Count > 0 && AttackReady)
        {
            Attack();
        }
        if (!isClicked)
        {
            RangeIndicator.SetActive(false);
        }


        isHovering = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //if the new collision is an enemy
        {
          if (collision.gameObject.GetComponent<Enemy>().Aura) //if the enemy has Aura (so camo basically)
           {
                if (CanReadAura) //and if we can read aura
                {
                    EnemiesInRange.Add(collision.gameObject); //enemy is added to the list
                }
           }
          else //if the enemy has no aura
          {
            EnemiesInRange.Add(collision.gameObject); //we add it to the list
          }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().Aura) //if the enemy has Aura (so camo basically)
            {
                if (CanReadAura) //and if we can read aura
                {
                    EnemiesInRange.Remove(collision.gameObject); //enemy is removed from the list
                    //we do this to prevent stupid Null errors
                }
            }
            else
            {
                EnemiesInRange.Remove(collision.gameObject);
            }

        }
    }


    public void Attack() 
    {
        EnemyProgression.Clear();
        //attack
        int firstEnemyIndex = 0; //the index of the first enemy in the list
        float firstenemycomp = 0; //the completion this first enemy has
        int LastEnemyIndex = 0; //ditto for last
        float lastenemycomp = 0; //ditto for last

        int StrongEnemyIndex = 0; //Same for strong
        int StrongEnemyHP = 0;
        foreach (GameObject enemy in EnemiesInRange) //for every enemy in range
        {
            Enemy currEnemyScript = enemy.GetComponent<Enemy>();
            int currEnemyHP = currEnemyScript.EnemyHealth._currentHealth;
            float currMapComp = currEnemyScript.MapCompletion; //we get their map completion
            

            EnemyProgression.Add(currMapComp); //add them to our progression list
            if (currMapComp > firstenemycomp) //if this enemy is further than everyone we had before
            {
                firstenemycomp = currMapComp; //he is the new first enemy
                firstEnemyIndex = EnemyProgression.Count - 1; //and we set the index
            }
            if (currMapComp < lastenemycomp) //same for lastenemy
            {
                lastenemycomp = currMapComp;
                LastEnemyIndex = EnemyProgression.Count - 1;
            }
            if (StrongEnemyHP < currEnemyHP) //same for strong
            {
                StrongEnemyHP = currEnemyHP;
                StrongEnemyIndex = EnemyProgression.Count - 1;
            }

        } 
        switch (targeting) //we change the behaivour for each targeting option
        {
            case 0:
                TargetEnemy = EnemiesInRange[firstEnemyIndex];
                break;
            case 1:
                TargetEnemy = EnemiesInRange[LastEnemyIndex];
                break;
            case 2:
                TargetEnemy = EnemiesInRange[StrongEnemyIndex];
                break;
        }

        TurnToEnemy();
        GameObject newBullet = Instantiate(BulletPrefab, BulletPoint.position, BulletPoint.rotation, gameObject.transform); //instantiate the bullet at the right position
        newBullet.GetComponent<Rigidbody2D>().AddForce(BulletPoint.up * ShotSpeed, ForceMode2D.Impulse); //give the bullet velocity
        //attack

        AttackReady = false; //the attack is over so its not ready anymore
        StartCoroutine(waitforNextAttack()); //start the timer for the next attack
    }

    public IEnumerator waitforNextAttack()
    {
        yield return new WaitForSeconds(AttackSpeed);
        AttackReady = true;
    }

public void TurnToEnemy()
    {
        distance = Vector2.Distance(transform.position, TargetEnemy.transform.position);
        Vector2 direction = TargetEnemy.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0, 0, -90);
        //turning stuff
    }

    private void OnMouseDown()
    {
        //do the funny tower information
        if (gm.SelectedTower != gameObject)
        {

            StartCoroutine(SetNewSelectedTower());

        }
        else
        {
            gm.DeselectTower();
        }

    }


    public IEnumerator SetNewSelectedTower() //we need to wait a frame because of some shenanigans with the RangeIndicator and this code happening before code in the GameManager
    {
        yield return new WaitForEndOfFrame();
        isClicked = true;
        gm.UpgradePanel.SetActive(true); //set the Upgrade UI true
        gm.UpgradeArea.SetActive(true); // set the Upgrade Collision True
        SetTowerInformation();
        gm.SelectedTower = gameObject;

        gm.SelectEquippedAddOn(0);
        gm.SelectAddOn(0);
        //make a standard Selection
    }
    private void OnMouseOver()
    {
        if (gm.MockTower == null)
        {
            RangeIndicator.SetActive(true);
        }

        isHovering = true;
        gm.isOverTower = true;

    }


    public void SetTowerInformation()
    {
        UpgradeTowerNameText = gm.UpgradePanel.transform.Find("TowerName").gameObject.GetComponent<TMP_Text>();
        //UpgradeNameText = gm.UpgradePanel.transform.Find("UpgradeName").gameObject.GetComponent<TMP_Text>();
        //UpgradeCostText = gm.UpgradePanel.transform.Find("UpgradeCost").gameObject.GetComponent<TMP_Text>();
        //UpgradeCostText = gm.UpgradePanel.transform.Find("UpgradeCost").gameObject.GetComponent<TMP_Text>();
        //UpgradeImage = gm.UpgradePanel.transform.Find("UpgradeImage").gameObject.GetComponent<Image>();

        UpgradeTowerNameText.SetText(TowerName);
       // UpgradeNameText.SetText(UpgradeNames[currUpgradeStage]);
        //UpgradeCostText.SetText(UpgradeCosts[currUpgradeStage]);
        //UpgradeImage.sprite = UpgradeSprites[currUpgradeStage];

        for (int i = 0; i < gm.AddOnSprites[TowerID].sprites.Count; i++)
        {
            gm.AddOnImages[i].sprite = gm.AddOnSprites[TowerID].sprites[i];
        }
    }

    public void ChangeAddOns(int WhichAddonSwitches, int newAddonID) //i want to change this so every killer has addons and can equip 2 of them at once
    {
        if (equippedAddOns[WhichAddonSwitches] != null)
        {
            switch (equippedAddOns[WhichAddonSwitches].ID) //this reverts the effects of the now unequipped addon
            {
                case 0:
                    AttackSpeed += 0.1f;
                    break;
                case 1:
                    Pierce -= 1;
                    break;
                case 2:
                    AttackSpeed += 0.2f;
                    break;
                case 3:
                    //after stunning is implemented
                    break;
                case 4:
                    //after the waves of 2 are implemented
                    break;
                case 5:
                    Pierce -= 4;
                    break;
                case 6:
                    //the aura reading is a bit more complicated so we check for that at the end
                    Damage -= 4;
                    break;
                case 7:
                    //another stun L
                    break;
                case 8:
                    Damage -= 20;
                    break;
                case 9:
                    AttackSpeed *= 2f;
                    Pierce -= 5;
                    //aura reading and stuff
                    break;
                default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

            }
        }
 
        equippedAddOns[WhichAddonSwitches] = AddOnList.TowerAddOns[TowerID][newAddonID]; //dont know if this works yet

        switch (equippedAddOns[WhichAddonSwitches].ID) //this reverts the effects of the now unequipped addon
        {
            case 0:
                AttackSpeed -= 0.1f;
                break;
            case 1:
                Pierce += 1;
                break;
            case 2:
                AttackSpeed -= 0.2f;
                break;
            case 3:
                //after stunning is implemented
                break;
            case 4:
                //after the waves of 2 are implemented
                break;
            case 5:
                Pierce += 4;
                break;
            case 6:
                //the aura reading is a bit more complicated so we check for that at the end
                Damage += 4;
                break;
            case 7:
                //another stun L
                break;
            case 8:
                Damage += 20;
                break;
            case 9:
                AttackSpeed /= 2f;
                Pierce += 5;
                //aura reading and stuff
                break;
            default: break; //if nothing is equipped, we change nothing

        }

        CheckForAuraReading();
    }

    public void UnEquipAddOn(int equippedAddOnID)
    {
        switch (equippedAddOns[equippedAddOnID].ID) //this reverts the effects of the now unequipped addon
        {
            case 0:
                AttackSpeed += 0.1f;
                break;
            case 1:
                Pierce -= 1;
                break;
            case 2:
                AttackSpeed += 0.2f;
                break;
            case 3:
                //after stunning is implemented
                break;
            case 4:
                //after the waves of 2 are implemented
                break;
            case 5:
                Pierce -= 4;
                break;
            case 6:
                //the aura reading is a bit more complicated so we check for that at the end
                Damage -= 4;
                break;
            case 7:
                //another stun L
                break;
            case 8:
                Damage -= 20;
                break;
            case 9:
                AttackSpeed *= 2f;
                Pierce -= 5;
                //aura reading and stuff
                break;
            default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

        }

        CheckForAuraReading();
    }

    public void CheckForAuraReading() //if at leats 1 of the 2 equippedAddOns has Aura Reading, it can read aura
    {
        foreach (AddOn AO in equippedAddOns)
        {
            if  (AO != null && AO.givesAuraReading )
            {
                CanReadAura = true;
            }
        }
    }

}
