using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField]
    private Text _healthText;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Image _bloodScreen;

    [SerializeField]
    [Range(0f, 1f)]
    private float _bloodScreenFading = 0f;
    
    public Player player;
    
    private void Update()
    {
        var currentWeapon = player.GetComponent<WeaponManager>().GetCurrentWeapon();

        _healthText.text = player.CurrentHealth.ToString();
        _ammoText.text = string.Format("{0}/{1}", currentWeapon.Bullets, currentWeapon.maxBullets);

        if (player.CurrentHealth <= 50)
        {
            _bloodScreenFading = (100 - player.CurrentHealth) / 100;
        }

        var newColor = _bloodScreen.color;
        newColor.a = _bloodScreenFading;

        _bloodScreen.color = Color.Lerp(_bloodScreen.color, newColor, 1f);
    }
}
