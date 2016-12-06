using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name = "Glock";

    public int damage;
    public float range;
    public float fireRate;
    public int maxBullets;
    public float reloadTime;
    public GameObject graphics;

    [SerializeField]
    private int _bullets;

    public int Bullets
    {
        get
        {
            return _bullets;
        }
    }

    public PlayerWeapon()
    {
        _bullets = maxBullets;
    }

    public void Reload()
    {
        _bullets = maxBullets;
    }

    public bool NeedReload()
    {
        return _bullets <= 0;
    }

    public void DecrementBullets()
    {
        _bullets--;
    }

    public bool CanBeReloaded()
    {
        return _bullets < maxBullets;
    }
}