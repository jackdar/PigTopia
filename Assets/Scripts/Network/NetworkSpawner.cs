using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
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

<<<<<<< Updated upstream
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

=======
    bool isFoodSpawned = false;

    //Other components
    InputHandler inputHandler;

    void SpawnFood()
    {
        for (int i = 0; i < 300; i++)
        {
            NetworkObject spawnedGameObject = Runner.Spawn(foodPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
            spawnedGameObject.transform.position = Utils.GetRandomSpawnPosition();
        }

        isFoodSpawned = true;
    }

>>>>>>> Stashed changes
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Utils.DebugLog("OnPlayerJoined");

        if (runner.IsServer)
        {
            NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity, player);
<<<<<<< Updated upstream
            if (!isSpawned)
            {
                SpawnFood();
            }
=======
            spawnedNetworkPlayer.playerState = NetworkPlayer.PlayerState.connected;

            if (!isFoodSpawned)
                SpawnFood();
>>>>>>> Stashed changes
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
<<<<<<< Updated upstream

=======
        if (inputHandler == null && NetworkPlayer.Local != null)
            inputHandler = NetworkPlayer.Local.GetComponent<InputHandler>();

        if (inputHandler != null)
            input.Set(inputHandler.GetNetworkInput());
>>>>>>> Stashed changes
    }

    public void OnConnectedToServer(NetworkRunner runner) { Utils.DebugLog("OnConnectedToServer"); }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { Utils.DebugLog("OnPlayerLeft"); }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Utils.DebugLog("OnShutdown"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Utils.DebugLog("OnDisconnectedFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Utils.DebugLog("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Utils.DebugLog("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
<<<<<<< Updated upstream

}
=======
}
>>>>>>> Stashed changes
