using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInstance : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    GameManager gameManager;
    public int row;
    public Card card;
    public int atk;
    public int def;
    public Sprite image;
    public Sprite namePlate;
    public string cardName;
    public int cost;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        int displayID = gameManager.cardCreated;
        card = gameManager.cardDatabase.GetCard(displayID);
        atk = card.atk;
        def = card.def;
        name = card.name;
        image = card.cardImage;
        namePlate = card.cardNameplate;
        cost = card.cost;
    }

    public void Attack(CardInstance target){
        Debug.log("Card " + target.card.nameText + " is hit by " + card.nameText);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameManager.currState.currState == States.PlayState && row<3){
            if(!gameManager.currState.isCondition()){
                gameManager.currState.doAction();
                gameManager.selectedCard = this;
            }
        }
        if(gameManager.currState.currState == States.AttackState && gameManager.attackTargets.Keys().Contains(this) && row == 1){
            gameManager.currState = gameManager.attackTargetState;
            gameManager.selectedCard = this;
        }
        if(gameManager.currState.currState == States.AttackTargetState && row>3){
            gameManager.attackTargets[gameManager.selectedCard] = this;
            gameManager.currState = gameManager.currState.NextState();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
