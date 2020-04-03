using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations

    public static CharacterInventory instance;

    #endregion

    private void Start()
    {
        instance = this;
        
    }
}
