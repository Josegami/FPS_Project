using UnityEngine;
using TMPro;

public class MissionEndTrigger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject missionCompletePanel;

    private bool missionCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (missionCompleted) return;

        if (other.CompareTag("Player"))
        {
            missionCompleted = true;
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        Time.timeScale = 0f;

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(true);

        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.missionCompletedMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
