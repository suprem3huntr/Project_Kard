using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button endPhaseButton;
    [SerializeField]
    private TMP_Text currentState;
    private GameManager gameManager;
    void Awake(){
        endPhaseButton.onClick.AddListener(EndPhase);
    }
    void EndPhase(){
        if(gameManager.currState.currState == States.PlayState){
            gameManager.currState = gameManager.currState.NextState();
        }
        else if(gameManager.currState.currState == States.AttackState){
            gameManager.currState = gameManager.currState.NextState();
            gameManager.player.CommitAttack();
            gameManager.player.EndTurn();
        }
    }
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        currentState.text = gameManager.currState.currState.ToString();
    }
}
