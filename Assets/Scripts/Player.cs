using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [Header("Setup")]
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
    [Tooltip("Usado para desabilitar os componentes que não devem ser usados no player remoto")]
    private Behaviour[] _disableComponents;

    [Header("Player")]
    [SyncVar]
    public string nickname;

    [SerializeField]
    private Transform _holder;

    public Transform Holder
    {
        get
        {
            return _holder;
        }
    }

    [SerializeField]
    private GameObject[] _weapons;

    [SyncVar]
    private int _currentWeaponIndex = 0;

    public float moveSpeed = 10f;

    [SyncVar(hook = "OnCurrentHealthChanged")]
    private float _currentHealth = 100f;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    
    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");

        base.OnStartClient();

        var netId = GetComponent<NetworkIdentity>().netId.ToString();
        var player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }

    [ClientRpc]
    public void RpcTakeDamage(float damage, string sourcePlayerId)
    {
        _currentHealth -= damage;
    }
    
    private void Start()
    {
        Debug.Log("Start");

        if (!isLocalPlayer)
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("RemotePlayer"));
        }
        else
        {
            _playerGUI = Instantiate(_playerGUIPrefab);
            _playerGUI.player = this;

            Camera.main.GetComponent<MoveCamera>().followTarget = transform;
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
        if (!isLocalPlayer && isServer) return;

        var player = GameManager.GetPlayer(playerId);
        
        if (player != null)
        {
            player.nickname = nickname;

            Debug.Log(nickname + " has joined.");
        } 
    }

    private void OnCurrentHealthChanged(float value)
    {
        // apply effect on HUD
    }

    private void OnDisable()
    {
        GameManager.UnRegisterPlayer(transform.name);

        Destroy(_playerGUI);
    }

    public static void SetLayerRecursively(GameObject go, int layer)
    {
        if (go == null)
            return;

        go.layer = layer;

        foreach (Transform _child in go.transform)
        {
            if (_child == null)
                continue;

            SetLayerRecursively(_child.gameObject, layer);
        }
    }
}
