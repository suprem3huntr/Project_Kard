using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card",menuName = "Cards/Character Card")]
public class Card : ScriptableObject
{
    public int id = -1;
    public string cardName;
    public int cost;
    public int atk;
    public int def;
    public string characterDescription;
    public string description;
    public Sprite cardImage;
    public Sprite cardNameplate;
    public bool Ability;
    public CostType[] costTypes;
    public int[] costCounts;
    public EffectType[] effectTypes;
    public int[] effectValues;
    public TargetType[] targetTypes;
    public TriggerType[] triggerTypes;
    public SummonLocation summonLocation;
    public int summonCost;
    public string toolTip;

}
