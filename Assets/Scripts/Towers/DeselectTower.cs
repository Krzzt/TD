using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;




//BTW we use the fucking Z-Axis to put the collision of this deadzone "behind" the tower UI because thats how it fucking works i guess
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


    private void OnMouseOver()
    {
       if (needsToBeDeselected)
        {
            gm.DeselectTowerUI();
            needsToBeDeselected= false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);

        }

    }

    private void OnMouseExit()
    {
        if (gm.SelectedTowerInUI != null)
        {
            needsToBeDeselected = true;

        }

    }

}
