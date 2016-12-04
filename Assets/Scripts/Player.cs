using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string nickname;

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
    }
}
