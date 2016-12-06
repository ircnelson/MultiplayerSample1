using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    public Player player;

    private void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(20, 200, 100, 20), string.Format("Health: {0}", player.CurrentHealth.ToString()), new GUIStyle { fontStyle = FontStyle.Bold });

    }
}
