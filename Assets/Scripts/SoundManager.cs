using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioClip AutoShot;
    public AudioClip P1911Shot;

    public AudioSource reloadingSound1911;
    public AudioSource reloadingSoundAuto;

    public AudioSource emptyMagazineSound1911;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.AutoWeapon:
                ShootingChannel.PlayOneShot(AutoShot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                reloadingSound1911.Play();
                break;
            case WeaponModel.AutoWeapon:
                reloadingSoundAuto.Play();
                break;
        }
    }
}
