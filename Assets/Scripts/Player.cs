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
            Util.SetLayerRecursively(gameObject, LayerMask.NameToLayer("RemotePlayer"));
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
}
