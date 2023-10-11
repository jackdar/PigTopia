using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    [Header("Food")]
    [SerializeField]
    GameObject foodPrefab;

    [Header("Player")]
    [SerializeField]
    NetworkPlayer playerPrefab;

    // Other components
    InputHandler inputHandler;
    bool isSpawned = false;

    public void SpawnFood()
    {
        for (int i = 0; i < 300; i++)
        {
            NetworkObject SpawnedGameObject = Runner.Spawn(foodPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
            SpawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
        }
        isSpawned = true;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerJoined");

        if (runner.IsServer)
        {
            NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity, player);
            spawnedNetworkPlayer.playerState = NetworkPlayer.PlayerState.connected;
            if (!isSpawned)
            {
                SpawnFood();
            }
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputHandler == null && NetworkPlayer.Local != null)
            inputHandler = NetworkPlayer.Local.GetComponent<InputHandler>();

        if (inputHandler != null)
            input.Set(inputHandler.GetNetworkInput());
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