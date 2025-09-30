using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerPlacing : MonoBehaviour
{
    public bool isSelected;
    public int ID;
    private GameManager gm;
    public Tower thisTower;

    


    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        thisTower = TowerList.ListOfTowers[ID];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected && Input.GetMouseButtonUp(0))
        {
            isSelected = false;
            gm.PlaceTower();
            //place tower
        }

    }


    private void OnMouseDown()
    {
        isSelected = true;
        gm.SelectedTowerInUI = gameObject;
    }
    
    public void OnEndDrag()
    {
        isSelected = false;
        gm.PlaceTower();


    }


}
