using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour,IPointerClickHandler
{
    private GameManager gameManager;
    [SerializeField]
    bool isMana;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(gameManager.currState.currState == States.DrawState){
            Debug.Log("Fuck");
            gameManager.currState.doAction();
            gameManager.player.DrawCard(isMana);
            if(gameManager.currState.isCondition()){
                gameManager.currState = gameManager.currState.NextState();
            }
        }
    }

}
