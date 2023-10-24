using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GrenadeHandler : NetworkBehaviour
{
    public GameObject explosionParticleSystemPrefab;
    
    // Player info
    private PlayerRef thrownByPlayerRef;
    private string thrownByPlayerName;
    
    // Timing
    private TickTimer explodeTickTimer = TickTimer.None;
    
    // Other components
    private NetworkObject _networkObject;
    private NetworkRigidbody2D _networkRigidbody2D;
    
    public void Throw(Vector2 throwForce, PlayerRef thrownPlayerRef, string thrownPlayerName)
    {
        _networkObject = GetComponent<NetworkObject>();
        _networkRigidbody2D = GetComponent<NetworkRigidbody2D>();
        
        _networkRigidbody2D.Rigidbody.AddForce(throwForce, ForceMode2D.Impulse);

        this.thrownByPlayerRef = thrownPlayerRef;
        this.thrownByPlayerName = thrownPlayerName;

        explodeTickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (explodeTickTimer.Expired(Runner))
            {
                Runner.Despawn(_networkObject);
                
                // Stop the explode timer from being triggered again
                explodeTickTimer = TickTimer.None;
            }
        }
    }
    
    // When despawning the object we want to create a visual explosion
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        SpriteRenderer grenadeMesh = GetComponentInChildren<SpriteRenderer>();

        Instantiate(explosionParticleSystemPrefab, grenadeMesh.transform.position, Quaternion.identity);
    }
}
