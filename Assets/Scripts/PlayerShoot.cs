using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera _aimCamera;

    [SerializeField]
    private LayerMask _maskColliders;

    private WeaponManager _weaponManager;
    private PlayerWeapon _currentWeapon;

    private void Start()
    {
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _currentWeapon = _weaponManager.GetCurrentWeapon();

        if (_currentWeapon.CanBeReloaded())
        {
            if (Input.GetButtonDown("Reload"))
            {
                _weaponManager.Reload();
                return;
            }
        }

        if (_currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / _currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Shoot!");

        if (!isLocalPlayer) return;

        if (_currentWeapon.NeedReload())
        {
            _weaponManager.Reload();

            return;
        }

        _currentWeapon.DecrementBullets();

        Debug.Log("Remaining bullets: " + _currentWeapon.Bullets);

        RaycastHit _hit;
        if (Physics.Raycast(_aimCamera.transform.position, _aimCamera.transform.forward, out _hit, _aimCamera.farClipPlane, _maskColliders))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                CmdPlayerShoot(_hit.collider.name, _currentWeapon.damage, transform.name);
            }
        }

        if (_currentWeapon.NeedReload())
        {
            _weaponManager.Reload();
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