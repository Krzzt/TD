using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockTower : MonoBehaviour
{

    private GameManager gm;
    private GameObject RangeIndicator;


    private int framebuffer;
    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gm.TowerIsInPlaceablePos = true;
        RangeIndicator = gameObject.transform.GetChild(0).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);

        if (gm.TowerIsInPlaceablePos)
        {
            framebuffer++; //this framebuffer fixes the issue with overlapping collisions (mostly)
        }

        if (gm.TowerIsInPlaceablePos && framebuffer >= 4)
        {
            RangeIndicator.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5254f);
        }
        else
        {
            RangeIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5254f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Projectile")) //we dont want projectiles (like Clown Bottles) to prevent us from placing towers
        {
            gm.TowerIsInPlaceablePos = false;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Projectile"))
        {
            gm.TowerIsInPlaceablePos = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Projectile"))
        {
            gm.TowerIsInPlaceablePos = true;
            framebuffer = 0;
        }

    }
}
