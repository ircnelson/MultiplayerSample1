using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField]
    private Text _healthText;

    [SerializeField]
    private Text _ammoText;

    public Player player;

    private void Update()
    {
        _healthText.text = player.CurrentHealth.ToString();
    }
}
