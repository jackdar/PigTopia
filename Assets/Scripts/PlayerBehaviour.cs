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

    public bool IsAlive => !wasHit;

    public override void Spawned()
    {
        _networkTransform = GetComponent<NetworkTransform>();
        _networkTransform.InterpolationTarget.localScale = Vector3.one;
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
        if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
        {
            playerNetworkObject.GetComponent<PlayerNetworkedData>().AddToScore(points);
        }

        wasHit = true;
        despawnTimer = TickTimer.CreateFromSeconds(Runner, .2f);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && wasHit && despawnTimer.Expired(Runner))
        {
            wasHit = false;
            despawnTimer = TickTimer.None;
            
            Runner.Despawn(Object);
        }
    }

    // public override void Render()
    // {
    //     if (wasHit && despawnTimer.IsRunning)
    //     {
    //         _networkTransform.InterpolationTarget.localScale *= .96f;
    //     }
    // }
}
