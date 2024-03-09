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
        
    }

    
}
