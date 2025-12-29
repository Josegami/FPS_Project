using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public AmmoType ammoTytpe;

    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo
    }
}
