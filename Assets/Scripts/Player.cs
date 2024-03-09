using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    
    public NetworkList<int> deck;
    public NetworkList<int> hand;
    public NetworkList<int> backrow;
    public NetworkList<int> frontrow;

    [SerializeField]
    private List<NetworkList<int>> allRows = new List<NetworkList<int>>();
    
    PlayerManager playerManager;
    NetworkGameManager networkgameManager;
    GameManager gameManager;
    public Player otherPlayer;

    void Awake()
    {

        deck = new NetworkList<int>(null,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        hand = new NetworkList<int>(null,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        backrow = new NetworkList<int>(null,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        frontrow = new NetworkList<int>(null,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
    }
    
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        gameManager.player = this;
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
        
        List<int> tempdeck = playerManager.deck;
        for (int i=0;i<tempdeck.Count;i++)
        {
            deck.Add(tempdeck[i]);
        }
        
    }

    public void EndTurn()
    {
        gameManager.EndTurn();
        endTurnServerRpc();
        
    }
    
    public void StartGame(ulong starter)
    {
        networkgameManager = GameObject.FindGameObjectWithTag("NetworkGameManager").GetComponent<NetworkGameManager>();
        gameManager.AddUIs();
        deck = networkgameManager.Shuffle(deck);
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(i != gameObject)
            {
                otherPlayer = i.GetComponent<Player>();
                otherPlayer.otherPlayer = this;
                break;
            }
        }
        allRows.Add(hand);
        allRows.Add(frontrow);
        allRows.Add(backrow);
        allRows.Add(otherPlayer.hand);
        allRows.Add(otherPlayer.frontrow);
        allRows.Add(otherPlayer.backrow);
        for(int i=0;i<5;i++)
        {
            addtoRow(deck[0],0);
            deck.RemoveAt(0);
        }
        if(starter == OwnerClientId)
        {
            gameManager.turn = true;

            Debug.Log("Starter");

            gameManager.currState = new DrawState(new PlayState(), 2);
        }
        else{
            gameManager.currState = new IdleState();
        }
        
    }
    public void addtoRow(int i,int row)
    {
        gameManager.addtoUI(i,row);
        addUIServerRpc(i,(row+3)%6);
        switch(row)
        {
            case 0:
                hand.Add(i);
                break;
            case 1:

                frontrow.Add(i);
                break;
            case 2:

                backrow.Add(i);
                break;
            default:
                addToRowServerRpc(row,i);
                break;
        }
        
    }
    public void move(int from,int to,int indexFrom)
    {
        int card = -1;
        switch(from)
        {
            case 0:
                card = hand[indexFrom];
                hand.RemoveAt(indexFrom);
                break;
            case 1:
                card = frontrow[indexFrom];
                frontrow.RemoveAt(indexFrom);
                break;
            case 2:
                card = backrow[indexFrom];
                backrow.RemoveAt(indexFrom);
                break;
            case 3:
                card = otherPlayer.hand[indexFrom];
                removeFromRowServerRpc(from,indexFrom);
                break;
            case 4:
                card = otherPlayer.frontrow[indexFrom];
                removeFromRowServerRpc(from,indexFrom);
                break;
            case 5:
                card = otherPlayer.backrow[indexFrom];
                removeFromRowServerRpc(from,indexFrom);
                break;
                
        }
        
        switch(to)
        {
            case 0:
                hand.Add(card);
                break;
            case 1:

                frontrow.Add(card);
                break;
            case 2:

                backrow.Add(card);
                break;
            default:
                addToRowServerRpc(to,card);
                break;
        }
        gameManager.moveUI(from,to,indexFrom);
        moveUIServerRpc((from+3)%6,(to+3)%6,indexFrom);
        
        
    }

    public void NetworkSceneLoad(string s)
    {
        if(!IsServer) return;
        NetworkManager.Singleton.SceneManager.LoadScene(s,LoadSceneMode.Single);
    }

    [ServerRpc]
    private void addToRowServerRpc(int row,int i,ServerRpcParams serverRpcParams = default)
    {
        
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        addToRowClientRpc(row,i,clientRpcParams);

    }

    [ClientRpc]
    private void addToRowClientRpc(int row, int i, ClientRpcParams clientRpcParams = default)
    {
        switch(row)
        {
            case 3:
        
                otherPlayer.hand.Add(i);
                break;
            case 4:
            
                otherPlayer.frontrow.Add(i);
                break;
            case 5:
            
                otherPlayer.backrow.Add(i);
                break;
        }
    }

    [ServerRpc]
    private void removeFromRowServerRpc(int row,int index,ServerRpcParams serverRpcParams = default)
    {
        
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        removeFromClientRpc(row,index,clientRpcParams);

    }

    [ClientRpc]
    private void removeFromClientRpc(int row, int index, ClientRpcParams clientRpcParams = default)
    {
        switch(row)
        {
            case 3:
        
                otherPlayer.hand.RemoveAt(index);
                break;
            case 4:
            
                otherPlayer.frontrow.RemoveAt(index);
                break;
            case 5:
            
                otherPlayer.backrow.RemoveAt(index);
                break;
        }
    }
    
    [ServerRpc]
    private void moveUIServerRpc(int from ,int to,int indexFrom,ServerRpcParams serverRpcParams = default)
    {
        
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        moveUIClientRpc(from,to,indexFrom,clientRpcParams);
    }

    [ClientRpc]
    private void moveUIClientRpc(int from ,int to,int indexFrom, ClientRpcParams clientRpcParams = default)
    {
        otherPlayer.gameManager.moveUI(from,to,indexFrom);
    }

    [ServerRpc]
    private void addUIServerRpc(int i,int UI,ServerRpcParams serverRpcParams = default)
    {
        
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        addUIClientRpc(i,UI,clientRpcParams);
    }

    [ClientRpc]
    private void addUIClientRpc(int i ,int UI, ClientRpcParams clientRpcParams = default)
    {
        otherPlayer.gameManager.addtoUI(i,UI);
    }

    [ServerRpc]
    private void endTurnServerRpc(ServerRpcParams serverRpcParams = default)
    {
         ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        endTurnClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void endTurnClientRpc(ClientRpcParams clientRpcParams = default)
    {
        otherPlayer.gameManager.EndTurn();
    }
    
    
}
