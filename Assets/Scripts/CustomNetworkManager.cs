using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        var playerController = conn.playerControllers[0];

        if (playerController != null)
        {
            var player = playerController.gameObject.GetComponent<Player>();

            GameManager.RegisterPlayer(playerController.unetView.netId.ToString(), player);
        }
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);

        GameManager.UnRegisterPlayer(player.unetView.netId.ToString());
    }
}
