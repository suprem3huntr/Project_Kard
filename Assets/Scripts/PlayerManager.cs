using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<int> deck = new List<int>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void AddCard(int i)
    {
        if (deck.Count == 60)
        {
            throw new DeckFullException();
        }
        else
        {
            int count = 0;
            for(int j = 0; i<deck.Count;i++)
            {
                if(deck[j] == i)
                {
                    count++;
                }
                if (count == 4)
                {
                    throw new OverStockException(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().cardDatabase.GetCard(i).cardName);
                }
            }
            deck.Add(i);
        }
        
            
        

    }
}


public class DeckFullException : Exception
{
    public DeckFullException() : base("Your deck cannot handle the power of more cards")
    {

    }

    
}

public class OverStockException : Exception
{
    public OverStockException()
    {

    }
    public OverStockException(string cardName) : base ("You already have 4 " + cardName + "s in your deck")
    {

    }
}


