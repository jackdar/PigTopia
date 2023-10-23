using Fusion;
using UnityEngine;

namespace Weapon
{
    public class BulletHandler : NetworkBehaviour
    {
        public GameObject explosionParticleSystemPrefab;
    
        // Thrown by Info
        private PlayerRef thrownByPlayerRef;
        private string thrownByPlayerName;
    
        // Timing
        private TickTimer explodeTickTimer = TickTimer.None;
    
        private NetworkObject networkObject;
        private NetworkRigidbody2D networkRigidbody2D;

        public void Throw(Vector2 throwForce, PlayerRef thrownByPlayerRef, string thrownByPlayerName)
        {
            networkObject = GetComponent<NetworkObject>();
            networkRigidbody2D = GetComponent<NetworkRigidbody2D>();

            networkRigidbody2D.Rigidbody.AddForce(throwForce, ForceMode2D.Impulse);
        
            explodeTickTimer = TickTimer.CreateFromSeconds(Runner, 2);
        }

        public void FixedUpdate()
        {
            if (Object.HasStateAuthority)
            {
                if (explodeTickTimer.Expired(Runner))
                {
                    //Stop the explode timer from being triggered again
                    explodeTickTimer = TickTimer.None;
                }
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            MeshRenderer bulletMesh = GetComponentInChildren<MeshRenderer>();
    
            Instantiate(explosionParticleSystemPrefab, bulletMesh.transform.position, Quaternion.identity);
        }
    }
}
