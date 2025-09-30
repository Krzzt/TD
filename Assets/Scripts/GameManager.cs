using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float MAP_LENGTH;

    public int CurrMoney;

    public GameObject SelectedTower;
    public GameObject UpgradeScreen;

    public bool isOverTower;

    public bool IsSelectingAnyTowerUI;

    public List<GameObject> TowerPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !isOverTower) //if you press the lft mouse button and are not over any towers
        {
            UpgradeScreen.SetActive(false); //the upgradescreen disappears
            SelectedTower = null; //you have no tower selected

        }
        isOverTower = false; //this is always set false here but the script in the towers runs after that so it can get set true if you hover over a tower
    }

    public void GiveMoney(int MoneyToGive)
    {
        CurrMoney += MoneyToGive;
    }
}
