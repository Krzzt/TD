using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float MAP_LENGTH;

    public int CurrMoney;

    public GameObject SelectedTower; //tower that is selected (with the upgrades)
    public GameObject UpgradeScreen;

    public bool isOverTower;

    public bool IsSelectingAnyTowerUI;

    public List<GameObject> TowerPrefabs;
    public List<GameObject> MockTowerPrefabs;

    public GameObject SelectedTowerInUI;

    public DeselectTower DT;


    public GameObject UpgradePanel;
    private void Awake()
    {
        DT = GameObject.FindWithTag("Deadzone").GetComponent<DeselectTower>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0) && !SelectedTower.GetComponent<TowerTest>().isHovering) //if you press the lft mouse button and are not over the selectedtower
            {

                DeselectTower();
            }
            isOverTower = false; //this is always set false here but the script in the towers runs after that so it can get set true if you hover over a tower
        }
      

    }

    public void GiveMoney(int MoneyToGive)
    {
        CurrMoney += MoneyToGive;
    }

    public void DeselectTower()
    {
        Debug.Log("range should disappear");
        TowerTest selectedTowerScript = SelectedTower.GetComponent<TowerTest>();
        selectedTowerScript.isClicked = false; //set the isClicked in the script from the tower to false
        UpgradeScreen.SetActive(false); //the upgradescreen disappears
        SelectedTower = null; //you have no tower selected
    }

    public void DeselectTowerUI()
    {
        SelectedTowerInUI.GetComponent<TowerPlacing>().isSelected = false;
        SelectedTowerInUI = null;
        //destroy the fake tower
        Debug.Log("deselected the tower");

    }

    public void PlaceTower()
    {
        Tower TowerToPlace = SelectedTowerInUI.GetComponent<TowerPlacing>().thisTower;
        //Instantiate the tower at position of the mouse
        if (CurrMoney >= TowerToPlace.cost)
        {
            GiveMoney(-TowerToPlace.cost); //we give the player the negative amount of the cost
            Vector3 PlacementVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlacementVector = new Vector3(PlacementVector.x, PlacementVector.y, 0);
            GameObject newTower = Instantiate(TowerPrefabs[TowerToPlace.ID], PlacementVector, Quaternion.identity, GameObject.FindWithTag("PlacedTowers").transform);
        }
        else
        {
            //tell the player u dont have money xd
        }
        SelectedTowerInUI = null;
        DT.needsToBeDeselected = false;

    }
}
