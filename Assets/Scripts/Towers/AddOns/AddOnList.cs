using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AddOnList
{
    public static List<List<AddOn>> TowerAddOns = new List<List<AddOn>>
    {
        //for Huntress
        new List<AddOn>{
            new AddOn{ID = 0, Name = "Bandaged Haft", Description = "Attack speed -0.1", cost = 100, rarity = 0, givesAuraReading = false },
            new AddOn{ID = 1, Name = "Yellowed Cloth", Description = "Pierce +1", cost = 150, rarity = 0, givesAuraReading = false},
            new AddOn{ID = 2, Name = "Oak Haft", Description = "Attack speed -0.2", cost = 500, rarity = 1, givesAuraReading = false},
            new AddOn{ID = 3, Name = "Weighted Haft", Description = "Survivors are briefly stunned when hitting them", cost = 400, rarity = 1, givesAuraReading = false},
            new AddOn{ID = 4, Name = "Deerskin Gloves", Description = "Now shoots Hatchets in Waves of 2", cost = 1000, rarity = 2 , givesAuraReading = false},
            new AddOn{ID = 5, Name = "Rose Root", Description = "Pierce +4", cost = 1200, rarity = 2, givesAuraReading = false},
            new AddOn{ID = 6, Name = "Glowing Concoction", Description = "Reveals Auras and gives +4 Damage", cost = 2000, rarity = 3, givesAuraReading = true},
            new AddOn{ID = 7, Name = "Begrimed Head", Description = "Survivors are stunned when hitting them", cost = 2500, rarity = 3, givesAuraReading = false},
            new AddOn{ID = 8, Name = "Iridescent Head", Description = "+20 Damage", cost = 6000, rarity = 4, givesAuraReading = false},
            new AddOn{ID = 9, Name = "Soldier's Puttee", Description = "Attack Speed -50%, +5 Pierce, gives Aura Reading", cost = 5000, rarity = 4, givesAuraReading = true}
        
        },
    };
}


public class AddOn
{
    public int ID;
    public string Name;
    public string Description;
    public int cost;
    public int rarity; //0 = Brown, 1 = Green, 2 = Blue, 3 = Purple, 4 = Iri
    public bool givesAuraReading;
}
