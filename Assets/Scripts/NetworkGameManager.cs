using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using Unity.Netcode;
using System.Linq;

public class NetworkGameManager : NetworkBehaviour
{
    System.Random rand = new System.Random();
    private Player player;
    private List<GameObject> allPlayers;
    private int starter;


    public override void OnNetworkSpawn()
    {

        if(!IsServer) return;
        ulong[] clientIds = {0,1};
        starter = rand.Next(1);
        Invoke("StartGameClientRpc",3);
        
    }
    
    [ClientRpc]
    private void StartGameClientRpc()
    {
        player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();
        player.StartGame((ulong)starter);
    }

    [ClientRpc]
    private void EndTurnClientRpc()
    {
        player.EndTurn();
    }

    public NetworkList<int> Shuffle(NetworkList<int> deck)
    {
        
        for(int i=deck.Count;i>0;i--)
        {
            var k = rand.Next(i);
            var value = deck[k];
            deck[k] = deck[i-1];
            deck[i-1] = value;
        }
        return deck;
    }
    
}
