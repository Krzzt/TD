using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerPlacing : MonoBehaviour
{
    public bool isSelected;
    public int ID;
    private GameManager gm;
    public Tower thisTower;

    public GameObject MockTower;

    

    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        thisTower = TowerList.ListOfTowers[ID];
        //test
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //how to place a tower
        if (isSelected && Input.GetMouseButtonUp(0)) //if the tower is selected and the mouse button is no longer held
        {
            isSelected = false;
            gm.PlaceTower(); //we place the tower
            Destroy(gm.MockTower);
            GameObject Deadzone = GameObject.FindWithTag("Deadzone");
            Deadzone.transform.position = new Vector3(Deadzone.transform.position.x, Deadzone.transform.position.y, 1);

            //kill the mock tower
            //place tower
        }
    }


    private void OnMouseDown() //on click of this collider
    {
        isSelected = true;
        gm.SelectedTowerInUI = gameObject;
        Vector3 PlacementVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlacementVector = new Vector3(PlacementVector.x, PlacementVector.y, 0);
        gm.MockTower = Instantiate(gm.MockTowerPrefabs[ID],PlacementVector, Quaternion.identity);
        //we select this tower as currently selected
        GameObject Deadzone = GameObject.FindWithTag("Deadzone");
        Deadzone.transform.position = new Vector3(Deadzone.transform.position.x, Deadzone.transform.position.y, -1);

        //also generate the mock tower

        //and PLEASE set the Z of every tower Art to like 2 or smth. so it doesnt affect the deadzones. AFter the tower is deselected or placed, set the z to 0 again
    }
    


}
