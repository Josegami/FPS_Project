using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Position Recoil")]
    public Vector3 recoilKick = new Vector3(-0.06f, 0.03f, 0f);
    public float kickSpeed = 25f;
    public float returnSpeed = 18f;

    [Header("Rotation Recoil")]
    public Vector3 rotationKick = new Vector3(-2f, 1f, 0f);

    private Vector3 currentPos;
    private Vector3 targetPos;

    private Vector3 currentRot;
    private Vector3 targetRot;

    void Update()
    {
        targetPos = Vector3.Lerp(targetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);

        currentPos = Vector3.Lerp(currentPos, targetPos, kickSpeed * Time.deltaTime);
        currentRot = Vector3.Lerp(currentRot, targetRot, kickSpeed * Time.deltaTime);

        transform.localPosition = currentPos;
        transform.localRotation = Quaternion.Euler(currentRot);
    }

    public void ApplyRecoil()
    {
        targetPos += recoilKick;
        targetRot += new Vector3(
            rotationKick.x,
            Random.Range(-rotationKick.y, rotationKick.y),
            Random.Range(-rotationKick.z, rotationKick.z)
        );
    }
}
