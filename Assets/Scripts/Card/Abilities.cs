using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class Abilities
{
    public delegate void AbilityDelegate();
    public static List<AbilityDelegate> abilityList = new List<AbilityDelegate>();
    
    public static void build()
    {
        
    }
    
}
