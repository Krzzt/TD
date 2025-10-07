using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> Marks; //all the marks on the map
    public List<GameObject> AllEnemies; //a list of every Enemy

    public List<GameObject> EnemiesAlive; //A List of Enemies Alive

    public int WaveCounter;
    public float WaveTimer;
    public float TimeLeftInWave;
    public bool EveryEnemySpawned;

    private TMP_Text WaveText;
    private void Awake()
    {
        WaveText = GameObject.FindWithTag("WaveText").GetComponent<TMP_Text>();
        Marks = GameObject.FindGameObjectsWithTag("Mark").ToList<GameObject>(); //we set the marks
        for (int a = 0; a < Marks.Count; a++)
        {

            for(int i = 0; i < Marks.Count; i++) //for every Mark
            {
                GameObject currObject = Marks[i]; //we save the current mark
                string numberstr = currObject.name.Substring(4); //get the number of it from the name
                int number = int.Parse(numberstr,System.Globalization.NumberStyles.Integer); //convert it into a number
                GameObject temp = Marks[number]; //save the Mark of said number in temp
                Marks[number] = currObject; //convert the mark at the number to the object of this place
                Marks[i] = temp; //and switch places
            }

        }
        NextWave();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeLeftInWave -= Time.fixedDeltaTime;

        if (EnemiesAlive.Count <= 0 && EveryEnemySpawned)
        {
            Debug.Log("AAAAAAAA");
            NextWave();
        }

    }

    public IEnumerator SpawnEnemies(int wave)
    {
        yield return new WaitForSeconds(2.5f); //wait for 2.5 secs between the waves

        switch (wave)
        {
            case 1:
                for (int i = 0; i < 5; i++)
                {
                    SpawnEnemy(0);
                    yield return new WaitForSeconds(0.6f);
                }
                EveryEnemySpawned = true;

                break;
            case 2:
                for (int i = 0; i < 10; i++)
                {
                    SpawnEnemy(0);
                    yield return new WaitForSeconds(0.1f);
                }
                EveryEnemySpawned = true;
                break;
        }
    }


    public void SpawnEnemy(int ID)
    {
        GameObject SpawnedEnemy = Instantiate(AllEnemies[ID], Marks[0].transform.position, Quaternion.identity, gameObject.transform); //and we instantiate it
        EnemiesAlive.Add(SpawnedEnemy);
    }
    public void NextWave()
    {
        EveryEnemySpawned = false;
        WaveCounter++;
        WaveText.SetText("Wave " + WaveCounter);
        StartCoroutine(SpawnEnemies(WaveCounter));
    }
}
