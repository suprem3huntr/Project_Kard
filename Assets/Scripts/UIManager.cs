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
    [SerializeField]
    private TMP_Text playerHealth;
    [SerializeField]
    private TMP_Text enemyHealth;
    private GameManager gameManager;
    void Awake(){
        endPhaseButton.onClick.AddListener(EndPhase);
    }
    void EndPhase(){
        if(gameManager.currState.currState == States.PlayState){
            gameManager.currState = gameManager.currState.NextState();
            if(gameManager.currState.isCondition())
            {
                gameManager.attackState.doAction();
                EndPhase();
            }
        }
        else if(gameManager.currState.currState == States.AttackState){
            gameManager.currState = gameManager.currState.NextState();
            if(gameManager.allUIs[4].transform.childCount!=0)
            {
                gameManager.player.CommitAttack();
            }
            else
            {
                gameManager.player.rowAttack();
            }
            
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
        playerHealth.text = gameManager.player.health.Value.ToString();
        enemyHealth.text = gameManager.player.otherPlayer.health.Value.ToString();
    }
}
