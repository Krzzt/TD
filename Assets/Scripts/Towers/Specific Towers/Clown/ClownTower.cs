using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownTower : TowerTest
{


    new public void ChangeAddOns(int WhichAddonSwitches, int newAddonID) //i want to change this so every killer has addons and can equip 2 of them at once
    {
        if (equippedAddOns[WhichAddonSwitches] != null)
        {
            switch (equippedAddOns[WhichAddonSwitches].ID) //this reverts the effects of the now unequipped addon
            {
                case 0:
                    AttackSpeed += 0.1f;
                    break;
                case 1:
                    Pierce -= 1;
                    break;
                case 2:
                    AttackSpeed += 0.2f;
                    break;
                case 3:
                    //after stunning is implemented
                    break;
                case 4:
                    //after the waves of 2 are implemented
                    break;
                case 5:
                    Pierce -= 4;
                    break;
                case 6:
                    //the aura reading is a bit more complicated so we check for that at the end
                    Damage -= 4;
                    break;
                case 7:
                    //another stun L
                    break;
                case 8:
                    Damage -= 20;
                    break;
                case 9:
                    AttackSpeed *= 2f;
                    Pierce -= 5;
                    //aura reading and stuff
                    break;
                default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

            }
        }
        equippedAddOns[WhichAddonSwitches] = AddOnList.TowerAddOns[TowerID][newAddonID]; //it works

        switch (equippedAddOns[WhichAddonSwitches].ID) //this gives the effect of the new addon
        {
            case 0:
                AttackSpeed -= 0.1f;
                break;
            case 1:
                Pierce += 1;
                break;
            case 2:
                AttackSpeed -= 0.2f;
                break;
            case 3:
                //after stunning is implemented
                break;
            case 4:
                //after the waves of 2 are implemented
                break;
            case 5:
                Pierce += 4;
                break;
            case 6:
                //the aura reading is a bit more complicated so we check for that at the end
                Damage += 4;
                break;
            case 7:
                //another stun L
                break;
            case 8:
                Damage += 20;
                break;
            case 9:
                AttackSpeed /= 2f;
                Pierce += 5;
                //aura reading and stuff
                break;
            default: break; //if nothing is equipped, we change nothing

        }



        CheckForAuraReading();
    }

    new public void UnEquipAddOn(int equippedAddOnID)
    {
        switch (equippedAddOns[equippedAddOnID].ID) //this reverts the effects of the now unequipped addon
        {
            case 0:
                AttackSpeed += 0.1f;
                break;
            case 1:
                Pierce -= 1;
                break;
            case 2:
                AttackSpeed += 0.2f;
                break;
            case 3:
                //after stunning is implemented
                break;
            case 4:
                //after the waves of 2 are implemented
                break;
            case 5:
                Pierce -= 4;
                break;
            case 6:
                //the aura reading is a bit more complicated so we check for that at the end
                Damage -= 4;
                break;
            case 7:
                //another stun L
                break;
            case 8:
                Damage -= 20;
                break;
            case 9:
                AttackSpeed *= 2f;
                Pierce -= 5;
                //aura reading and stuff
                break;
            default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

        }
        equippedAddOns[equippedAddOnID] = null; //dont know if this works yet, i dont think it does
        CheckForAuraReading();
    }


}
