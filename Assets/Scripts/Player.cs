using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Player : NetworkBehaviour
{
    public NetworkVariable<int> mana = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> health = new NetworkVariable<int>(20,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
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
            DrawCard(false);
        }
        if(starter == OwnerClientId)
        {
            gameManager.turn = true;
            gameManager.attackState.doAction();

           

            gameManager.currState = gameManager.drawState;
            gameManager.currState.ResetVariable(2);
            gameManager.currState.SetNextState(gameManager.playState);
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
        if(from==to){
            
        }
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

    public void DrawCard(bool isMana)
    {
        
        if(isMana)
        {
            mana.Value++;
        }
        else
        {
            addtoRow(deck[0],0);
            deck.RemoveAt(0); 
        }
        
    }
    public void CommitAttack()
    {
        List<int> indexes = new List<int>();
        List<int> targetindexes = new List<int>();
        List<int> targetRows = new List<int>();
        foreach(CardInstance key in gameManager.attackTargets.Keys){
            indexes.Add(key.gameObject.transform.GetSiblingIndex());
            List<int> temp = new List<int>();
            targetRows.Add((gameManager.attackTargets[key].row+3)%6);
            targetindexes.Add(gameManager.attackTargets[key].gameObject.transform.GetSiblingIndex());
            ;
            key.Attack(gameManager.attackTargets[key]);
        }
        foreach(CardInstance val in gameManager.attackTargets.Values)
        {
            val.Damage(val.damageBuildup);
            val.damageBuildup = 0;
        }
        gameManager.attackTargets.Clear();
    
        attackServerRpc(indexes.ToArray(),targetRows.ToArray(),targetindexes.ToArray());
    }

    public void NetworkSceneLoad(string s)
    {
        if(!IsServer) return;
        NetworkManager.Singleton.SceneManager.LoadScene(s,LoadSceneMode.Single);
    }

    public void Damage(int dmg)
    {
        if(!IsOwner) return;
        health.Value -= dmg;
    }
    public void rowAttack()
    {
        rowDamageServerRpc(gameManager.rowAccum);
        
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

    [ServerRpc]
    private void attackServerRpc(int[] indexes,int[] targetRows,int[] targetindexes,ServerRpcParams serverRpcParams = default)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        attackClientRpc(indexes,targetRows,targetindexes,clientRpcParams);
    }

    [ClientRpc]
    private void attackClientRpc(int[] indexes,int[] targetRows,int[] targetindexes,ClientRpcParams clientRpcParams = default)
    {
        Dictionary<CardInstance,CardInstance> attackdict = new Dictionary<CardInstance,CardInstance>();
        
        for(int i=0;i<indexes.Length;i++){
            
        
        
            CardInstance attacker = gameManager.allUIs[4].transform.GetChild(indexes[i]).GetComponent<CardInstance>();
            CardInstance target = gameManager.allUIs[targetRows[i]].transform.GetChild(targetindexes[i]).GetComponent<CardInstance>();
            attacker.Attack(target);
            Debug.Log(i + " adds " + attacker.atk+" "+target.damageBuildup);
            
        }
        for(int i=0;i<indexes.Length;i++)
        {
            CardInstance target =  gameManager.allUIs[targetRows[i]].transform.GetChild(targetindexes[i]).GetComponent<CardInstance>();
            target.Damage(target.damageBuildup);
            target.damageBuildup = 0;
        }
    }
    [ServerRpc]
    private void rowDamageServerRpc(int dmg,ServerRpcParams serverRpcParams = default)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{1-serverRpcParams.Receive.SenderClientId}
                
            }
        };
        rowDamageClientRpc(dmg,clientRpcParams);
    }

    [ClientRpc]
    private void rowDamageClientRpc(int dmg,ClientRpcParams clientRpcParams = default)
    {
        otherPlayer.Damage(dmg);
    }
    
    
}
