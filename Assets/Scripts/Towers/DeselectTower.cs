using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeselectTower : MonoBehaviour
{

    private GameManager gm;

    public bool needsToBeDeselected;

    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseEnter()
    {
       if (needsToBeDeselected)
        {
            gm.DeselectTowerUI();
            needsToBeDeselected= false;
        }
    }

    private void OnMouseExit()
    {
        if (gm.SelectedTowerInUI != null)
        {
            needsToBeDeselected = true;
        }

    }

    //okay let me write out the problem: i want a "deadzone" that if you go to it, it deselects a tower you selected. That deadzone is the tower selection UI. SO it will get deselected instantly
    //because im always in the deadzone. So we need to check if you leave the deadzone with a tower selected an when you enter it again
}
