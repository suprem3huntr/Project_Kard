using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button endPhaseButton;
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
            foreach(CardInstance key in gameManager.attackTargets.Keys){
                key.Attack(gameManager.attackTargets[key]);
            }
            gameManager.attackTargets.Clear();
        }
    }
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }
}
