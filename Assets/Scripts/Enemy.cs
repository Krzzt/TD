using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemySpawner spawner;
    private GameManager gm;

    public int nextMark;

    public float speed;

    public bool isOut;

    public UnitHealth EnemyHealth = new UnitHealth(0, 0);
    public int MaxHealth;

    public float time; //the time the enemy has been alive for
    public float MapCompletion; //a value between 0 and 1 where 0 is the beginning of the track and 1 is the end

    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        spawner = gameObject.transform.parent.gameObject.GetComponent<EnemySpawner>(); //the parent of this object, and we only want the component
        nextMark = 1;
        isOut = false;
        EnemyHealth.addmaxHealth(MaxHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, spawner.Marks[nextMark].transform.position, speed * Time.fixedDeltaTime);
        time += Time.fixedDeltaTime;
        MapCompletion = (speed * time) / gm.MAP_LENGTH;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mark"))
        {
            nextMark++;
            if (nextMark >= spawner.Marks.Count)
            {
                isOut = true;
                LoseLives();
                Destroy(gameObject);
            }
        }
    }

    public void LoseLives()
    {

    }

    public void TakeDamage(int amount)
    {
        EnemyHealth.DamageUnit(amount);
        Debug.Log("took Damage! current Health: " + EnemyHealth._currentHealth);
        if (EnemyHealth._currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        spawner.EnemiesAlive.Remove(gameObject);
        Destroy(gameObject);
    }
}
