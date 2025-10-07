using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDeselection : MonoBehaviour
{

    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnMouseEnter()
    {
        gm.isOverUpgrades = true;   
    }

    private void OnMouseExit()
    {
        gm.isOverUpgrades = false;
    }
}
