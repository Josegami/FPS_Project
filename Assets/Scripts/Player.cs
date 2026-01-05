using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bloodyScreen;

    [Header("Health")]
    public int HP = 100;
    public Image healthFill;
    public int maxHP = 100;

    public Animator animator;

    public GameObject gameOverUI;

    public bool isDead;

    [Header("Settings")]
    [SerializeField] private float immunityTime = 1.0f;
    private bool isInvincible = false; 

    private void Start()
    {
        Application.targetFrameRate = 60; // Limita el juego a 60fps para ahorrar recursos

        maxHP = HP;
    }

    private void Update()
    {
        HandleHealthBarSmooth();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead || isInvincible) return;

        HP -= damageAmount;

        StartCoroutine(BecomeInvincible());

        if (HP <= 0)
        {
            print("Player dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player hit");
            StartCoroutine(BloodyScreenEffect());
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);

        }
    }
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(immunityTime);
        isInvincible = false;
    }
    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);

        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        GetComponent<PlayerMovement>().enabled = false;

        animator.SetTrigger("DEAD");

        GetComponent<ScreenBlacout>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterHand"))
        {
            if (!isDead && !isInvincible)
            {
                MonsterHand hand = other.gameObject.GetComponent<MonsterHand>();
                if (hand != null)
                {
                    TakeDamage(hand.damage);
                }
            }
        }
    }

    // Hace que la barra baje suavemente
    private void HandleHealthBarSmooth()
    {
        if (healthFill == null) return;

        float targetFill = (float)HP / maxHP;
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, targetFill, 5f * Time.deltaTime);
    }
}
