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
        if (HasHitPlayer() == false)
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
    private bool HasHitPlayer()
    {
        var hitPlayer = Runner.LagCompensation.Raycast(transform.position, transform.forward, speed * Runner.DeltaTime,
            Object.InputAuthority, out var hit, playerLayer);

        if (hitPlayer == false)
        {
            return false;
        }

        var bulletBehaviour = hit.GameObject.GetComponent<PlayerBehaviour>();

        if (bulletBehaviour.IsAlive == false)
        {
            return false;
        }

        bulletBehaviour.HitPlayer(Object.InputAuthority);

        return true;

    }
}
