using UnityEngine;
using System.Collections;
using Fusion;
using System.Collections.Generic;

public class NetworkPlayerListHandler : NetworkBehaviour
{
    [Header("Canvas")]
    [SerializeField]
    Canvas canvas;

    InGameUIHandler inGameUIHandler;

    [Networked(OnChanged = nameof(OnNetworkPlayerListChanged))]
    [Capacity(50)]
    public NetworkLinkedList<NetworkPlayer> Players { get; }

    void Start()
    {
        inGameUIHandler = canvas.GetComponent<InGameUIHandler>();
    }

    public override void FixedUpdateNetwork()
    {
        if (inGameUIHandler.playerListHandler != null)
        {
            if (inGameUIHandler.playerListHandler.Players.Count > 0)
                inGameUIHandler.HandleScoreboard();
        }
    }

    static void OnNetworkPlayerListChanged(Changed<NetworkPlayerListHandler> changed)
    {
        changed.Behaviour.OnNetworkPlayerListChanged();
    }

    void OnNetworkPlayerListChanged()
    {
        inGameUIHandler.HandleScoreboard();
    }

}

