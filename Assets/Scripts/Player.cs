using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [Header("Setups")]
    [SerializeField]
    private PlayerGUI _playerGUIPrefab;

    private PlayerGUI _playerGUI;

    public PlayerGUI HUD
    {
        get
        {
            return _playerGUI;
        }
    }

    [SerializeField]
    private Behaviour[] _disableComponents;

    [Header("Player")]
    [SyncVar]
    public string nickname;

    [SerializeField]
    private Transform _holder;

    [SerializeField]
    private GameObject[] _weapons;

    [SyncVar]
    private int _currentWeaponIndex = 0;

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");

        base.OnStartClient();

        var netId = GetComponent<NetworkIdentity>().netId.ToString();
        var player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }
    
    private void Start()
    {
        Debug.Log("Start");

        if (!isLocalPlayer)
        {
            if (_disableComponents != null && _disableComponents.Length > 0)
            {
                for (int i = 0; i < _disableComponents.Length; i++)
                {
                    _disableComponents[i].enabled = false;
                }
            }
        }
        else
        {
            //_playerGUI = Instantiate(_playerGUIPrefab);
            //_playerGUI.player = this;
        }
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");

        GetComponent<MeshRenderer>().material.color = Color.green;

        CmdPlayerJoined(transform.name, User.instance.Nickname);
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
        Debug.Log("Player: " + nickname);
        Debug.Log("Switch weapon");
        Debug.Log("Weapon index: " + _currentWeaponIndex);

        if (_holder.childCount > 0)
        {
            Destroy(_holder.GetChild(0).gameObject);
        }

        var currentWeapon = (GameObject)Instantiate(_weapons[_currentWeaponIndex], _holder.position, _holder.transform.rotation, _holder.transform);

        _currentWeaponIndex++;

        if (_currentWeaponIndex >= _weapons.Length) _currentWeaponIndex = 0;
    }

    [Command]
    private void CmdPlayerJoined(string playerId, string nickname)
    {
        var player = GameManager.GetPlayer(playerId);
        
        if (player != null)
        {
            player.nickname = nickname;

            Debug.Log(nickname + " has joined.");
        } 
    }

    private void OnDisable()
    {
        GameManager.UnRegisterPlayer(transform.name);

        Destroy(_playerGUI);
    }
}
