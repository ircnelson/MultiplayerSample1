using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private Camera _aimCamera;

	[SerializeField]
	private string _weaponLayerName = "Weapon";

	[SerializeField]
	private Transform _weaponHolder;

	[SerializeField]
	private PlayerWeapon _primaryWeapon;

    [SerializeField]
    private PlayerWeapon _secondaryWeapon;
    
    private int _currentWeaponIndex = 0;
    private PlayerWeapon _currentWeapon;

	public bool isReloading = false;

	void Start ()
	{
		EquipWeapon(_primaryWeapon);
	}

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CmdSwitchWeapon();
        }
    }

    [Command]
    private void CmdSwitchWeapon()
    {
        RpcSwitchWeapon();
    }

    [ClientRpc]
    private void RpcSwitchWeapon()
    {
        Debug.Log("Player: " + transform.name);
        Debug.Log("Switch weapon");

        UnEquipWeapon();

        if (_currentWeaponIndex > 1) _currentWeaponIndex = 0;

        var nextWeapon = (_currentWeaponIndex == 0) ? _primaryWeapon : _secondaryWeapon;

        EquipWeapon(nextWeapon);

        _currentWeaponIndex++;
    }

    public PlayerWeapon GetCurrentWeapon ()
	{
		return _currentWeapon;
	}

	void EquipWeapon (PlayerWeapon weapon)
	{
		_currentWeapon = weapon;

		GameObject weaponIns = (GameObject)Instantiate(weapon.graphics, _weaponHolder.position, _weaponHolder.rotation, _weaponHolder);
        
        _aimCamera.farClipPlane = weapon.range;

        if (isLocalPlayer)
			Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(_weaponLayerName));

	}

    void UnEquipWeapon()
    {
        if (_weaponHolder.childCount > 0)
        {
            Destroy(_weaponHolder.GetChild(0).gameObject);
        }
    }

	public void Reload ()
	{
		if (isReloading)
			return;

		StartCoroutine(Reload_Coroutine());
	}

	private IEnumerator Reload_Coroutine ()
	{
		Debug.Log("Reloading...");

		isReloading = true;

		CmdOnReload();

		yield return new WaitForSeconds(_currentWeapon.reloadTime);

		_currentWeapon.Reload();

		isReloading = false;
	}

	[Command]
	void CmdOnReload ()
	{
		RpcOnReload();
	}

	[ClientRpc]
	void RpcOnReload ()
	{
		//Animator anim = currentGraphics.GetComponent<Animator>();
		//if (anim != null)
		//{
		//	anim.SetTrigger("Reload");
		//}
	}

}
