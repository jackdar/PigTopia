using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootController : NetworkBehaviour
{
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private NetworkPrefabRef bulletPrefab = NetworkPrefabRef.Empty;
    private Vector3 aimDirection;
    
    // Local Runtime references
    private Rigidbody2D _rigidbody2D = null;
    [SerializeField] private Transform gunShotPoint;

    // Game Session SPECIFIC Settings
    [Networked] private NetworkButtons _buttonsPrevious { get; set; }
    [Networked] private TickTimer ShootCooldown { get; set; }

    public override void Spawned()
    {
        //Set local runtime references
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            aimDirection = networkInputData.aimDirection;
            gunShotPoint.transform.eulerAngles = aimDirection;
            if (networkInputData.isFireButtonPressed)
            {
                Fire(networkInputData);
            }
        }
        // if (GetInput<NetworkInputData>(out var input) == false) return;
        
        
            // Fire(input);
        
    }

    private void Fire(NetworkInputData input)
    {
        // if (input.Buttons.WasPressed(_buttonsPrevious, PlayerButtons.Fire))
        // {
            SpawnBullet();
        // }

        // _buttonsPrevious = input.Buttons;
    }

    private void SpawnBullet()
    {
        if (ShootCooldown.ExpiredOrNotRunning(Runner) == false) return;

        var position = gunShotPoint.position;
        Runner.Spawn(bulletPrefab, new Vector3(position.x, position.y, 0f), gunShotPoint.rotation, Object.InputAuthority);

        
        ShootCooldown = TickTimer.CreateFromSeconds(Runner, cooldown);
        
    }
}
