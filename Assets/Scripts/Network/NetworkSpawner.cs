using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    [Header("Prefabs")]
    [SerializeField]
    NetworkPlayer playerPrefab;
    [SerializeField]
    NetworkObject foodPrefab;
    [SerializeField]
    NetworkObject healthPackPrefab;

    InputHandler inputHandler;

    private bool isFoodSpawned = false;
    private bool isHealthPacksSpawned = false;

    void SpawnNetworkObject(NetworkObject prefab, int amount, bool isSpawned)
    {
        for (int i = 0; i < amount; i++)
        {
            NetworkObject spawnedGameObject = Runner.Spawn(prefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
            spawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
        }

        isSpawned = true;
    }

    //void SpawnFood()
    //{
    //    for (int i = 0; i < 300; i++)
    //    {
    //        NetworkObject spawnedGameObject = Runner.Spawn(foodPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
    //        spawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
    //    }

    //    isFoodSpawned = true;
    //}

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerJoined");

        if (runner.IsServer)
        {
            NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity, player);
            spawnedNetworkPlayer.NetPlayerState = NetworkPlayer.PlayerState.connected;

            if (!isFoodSpawned)
                SpawnNetworkObject(foodPrefab, 200, isFoodSpawned);
            if (!isHealthPacksSpawned)
                SpawnNetworkObject(healthPackPrefab, 15, isHealthPacksSpawned);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputHandler == null && NetworkPlayer.Local != null)
        {
            inputHandler = NetworkPlayer.Local.GetComponent<InputHandler>();
        }
        if (inputHandler != null)
        {
            input.Set(inputHandler.GetNetworkInput());
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerLeft");
    }

    public void OnConnectedToServer(NetworkRunner runner) { Utils.DebugLog("OnConnectedToServer"); }
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