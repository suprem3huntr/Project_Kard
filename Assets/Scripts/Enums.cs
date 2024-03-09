using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostType
{
    None,
    Mana,
    Troop,
    Hand,
    Graveyard,
}
public enum EffectType
{
    None,
    Draw,
    Summon,
    AtkBoost,
    Heal,
    Damage,
}
public enum TargetType
{
    Player,
    Troop,
    Row,
}
public enum TriggerType
{
    OnPlace,
    OnAttack,
    OnHit,
    OnDeath,
    OnTrigger,
}

public enum SummonLocation
{
    Graveyard,
    OpponentGraveyard,
    Hand,
    Deck,
    Instant,
    OpponentHand,
    OpponentDeck,
}


