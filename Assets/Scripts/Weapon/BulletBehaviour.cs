using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    // Bullet Settings
    [SerializeField] private float maxLifetime = 1.0f;
    [SerializeField] private float speed = 15f;
    [SerializeField] private LayerMask playerLayer;
    
    [Networked] private TickTimer _currentLifetime { get; set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority == false)
        {
            return;
        }

        _currentLifetime = TickTimer.CreateFromSeconds(Runner, maxLifetime);
    }

    public override void FixedUpdateNetwork()
    {
        // If bullet has not hit continue
        if (HasHit() == false)
        {
            transform.Translate(transform.up * speed * Runner.DeltaTime, Space.World);
           
        }
        else
        {
            Runner.Despawn(Object);
            return;
        }

        CheckLifetime();
    }
    
    // Check bullet lifetime
    private void CheckLifetime()
    {
        if (_currentLifetime.Expired(Runner) == false)
        {
            return;
        }
        Runner.Despawn(Object);
    }
    
    // Check if bullet will hit in next tick
    private bool HasHit()
    {
        RaycastHit2D hitCollider = Runner.GetPhysicsScene2D().Raycast(transform.position, transform.up, speed * Runner.DeltaTime, playerLayer);
        PolygonCollider2D tempTree = new PolygonCollider2D();
        
        Debug.Log("Projectile Owner: " + Object.InputAuthority);
        
        // Detects collision with objects
        if (hitCollider.collider != null)
        {
            LayerMask hitLayer = hitCollider.collider.gameObject.layer;
            if (LayerMask.LayerToName(hitLayer) == "Obstacle")
            {
                if (hitCollider.collider.GetType() != tempTree.GetType())
                {
                    return true; 
                }
            }

            // Checks for player hit
            if (LayerMask.LayerToName(hitLayer) == "Player")
            {
                GameObject playerHit = hitCollider.collider.gameObject;
                PlayerRef hitPlayerIA = playerHit.GetComponent<NetworkObject>().InputAuthority;
                PlayerRef currentPlayerIA = Object.InputAuthority;
                // Ensures player can't hurt self
                if(currentPlayerIA == hitPlayerIA)
                {
                    return false;
                }
                

                var playerBehaviour = playerHit.GetComponent<PlayerBehaviour>();

                if (playerBehaviour.IsAlive == false)
                {
                    return false;
                }
                playerBehaviour.HitPlayer(Object.InputAuthority);
                return true;
            }

        }
        
        // var hitPlayer = Runner.LagCompensation.Raycast(transform.position, transform.up, speed * Runner.DeltaTime,
        //     Object.InputAuthority, out var hit, playerLayer);
        // if (hitPlayer == false)
        // {
        //     return false;
        // }
        //
        // var playerBehaviour = hit.GameObject.GetComponent<PlayerBehaviour>();
        //
        // if (playerBehaviour.IsAlive == false)
        // {
        //     return false;
        // }
        //
        // playerBehaviour.HitPlayer(Object.InputAuthority);
        
        return false;

    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayShootSound(NetworkObject bullet, RpcInfo info = default)
    {
        Utils.DebugLog($"[RPC] RPC_PlayShootSound");

        bullet.GetComponent<AudioSource>().PlayOneShot(bullet.GetComponent<AudioSource>().clip);
    }
}
