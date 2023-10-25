using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    [Header("Food")]
    [SerializeField]
    NetworkObject foodPrefab;

    [Header("Player")]
    [SerializeField]
    NetworkPlayer playerPrefab;

    InputHandler inputHandler;

    private bool isFoodSpawned = false;
<<<<<<< Updated upstream
    
    void SpawnFood()
=======
    private bool isHealthPacksSpawned = false;

    private List<NetworkObject> foodObjects = new();
    private List<NetworkObject> healthPackObjects = new();

    void SpawnFood(int amount)
>>>>>>> Stashed changes
    {
        for (int i = 0; i < 300; i++)
        {
            NetworkObject spawnedGameObject = Runner.Spawn(foodPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
            spawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
            foodObjects.Add(spawnedGameObject);
        }

        isFoodSpawned = true;
<<<<<<< Updated upstream
=======
    }

    void SpawnHealthPacks(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            NetworkObject spawnedGameObject = Runner.Spawn(healthPackPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
            spawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
            healthPackObjects.Add(spawnedGameObject);
        }

        isHealthPacksSpawned = true;
>>>>>>> Stashed changes
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerJoined");

        if (runner.IsServer)
        {
            NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity, player);
            spawnedNetworkPlayer.NetPlayerState = NetworkPlayer.PlayerState.connected;

            if (!isFoodSpawned)
<<<<<<< Updated upstream
                SpawnFood();
=======
                SpawnFood(200);
            else
                SpawnFood(25);

            if (!isHealthPacksSpawned)
                SpawnHealthPacks(15);
            else
                SpawnHealthPacks(2);
>>>>>>> Stashed changes
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

        // Remove extra food
        for (int i = foodObjects.Count; i > foodObjects.Count - 25; i--)
        {
            runner.Despawn(foodObjects[i]);
            foodObjects.RemoveAt(i);
        }

        // Remove extra health packs
        for (int i = healthPackObjects.Count; i > healthPackObjects.Count - 25; i--)
        {
            runner.Despawn(healthPackObjects[i]);
            healthPackObjects.RemoveAt(i);
        }
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
