using UnityEngine;
using DG.Tweening;

public class EnergyCell : MonoBehaviour
{
    public GameObject missionCompletePanel;

    public float rotateSpeed = 2f;
    public float floatDistance = 0.5f;
    public float floatDuration = 1.5f;

    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), rotateSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(true);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        transform.DOScale(0, 0.2f).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}