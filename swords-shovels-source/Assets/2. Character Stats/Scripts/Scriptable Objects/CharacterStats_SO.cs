using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName="NewStat", menuName="Character/Stats", order=1)]
public class CharacterStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharLevelUps
    {
        public int maxHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistance;
        public float maxEncumbrance;
    }

    #region Fields Variable

    public string characterName;

    public bool SetManually = false;
    public bool SaveDataOnClose = false;

// Health variables
    public int maxHealth = 0;
    public int currentHealth = 0;

// Mana variables
    public int maxMana = 0;
    public int currentMana = 0;

// ItemPickUp variables
    public ItemPickUp weapon {get;private set;}
    public ItemPickUp headArmor {get;private set;}
    public ItemPickUp chestArmor {get;private set;}
    public ItemPickUp handArmor {get;private set;}
    public ItemPickUp legArmor {get;private set;}
    public ItemPickUp footArmor {get;private set;}
    public ItemPickUp misc1 {get;private set;}
    public ItemPickUp misc2 {get;private set;}

// Character wealth variables
    public int maxWealth = 0;
    public int currentWealth = 0;

    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0f;
    public float currentResistance =0f;

    public float maxEncumbrance = 0f;
    public float currentEncumbrance = 0f;

// Character Level
    public int charLevel = 0;
    public int charExperience = 0;

    public CharLevelUps[] charLevelUps;

    #endregion

    #region Stat Increasers
    
    public void ApplyHealth(int healthAmount)
    {
        if ((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }
    }
        
    public void ApplyMana(int manaAmount)
    {
        if ((currentMana + manaAmount) > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }
    }

    public void GiveWealth(int wealthAmount)
    {
        if ((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }

    #endregion

    #region Stat Reducers
    
    public void TakeDamage(int damageAmount)
    {
        if ((currentHealth - damageAmount) <= 0)
        {
            Death();
        }
        else
        {
            currentHealth -= damageAmount;
        }
    }

    public void TakeMana(int manaAmount)
    {
    if ((currentMana - manaAmount) <= 0)
    {
        currentMana = 0;
    }
    else
    {
        currentMana -= manaAmount;
    }
    }

    #endregion

    #region Character Item/Weapon Methods
    
    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        weapon = weaponPickUp;
        currentDamage = baseDamage + weapon.itemDefinition.itemAmount;
    }

    public void EquipArmor(ItemPickUp armorPickup, CharacterInventory characterInventory)
    {
        switch (armorPickup.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                headArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Chest:
                chestArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Hands:
                handArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Legs:
                legArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Boots:
                footArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
        }
    }

    public bool UnEquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        bool previousWeaponSame = false;
        if (weapon != null)
        {
            if (weapon == weaponPickUp)
            {
                previousWeaponSame = true;
            }

            DestroyObject(weaponSlot.transform.GetChild(0).gameObject);
            weapon = null;
            currentDamage = baseDamage;
        }
        return previousWeaponSame;
    }

    public bool UnEquipArmor(ItemPickUp armorPickup, CharacterInventory characterInventory)
    {
        bool previousArmorSame = false;
        switch (armorPickup.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                if (headArmor != null)
                {
                    if (headArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickup.itemDefinition.itemAmount;
                    headArmor = null;
                }
                break;
            case ItemArmorSubType.Chest:
                if (chestArmor != null)
                {
                    if (chestArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickup.itemDefinition.itemAmount;
                    chestArmor = null;
                }
                break;
            case ItemArmorSubType.Hands:
               if (handArmor != null)
                {
                    if (handArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickup.itemDefinition.itemAmount;
                    handArmor = null;
                }
                break;
            case ItemArmorSubType.Legs:
               if (legArmor != null)
                {
                    if (legArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickup.itemDefinition.itemAmount;
                    legArmor = null;
                }
                break;
            case ItemArmorSubType.Boots:
                if (footArmor != null)
                {
                    if (footArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickup.itemDefinition.itemAmount;
                    footArmor = null;
                }
                break;
        }
        return previousArmorSame;
    }

    #endregion

    #region Character Events

    private void Death()
    {
        Debug.LogFormat("{0} has died.", characterName);
        // TO DO Functionality:
        // 1.Call to Game Manager for Death State to Trigger Respawn
        // 2.Display Death Visualisation
    }

    private void LevelUp()
    {
        charLevel += 1;
        // Display level up visualisation
        
        maxHealth = charLevelUps[charLevel - 1].maxHealth;
        maxMana = charLevelUps[charLevel - 1].maxMana;
        maxWealth = charLevelUps[charLevel - 1].maxWealth;
        baseDamage = charLevelUps[charLevel - 1].baseDamage;
        baseResistance = charLevelUps[charLevel - 1].baseResistance;
        maxEncumbrance = charLevelUps[charLevel -1].maxEncumbrance;

    }
    
    #endregion

    #region CURENNTLY COMMENTED OUT Save Character Data
    
    // public void SaveCharacterData()
    // {
    //     saveDateOnClose = true;
    //     EditorUtility.SetDirty(this);
    // }

    #endregion
}
