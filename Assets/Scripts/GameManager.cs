using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    public bool TowerIsInPlaceablePos;

    public GameObject MockTower;

    private TMP_Text MoneyText;

    public List<ListedSprites> AddOnSprites;
    private int[][] AddOnQuantities;

    public List<Image> AddOnImages;
    public List<TMP_Text> AddOnCountTexts;

    public List<Image> EquippedAddOnImages;

     [NonSerialized] public bool isOverUpgrades;

    [NonSerialized] public GameObject UpgradeArea;

    private GameObject EquippedAddOnSelectorObject;

    public int EquippedAddOnSelected;

    private GameObject AddOnSelectorObject;
    public int AddOnSelected;

    public TMP_Text TowerNameText;
    private TMP_Text AddOnNameText;
    private TMP_Text AddOnDescriptionText;
    private TMP_Text AddOnCostText;
    private void Awake()
    {
        DT = GameObject.FindWithTag("Deadzone").GetComponent<DeselectTower>();
        MoneyText = GameObject.FindWithTag("MoneyText").GetComponent<TMP_Text>();
        UpgradeArea = GameObject.FindWithTag("UpgradeArea");
        EquippedAddOnSelectorObject = GameObject.FindWithTag("EquippedAddOnSelector");
        AddOnSelectorObject = GameObject.FindWithTag("AddOnSelector");

        TowerNameText = UpgradePanel.transform.Find("TowerName").gameObject.GetComponent<TMP_Text>();
        AddOnNameText = UpgradePanel.transform.Find("AddOnName").gameObject.GetComponent<TMP_Text>();
        AddOnDescriptionText = UpgradePanel.transform.Find("AddOnDescription").gameObject.GetComponent<TMP_Text>();
        AddOnCostText = UpgradePanel.transform.Find("AddOnCost").gameObject.GetComponent<TMP_Text>();
        

        AddOnQuantities = new int[10][];
        for(int i = 0; i < AddOnQuantities.Length; i++)
        {
            AddOnQuantities[i] = new int[10];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MoneyText.SetText(CurrMoney + "$");
        UpgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0) && !SelectedTower.GetComponent<TowerTest>().isHovering && !isOverUpgrades) //if you press the left mouse button and are not over the selectedtower
            {

                DeselectTower();
            }
            isOverTower = false; //this is always set false here but the script in the towers runs after that so it can get set true if you hover over a tower
        }
        else 
        { 
        UpgradeArea.SetActive(false);
        }

        if (SelectedTowerInUI != null)
        {
        }
      

    }

    public void GiveMoney(int MoneyToGive)
    {
        CurrMoney += MoneyToGive;
        MoneyText.SetText(CurrMoney + "$");
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
        Destroy(MockTower);
        MockTower = null;
        //destroy the fake tower
        Debug.Log("deselected the tower");

    }

    public void PlaceTower()
    {
        Tower TowerToPlace = SelectedTowerInUI.GetComponent<TowerPlacing>().thisTower;
        Vector3 PlacementVector = Camera.main.ScreenToWorldPoint(Input.mousePosition); //this is the pos of the mouse
        PlacementVector = new Vector3(PlacementVector.x, PlacementVector.y, 0);
        //Instantiate the tower at position of the mouse
        if (CurrMoney >= TowerToPlace.cost)
        {
            if (TowerIsInPlaceablePos)
            {
                GiveMoney(-TowerToPlace.cost); //we give the player the negative amount of the cost

                GameObject newTower = Instantiate(TowerPrefabs[TowerToPlace.ID], PlacementVector, Quaternion.identity, GameObject.FindWithTag("PlacedTowers").transform);
            }
            else
            {
                DamagePopUp.Create(PlacementVector, "No space looser", Color.white);
                Debug.Log("LOOOOSER");
            }
        }
        else
        {
            DamagePopUp.Create(PlacementVector, "U poor lol", Color.white);
            Debug.Log("no money loser");
            //tell the player u dont have money xd
        }
        SelectedTowerInUI = null;
        DT.needsToBeDeselected = false;

    }

    public void SelectEquippedAddOn(int numberSelected)
    {
        EquippedAddOnSelected = numberSelected;
        if (EquippedAddOnSelected <= 1)
        {
            //set the selection thing to select
            EquippedAddOnSelectorObject.SetActive(true);
            EquippedAddOnSelectorObject.transform.position = EquippedAddOnImages[EquippedAddOnSelected].gameObject.transform.position;
        }
        else
        {
            EquippedAddOnSelectorObject.SetActive(false);
            //the selection thingy isnt visible
        }
    }

    public void SelectAddOn(int numberSelected)
    {
        AddOnSelected = numberSelected;
        if (AddOnSelected <= 9)
        {
            AddOnSelectorObject.SetActive(true);
            AddOnSelectorObject.transform.position = AddOnImages[AddOnSelected].gameObject.transform.position;
            int currTowerID = SelectedTower.GetComponent<TowerTest>().TowerID;
            AddOn selectedAddOn = AddOnList.TowerAddOns[currTowerID][numberSelected];
            AddOnNameText.SetText(selectedAddOn.Name);
            AddOnDescriptionText.SetText(selectedAddOn.Description);
            AddOnCostText.SetText(selectedAddOn.cost.ToString() + "$"); //this $ sign will be changed to bloodpoints
        }
        else
        {
            AddOnSelectorObject.SetActive(false);
        }
    }

    public void BuyAddOn()
    {
        int currTowerID = SelectedTower.GetComponent<TowerTest>().TowerID;
        AddOn AddOnToBuy = AddOnList.TowerAddOns[currTowerID][AddOnSelected];
        if (CurrMoney >= AddOnToBuy.cost) //if we have more money than the addOn costs
        {
            GiveMoney(-AddOnToBuy.cost);
            AddOnQuantities[currTowerID][AddOnSelected] += 1;
            SetAddOnUI();
        }
    }
    

    public void SetAddOnUI()
    {
        TowerTest SelectedTowerScript = SelectedTower.GetComponent<TowerTest>();
        
        //this is for the count of the Addons (sets the number at the bottom)
        for (int i = 0; i < AddOnImages.Count; i++)
        {
            AddOnCountTexts[i].SetText(AddOnQuantities[SelectedTower.GetComponent<TowerTest>().TowerID][i].ToString());
            AddOnImages[i].sprite = AddOnSprites[SelectedTower.GetComponent<TowerTest>().TowerID].sprites[i];

        }
        //this sets the currently equipped addons

        for (int i = 0; i < SelectedTowerScript.equippedAddOns.Length; i++)
        {
            if (SelectedTowerScript.equippedAddOns[i] != null)
            {
                EquippedAddOnImages[i].sprite = AddOnSprites[SelectedTowerScript.TowerID].sprites[SelectedTowerScript.equippedAddOns[i].ID];
            }
            else
            {
                EquippedAddOnImages[i].sprite = null;
            }


        }
    }

    
    public void EquipAddOn()
    {
        TowerTest currTower = SelectedTower.GetComponent<TowerTest>();
        if (AddOnSelected <= 9) //if the selected addon is at a number that corresponds to an addon
        {

            if (currTower.equippedAddOns[EquippedAddOnSelected] != null) //and if anything is equipped
            {
                if (currTower.equippedAddOns[EquippedAddOnSelected] != AddOnList.TowerAddOns[currTower.TowerID][AddOnSelected]) //and if the equipped addon isnt the same as the selected addon
                {
                    Debug.Log("YOU SHOULD SEE THIS ON THE GIVEN SITUATION");
                    Debug.Log("THE FOLLOWING IMAGE SHOULD SEE A -1: " + AddOnSelected);
                    Debug.Log("THE FOLLOWING IMAGE SHOULD SEE A +1: " + currTower.equippedAddOns[EquippedAddOnSelected].ID);
                    AddOnQuantities[currTower.TowerID][AddOnSelected] -= 1;
                    AddOnQuantities[currTower.TowerID][currTower.equippedAddOns[EquippedAddOnSelected].ID] += 1;
                    currTower.ChangeAddOns(EquippedAddOnSelected, AddOnSelected); //we change the addons
                    if (currTower.equippedAddOns[0] == currTower.equippedAddOns[1])
                    {
                        UnEquipAddOn();

                    }

                }
                else //if the selected addon and equipped addon are the same
                {
                    UnEquipAddOn(); //unequip it
                }
            }
            else //and if it is null (so nothing is equipped)
            {
                currTower.ChangeAddOns(EquippedAddOnSelected, AddOnSelected); //just equip it normally
                AddOnQuantities[currTower.TowerID][AddOnSelected]--;
                if (currTower.equippedAddOns[0] == currTower.equippedAddOns[1])
                {
                    UnEquipAddOn();
                }
            }

            SetAddOnUI();
        }

    }

    public void UnEquipAddOn()
    {
        Debug.Log("WE UNEQUIPPING");
        TowerTest currTower = SelectedTower.GetComponent<TowerTest>();
        Debug.Log(EquippedAddOnSelected);
        AddOnQuantities[currTower.TowerID][AddOnSelected]++;
        currTower.UnEquipAddOn(EquippedAddOnSelected);
        SetAddOnUI();
    }

}

[System.Serializable] public class ListedSprites
{
    public List<Sprite> sprites;
}
