using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInstance : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    GameManager gameManager;
    public int row;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        if(gameManager.state == States.PlayerTurn)
        {
            if(parent.name == "PlayerHand")
            {
                gameManager.state = States.PlayerSelectMove;
                gameManager.cardsInPlay.Add(gameObject);
            }
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
