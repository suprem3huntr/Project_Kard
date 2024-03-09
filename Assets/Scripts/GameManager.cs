using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public bool turn = false;

    public Player player;

    public IdleState idleState = new IdleState();
    public DrawState drawState = new DrawState();
    
    public AttackState attackState = new AttackState();
    
    public TargetState targetState = new TargetState();
    
    public GuardState guardState = new GuardState();
    
    public PlayState playState = new PlayState();
    public State currState;

    [SerializeField]
    private GameObject handUI;
    [SerializeField]
    private GameObject playerFrontUI;
    [SerializeField]
    private GameObject playerBackUI;
    [SerializeField]
    private GameObject enemyFrontUI;
    [SerializeField]
    private GameObject enemyBackUI;
    [SerializeField]
    private GameObject enemyHandUI;
    [SerializeField]
    private GameObject cardPrefab;

    List<GameObject> allUIs = new List<GameObject>();

    public int cardCreated;
    public List<GameObject> cardsInPlay = new List<GameObject>();

    [SerializeField] private Button startButton;

    void Awake()
    {
         
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        startButton.onClick.AddListener(() =>
        {
            StartMatch();
        });
    }
    void Start()
    {
        cardDatabase = Resources.Load<CardDatabase>("Card_Database");
        currState = idleState;
        
    }

    
    public void EndTurn()
    {
        Debug.Log("hm");
        if (turn)
        {

            turn = false;
        }
        else
        {

            turn = true;
        }
    }

    public void addtoUI(int card,int UI)
    {
        
        cardCreated = card;
        cardsInPlay.Add(Instantiate(cardPrefab,allUIs[UI].transform));
        cardsInPlay.Last().GetComponent<CardInstance>().row = UI;
        removeLastCardinPlay();
    }
    
    public void moveUI(int from,int to,int indexFrom)
    {
        GameObject card = allUIs[from].transform.GetChild(indexFrom).gameObject;
        card.transform.SetParent(allUIs[to].transform);
        card.transform.localPosition = new Vector3(0,0,0);
        card.transform.localScale = new Vector3(1,1,1);
        card.transform.localRotation = Quaternion.identity;
        card.GetComponent<CardInstance>().row = to;
    }
    
    public void removeLastCardinPlay()
    {
        cardsInPlay.RemoveAt(cardsInPlay.Count-1);
    }
    
    public void loadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
    public void StartMatch()
    {
        Debug.Log("Attempting Match Start");
        if (GameObject.FindGameObjectsWithTag("Player").Length != 2)
        {
            Debug.Log("Not enough players");
            return;
        }
        else
        {
            player.NetworkSceneLoad("GautamPrototyping");
        }
    }
    public void AddUIs()
    {
        handUI = GameObject.FindGameObjectWithTag("handUI");
        playerFrontUI = GameObject.FindGameObjectWithTag("playerFrontUI");
        playerBackUI = GameObject.FindGameObjectWithTag("playerBackUI");
        enemyHandUI = GameObject.FindGameObjectWithTag("enemyHandUI");
        enemyFrontUI = GameObject.FindGameObjectWithTag("enemyFrontUI");
        enemyBackUI = GameObject.FindGameObjectWithTag("enemyBackUI");
        allUIs.Add(handUI);
        allUIs.Add(playerFrontUI);
        allUIs.Add(playerBackUI);
        allUIs.Add(enemyHandUI);
        allUIs.Add(enemyFrontUI);
        allUIs.Add(enemyBackUI);
    }
}
