using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera _aimCamera;

    [SerializeField]
    private LayerMask _maskColliders;

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Shoot!");

        if (!isLocalPlayer) return;
        
        RaycastHit _hit;
        if (Physics.Raycast(_aimCamera.transform.position, _aimCamera.transform.forward, out _hit, _aimCamera.farClipPlane, _maskColliders))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                CmdPlayerShoot(_hit.collider.name, 20f, transform.name);
            }
        }
    }

    [Command]
    private void CmdPlayerShoot(string targetPlayerId, float damage, string sourcePlayerId)
    {
        Debug.Log(string.Format("{0} hit {1} and cause {2} damage", sourcePlayerId, targetPlayerId, damage));

        var targetPlayer = GameManager.GetPlayer(targetPlayerId);
        targetPlayer.RpcTakeDamage(damage, sourcePlayerId);
    }
}