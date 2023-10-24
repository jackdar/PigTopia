using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Holds player information and replicates to all clients
public class PlayerNetworkedData : NetworkBehaviour
{
    private const int STARTING_LIVES = 3;
    
    [HideInInspector]
    [Networked(OnChanged = nameof(OnLivesChanged))]
    public int Lives { get; private set; }
    
    [HideInInspector]
    [Networked(OnChanged = nameof(OnScoreChanged))]
    public int Score { get; private set; }

    public override void Spawned()
    {
        // Host
        if (Object.HasStateAuthority)
        {
            Lives = STARTING_LIVES;
            Score = 0;
        }
        
        // Host and Client
        
    }

    // public override void Despawned(NetworkRunner runner, bool hasState)
    // {
    //     
    // }
    
    // Increased score by x amt
    public void AddToScore(int points)
    {
        Score += points;
    }
    
    // Decrease life
    public void SubtractLife()
    {
        Lives--;
    }
    
    // RPC to send player info to host
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName)
    {
        // if (string.IsNullOrEmpty(nickName)) return;
        // NickName = nickName;
    }

    // Update player's score
    public static void OnScoreChanged(Changed<PlayerNetworkedData> playerInfo)
    {
        // playerInfo.Behaviour
        Debug.Log("Score Changed");
    }

    public static void OnLivesChanged(Changed<PlayerNetworkedData> playerInfo)
    {
        Debug.Log("Lives Changed");
    }
}
