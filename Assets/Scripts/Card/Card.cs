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
    public string description;
    public Sprite sprite;

}
