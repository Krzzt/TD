using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerList
{
    public static List<Tower> ListOfTowers = new List<Tower>
    {
        new Tower{ID = 0, Name = "Huntress", cost = 200},
        new Tower{ID = 1, Name = "Clown", cost = 250},
    };
}


public class Tower
{
    public int ID;
    public string Name;
    public int cost;
}
