using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ShootController : NetworkBehaviour
{
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private NetworkPrefabRef bulletPrefab = NetworkPrefabRef.Empty;
    
    // Local Runtime references
    private Rigidbody2D _rigidbody2D = null;
    [SerializeField] private Transform gunRotation;
    
    // Game Session SPECIFIC Settings
    [Networked] private TickTimer _shootCooldown { get; set; }

    public override void Spawned()
    {
        //Set local runtime refences
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        // if (GetInput(out NetworkInputData networkInputData))
        // {
        //     if (networkInputData.isFireButtonPressed)
        //     {
        //         Fire();
        //     }
        // }

        
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
  
            SpawnBullet();
        
    }

    private void SpawnBullet()
    {
        if (_shootCooldown.ExpiredOrNotRunning(Runner) == false) return;

        var position = _rigidbody2D.position;
        Runner.Spawn(bulletPrefab, new Vector3(position.x, position.y, 0f), gunRotation.rotation, Object.InputAuthority);

        
        _shootCooldown = TickTimer.CreateFromSeconds(Runner, cooldown);
        
    }
}
