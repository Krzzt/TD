using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownTower : TowerTest
{
    public int BottleSlowPercentage;
    public float BottleSize;
    public bool BottleRemoveAura;

    public override void ChangeAddOns(int WhichAddonSwitches, int newAddonID) //i want to change this so every killer has addons and can equip 2 of them at once
    {
        if (equippedAddOns[WhichAddonSwitches] != null)
        {
            switch (equippedAddOns[WhichAddonSwitches].ID) //this reverts the effects of the now unequipped addon
            {
                case 0:
                    //Aura Reading
                    BottleRemoveAura = false;
                    break;
                case 1:
                    AttackSpeed += 0.2f;
                    break;
                case 2:
                    BulletLifeTime -= 1;
                    break;
                case 3:
                    AttackSpeed += 0.4f;
                    break;
                case 4:
                    BottleSlowPercentage -= 10;
                    break;
                case 5:
                    BottleSize -= (BottleSize * (0.2f/1.2f));
                    break;
                case 6:
                    BulletLifeTime -= 2;
                    break;
                case 7:
                    BottleSize -= (BottleSize * (0.2f/1.2f));
                    BottleRemoveAura = false;
                    //Aura Reading
                    break;
                case 8:
                    BottleRemoveAura = false;
                    BottleSize -= (BottleSize * (0.3f / 1.3f));
                    AttackSpeed += 0.4f;
                    //Aura Reading
                    break;
                case 9:
                    BulletLifeTime -= 2;
                    BottleSlowPercentage -= 20;
                    break;
                default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

            }
        }
        equippedAddOns[WhichAddonSwitches] = AddOnList.TowerAddOns[TowerID][newAddonID]; //it works

        switch (equippedAddOns[WhichAddonSwitches].ID) //this gives the effect of the new addon
        {
            case 0:
                //Aura Reading
                BottleRemoveAura = true;
                break;
            case 1:
                AttackSpeed -= 0.2f;
                break;
            case 2:
                BulletLifeTime += 1;
                break;
            case 3:
                AttackSpeed -= 0.4f;
                break;
            case 4:
                BottleSlowPercentage += 10;
                break;
            case 5:
                BottleSize += (BottleSize * 0.2f);
                break;
            case 6:
                BulletLifeTime += 2;
                break;
            case 7:
                BottleSize +=(BottleSize * 0.2f);
                BottleRemoveAura = true;
                //Aura Reading
                break;
            case 8:
                BottleRemoveAura = true;
                BottleSize += (BottleSize * 0.3f);
                AttackSpeed -= 0.4f;
                //Aura Reading
                break;
            case 9:
                BulletLifeTime += 2;
                BottleSlowPercentage += 20;
                break;
            default: break; //if nothing is equipped, we change nothing

        }



        CheckForAuraReading();
    }

   public override void UnEquipAddOn(int equippedAddOnID)
    {
        switch (equippedAddOns[equippedAddOnID].ID) //this reverts the effects of the now unequipped addon
        {
            case 0:
                //Aura Reading
                BottleRemoveAura = false;
                break;
            case 1:
                AttackSpeed += 0.2f;
                break;
            case 2:
                BulletLifeTime -= 1;
                break;
            case 3:
                AttackSpeed += 0.4f;
                break;
            case 4:
                BottleSlowPercentage -= 5;
                break;
            case 5:
                BottleSize = BottleSize - (BottleSize * (0.2f/1.2f));
                break;
            case 6:
                BulletLifeTime -= 2;
                break;
            case 7:
                BottleSize = BottleSize - (BottleSize * (0.2f/1.2f));
                BottleRemoveAura = false;
                //Aura Reading
                break;
            case 8:
                BottleRemoveAura = false;
                BottleSize = BottleSize - (BottleSize * (0.3f/1.3f));
                AttackSpeed += 0.4f;
                //Aura Reading
                break;
            case 9:
                BulletLifeTime -= 2;
                BottleSlowPercentage -= 10;
                break;
            default: Debug.Log("nothing selected"); break; //if nothing is equipped, we change nothing

        }
        equippedAddOns[equippedAddOnID] = null; //dont know if this works yet, i dont think it does
        CheckForAuraReading();
    }


}
