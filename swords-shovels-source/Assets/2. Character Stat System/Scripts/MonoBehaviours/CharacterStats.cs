using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStats_SO characterDefinition;
    public CharacterInventory characterInventory;
    public GameObject characterWeaponSlot;

    #region Constructor

    public CharacterStats()
    {
        characterInventory = CharacterInventory.instance;
    }

    #endregion

    #region Initialisation

    private void Start()
    {
        if (!characterDefinition.SetManually)
        {
            characterDefinition.maxHealth = 100;
            characterDefinition.currentHealth = 50;

            characterDefinition.maxMana = 25;
            characterDefinition.currentMana = 10;

            characterDefinition.maxWealth = 500;
            characterDefinition.currentWealth = 0;

            characterDefinition.baseResistance = 0;
            characterDefinition.currentResistance = 0;

            characterDefinition.maxEncumbrance = 50f;
            characterDefinition.currentEncumbrance = 0f;

            characterDefinition.charExperience = 0;
            characterDefinition.charLevel = 1;
        }
    }
    
    #endregion

    #region CURRENTLY COMMENTED OUT Updates

    // private void Update() 
    // {
    //     if (Input.GetMouseButtonDown(2))
    //     {
    //         characterDefinition.SaveDataOnClose();
    //     }    
    // }

    #endregion

    #region Stat Increasers

    public void ApplyHealth(int healthAmount)
    {
        characterDefinition.ApplyHealth(healthAmount);
    }

    public void ApplyMana(int manaAmount)
    {
        characterDefinition.ApplyMana(manaAmount);
    }

    public void GiveWealth(int wealthAmount)
    {
        characterDefinition.GiveWealth(wealthAmount);
    }
    
    #endregion

    #region Stat Reducers
    
    public void TakeDamage(int damageAmount)
    {
        characterDefinition.TakeDamage(damageAmount);
    }

    public void TakeMana(int manaAmount)
    {
        characterDefinition.TakeMana(manaAmount);
    }

    #endregion

    #region Character Items and Weapons
    
    public void ChangeWeapon(ItemPickUp weaponPickUp)
    {
        if (!characterDefinition.UnEquipWeapon(weaponPickUp, characterInventory, characterWeaponSlot))
        {
            characterDefinition.EquipWeapon(weaponPickUp, characterInventory, characterWeaponSlot);
        }
        else
        {
            characterDefinition.EquipWeapon(weaponPickUp, characterInventory, characterWeaponSlot);
        }
    }

    public void ChangeArmor(ItemPickUp armorPickup)
    {
        if (!characterDefinition.UnEquipArmor(armorPickup, characterInventory))
        {
            characterDefinition.EquipArmor(armorPickup, characterInventory);
        }
        else
        {
            characterDefinition.EquipArmor(armorPickup, characterInventory);
        }
    }

    #endregion
    
    #region Reporters
    
    public int GetHealth()
    {
        return characterDefinition.currentHealth;
    }

    public ItemPickUp GetCurrentWeapon()
    {
        return characterDefinition.weapon;
    }

    #endregion
}
   

