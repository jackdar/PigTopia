using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Player")]
    [SerializeField]
    NetworkPlayer playerPrefab;

    InputHandler inputHandler;

    //called when a player joins the network
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerJoined"); //log message when player joins game

        if (runner.IsServer)
        {
            //spawn a player at a random position on the map
            NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity, player);
            spawnedNetworkPlayer.playerState = NetworkPlayer.PlayerState.connected;
        }
    }

    //called when network input is received
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if(inputHandler == null && NetworkPlayer.Local != null)
        {
            //if inputHandler is not set and a local player exists, get the InputHandler component
            inputHandler = NetworkPlayer.Local.GetComponent<InputHandler>();
        }
        if(inputHandler != null)
        {
            // set the network input to the input received from the InputHandler
            input.Set(inputHandler.GetNetworkInput());
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { Utils.DebugLog("OnConnectedToServer"); }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { Utils.DebugLog("OnPlayerLeft"); }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Utils.DebugLog("OnShutdown"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Utils.DebugLog("OnDisconnectFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Utils.DebugLog("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Utils.DebugLog("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    

    

    
}
