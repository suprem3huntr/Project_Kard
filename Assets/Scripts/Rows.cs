using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rows : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    int rowNumber;
    GameManager gameManager;
    
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameManager.currState.currState == States.PlayState && gameManager.currState.isCondition() && rowNumber<3){
            if(gameManager.selectedCard.row == rowNumber){
                gameManager.currState.doAction();
            }
            else if(gameObject.transform.childCount < 3*rowNumber && rowNumber>0){
                gameManager.player.move(gameManager.selectedCard.row, rowNumber, gameManager.selectedCard.gameObject.transform.GetSiblingIndex());
                gameManager.currState.doAction();
            }
        }
    }

    
}
