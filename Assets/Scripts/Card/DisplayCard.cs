using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DisplayCard : MonoBehaviour
{
    

    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text atkText;
    [SerializeField]
    TMP_Text defText;
    
    public UnityEngine.UI.Image spriteImage;
    public UnityEngine.UI.Image namePlateImage;
    public UnityEngine.UI.Image cardBack;

    private GameManager gameManager;

    private CardInstance instance;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        instance = gameObject.GetComponent<CardInstance>();
    }
    void Start()
    {
        nameText.text = instance.cardName;
        atkText.text = ""+instance.atk;
        defText.text = ""+instance.def;
        spriteImage.sprite = instance.image;
        namePlateImage.sprite = instance.namePlate;    
    }

    void Update()
    {
        
            
            nameText.text = instance.cardName;
            atkText.text = ""+instance.atk;
            defText.text = ""+instance.def;
            spriteImage.sprite = instance.image;
            namePlateImage.sprite = instance.namePlate;
            if(instance.row==3)
            {
                cardBack.color = new Color(1,1,1,1);
                namePlateImage.color = new Color(1,1,1,0);
            }
            else
            {
                cardBack.color = new Color(1,1,1,0);
                namePlateImage.color = new Color(1,1,1,1);
            }
            
        
        
        
        

    }    

    
}
