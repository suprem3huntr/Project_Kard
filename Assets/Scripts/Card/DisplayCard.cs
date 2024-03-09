using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DisplayCard : MonoBehaviour
{
    public int displayID;
    
    private Card card;
    public int id;
    public string cardName;
    public int cost;
    public int atk;
    public int def;
    public string description;
    public Sprite sprite;

    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text atkText;
    public TMP_Text defText;
    public TMP_Text descriptionText;
    public UnityEngine.UI.Image spriteImage;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        displayID = gameManager.cardCreated;
        card = gameManager.cardDatabase.GetCard(displayID);
        

       
        
    }

    
}
