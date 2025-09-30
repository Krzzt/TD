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

    public GameObject UpgradePanel;
    public TMP_Text UpgradeTowerNameText;
    public TMP_Text UpgradeNameText;
    public TMP_Text UpgradeCostText;
    public Image UpgradeImage;

    public int currUpgradeStage;

    public GameManager gm;


    

    private void Awake()
    {
        AttackReady = true;
        UpgradePanel = GameObject.FindWithTag("Upgrade");
        UpgradeTowerNameText = UpgradePanel.transform.Find("TowerName").gameObject.GetComponent<TMP_Text>();
        UpgradeNameText = UpgradePanel.transform.Find("UpgradeName").gameObject.GetComponent<TMP_Text>();
        UpgradeCostText = UpgradePanel.transform.Find("UpgradeCost").gameObject.GetComponent<TMP_Text>();
        UpgradeImage = UpgradePanel.transform.Find("UpgradeImage").gameObject.GetComponent<Image>();

        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemiesInRange.Remove(collision.gameObject);
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
            isClicked = true;
            UpgradePanel.SetActive(true);
            Debug.Log("Upgrade panel is " + UpgradePanel.activeSelf);
            SetTowerInformation();
            gm.SelectedTower = gameObject;
        }

    }

    private void OnMouseOver()
    {
        RangeIndicator.SetActive(true);
        isHovering = true;
        gm.isOverTower = true;

    }


    public void SetTowerInformation()
    {
        UpgradeTowerNameText.SetText(TowerName);
        UpgradeNameText.SetText(UpgradeNames[currUpgradeStage]);
        UpgradeCostText.SetText(UpgradeCosts[currUpgradeStage]);
        UpgradeImage.sprite = UpgradeSprites[currUpgradeStage];
    }

}
