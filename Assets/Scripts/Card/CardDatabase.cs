using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data.Common;

[CreateAssetMenu(fileName = "Card Database",menuName ="Cards/Database")]
public class CardDatabase : ScriptableObject
{
    [SerializeField] private List<Card> allCards;

    [ContextMenu(itemName:"Set IDs")]
    private void SetItemIDs()
    {
        allCards = new List<Card>();

        var foundCards = Resources.LoadAll<Card>("Cards").OrderBy(i => i.id).ToList();
        var hasIDinRange = foundCards.Where(i => i.id != -1 && i.id <foundCards.Count).OrderBy(i => i.id).ToList();
        var hasIDNotinRange = foundCards.Where(i => i.id != -1 && i.id >= foundCards.Count).OrderBy(i => i.id).ToList();
        var noID = foundCards.Where(i => i.id <= -1).OrderBy(i => i.id).ToList();

        var index = 0;
        for (int i=0; i<foundCards.Count;i++)
        {
            Card cardToAdd;
            cardToAdd = hasIDinRange.Find(d => d.id == i);

            if (cardToAdd != null)
            {
                allCards.Add(cardToAdd);
            }
            else if (index <= noID.Count)
            {
                noID[index].id = i;
                cardToAdd = noID[index];
                index++;
                allCards.Add(cardToAdd);
            }
        }
        foreach(var item in hasIDNotinRange)
        {
            allCards.Add(item);
        }
    }
    public Card GetCard(int index)
    {
        return allCards[index];
    }
    
    
}
