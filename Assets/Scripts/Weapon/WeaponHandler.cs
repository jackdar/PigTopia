using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Fusion;
using Weapon;

public class WeaponHandler : NetworkBehaviour
{
    [Header("Prefabs")] public BulletHandler bulletPrefab;
    
    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    public ParticleSystem fireParticleSystem;
    
    private float lastTimeFired = 0;
    
    //Timing
    TickTimer bulletFireDelay = TickTimer.None;
// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFireButtonPressed)
            {
                Fire(networkInputData.aimForwardVector);
            }
        }
    }

    void Fire(Vector2 aimForwardVector)
    {
        if (Time.time - lastTimeFired < 0.15f)
        {
            return;
        }

        StartCoroutine(FireEffectCO());
        Runner.Spawn(bulletPrefab, aimForwardVector, Quaternion.LookRotation(aimForwardVector), Object.InputAuthority, (runner, spawnedBullet) =>
        {
            spawnedBullet.GetComponent<BulletHandler>().Throw(aimForwardVector * 15, Object.InputAuthority, "Test");
        });
        lastTimeFired = Time.time;
    }

    IEnumerator FireEffectCO()
    {
        isFiring = true;
        fireParticleSystem.Play();
        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }
    
    static void OnFireChanged(Changed<WeaponHandler> changed)
    {
        Debug.Log($"{Time.time} OnFireChanged value (changed.Behaviour.isFiring)");

        bool isFiringCurrent = changed.Behaviour.isFiring;
        
        // Load old value
        changed.LoadOld();
        bool isFiringOld = changed.Behaviour.isFiring;
        
        // Check if firing value actually changed
        if (isFiringCurrent && !isFiringOld)
        {
            changed.Behaviour.OnFireRemote();
        }
    }

    void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
        {
            fireParticleSystem.Play();
        }
    }
}
