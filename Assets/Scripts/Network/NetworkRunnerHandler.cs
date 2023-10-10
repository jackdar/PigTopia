using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    [SerializeField]
    NetworkRunner networkRunnerPrefab;

    NetworkRunner runner;

    private void Awake()
    {
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        // If already have a network runner in the scene then we should not create another one but rather use the existing one
        if (networkRunnerInScene != null)
        {
            runner = networkRunnerInScene;
        }
    }

    void Start()
    {
        if (runner == null)
        {
            runner = Instantiate(networkRunnerPrefab);
            runner.name = $"Network Runner";

            GameMode gameMode = GameMode.Client;

#if UNITY_EDITOR
            gameMode = GameMode.AutoHostOrClient;
            Utils.DebugLog($"NetworkRunner Host mode");
#elif UNITY_SERVER
            gameMode = GameMode.Server;
            Utils.DebugLog("NetworkRunner Server mode");
#endif

            InitializeNetworkRunner(runner, gameMode, "Test Session", NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

            Utils.DebugLog($"Server NetworkRunner started.");
        }    
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        Utils.DebugLog($"InitializeNetworkRunner done");

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }

    INetworkSceneManager GetSceneManager(NetworkRunner runner)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            // Handle networked objects that already exist in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        Utils.DebugLog("Scene Manager has been instantiated");

        return sceneManager;
    }
}
