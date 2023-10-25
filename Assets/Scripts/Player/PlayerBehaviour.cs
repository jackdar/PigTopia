using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Information about player hit stuff lol
public class PlayerBehaviour : NetworkBehaviour
{
    [SerializeField] private int points = 1;

    [HideInInspector] [Networked] public NetworkBool IsBig { get; set; }

    [Networked] private NetworkBool wasHit { get; set; }

    [Networked] private TickTimer despawnTimer { get; set; }

    private NetworkTransform _networkTransform;
    private NetworkPlayer networkPlayer;
    public bool IsAlive => !wasHit;

    public override void Spawned()
    {
        _networkTransform = GetComponent<NetworkTransform>();
        _networkTransform.InterpolationTarget.localScale = Vector3.one;
        networkPlayer = GetComponent<NetworkPlayer>();
    }
    
    // When player is hit by another object, method is called to decide next action taken
    public void HitPlayer(PlayerRef player)
    {
        // The player hit only triggers behaviour on the host and if the player had not yet been hit
        if (Object == null) return;
        if (Object.HasStateAuthority == false) return;
        if (wasHit) return;

        // If hit was triggered by a projectile, player who shot gets points
        // Player object is retrieved via the Runner
        if (Runner.TryGetPlayerObject(player, out var playerNetworkObject) && networkPlayer.NetHealth <= 0f)
        {
            playerNetworkObject.GetComponent<PlayerNetworkedData>().AddToScore(points);
        }

        wasHit = true;
        Debug.Log("player was hit");
        despawnTimer = TickTimer.CreateFromSeconds(Runner, .2f);
    }

    public override void FixedUpdateNetwork()
    {
        if (wasHit && networkPlayer.NetHealth > 0f)
        {
            networkPlayer.decreaseHealth(2f);
            StartCoroutine(HandleHitColor());
            wasHit = false;
        }
        
        if (Object.HasStateAuthority && despawnTimer.Expired(Runner) && networkPlayer.NetHealth <= 0f)
        {
            wasHit = false;
            despawnTimer = TickTimer.None;
            
            // Runner.Despawn(Object);
            Object.GetComponent<NetworkPlayer>().ResetPlayer();
            Object.GetComponent<InGameUIHandler>().HandleScoreboard();
        }
    }

    IEnumerator HandleHitColor()
    {
        Object.GetComponent<NetworkPlayer>().NetSpriteColor = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.1f);
        Object.GetComponent<NetworkPlayer>().NetSpriteColor = new Color(1f, 1f, 1f, 1f);
    }

    // public override void Render()
    // {
    //     if (wasHit && despawnTimer.IsRunning)
    //     {
    //         _networkTransform.InterpolationTarget.localScale *= .96f;
    //     }
    // }
}
