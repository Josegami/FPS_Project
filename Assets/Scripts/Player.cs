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

    [Header("Berserk Mode")]
    public float berserkCharge = 0f;
    public float maxBerserkCharge = 100f;
    public float chargePerKill = 10f;
    public Image berserkFill; 
    public float berserkDuration = 7f;
    public float damageMultiplier = 2f;
    public bool isBerserkActive = false;

    [Header("Berserk Visual Effects")]
    public Image berserkOverlayImage;
    [Range(0f, 1f)] public float overlayMaxAlpha = 0.4f;
    public float overlayFadeSpeed = 5f;

    public Animator animator;
    public GameObject gameOverUI;
    public bool isDead;

    [Header("Settings")]
    [SerializeField] private float immunityTime = 1.0f;
    private bool isInvincible = false;

    private void Start()
    {
        Application.targetFrameRate = 60;
        maxHP = HP;
        if (berserkFill != null) berserkFill.fillAmount = 0;

        if (berserkOverlayImage != null)
        {
            Color c = berserkOverlayImage.color;
            c.a = 0f;
            berserkOverlayImage.color = c;
        }
    }

    private void Update()
    {
        HandleHealthBarSmooth();
        HandleBerserkBarSmooth(); 

        if (Input.GetKeyDown(KeyCode.Q) && berserkCharge >= maxBerserkCharge && !isBerserkActive && !isDead)
        {
            StartCoroutine(ActivateBerserk());
        }
    }

    public void AddBerserkCharge()
    {
        if (isBerserkActive || isDead) return;

        berserkCharge += chargePerKill;
        berserkCharge = Mathf.Clamp(berserkCharge, 0, maxBerserkCharge);
    }

    private IEnumerator ActivateBerserk()
    {
        isBerserkActive = true;
        berserkCharge = 0;

        if (berserkOverlayImage != null)
        {
            float targetAlpha = overlayMaxAlpha;
            Color c = berserkOverlayImage.color;
            while (c.a < targetAlpha - 0.05f)
            {
                c.a = Mathf.Lerp(c.a, targetAlpha, overlayFadeSpeed * Time.deltaTime);
                berserkOverlayImage.color = c;
                yield return null;
            }
            c.a = targetAlpha;
            berserkOverlayImage.color = c;
        }

        yield return new WaitForSeconds(berserkDuration - 0.5f);

        if (berserkOverlayImage != null)
        {
            float targetAlpha = 0f;
            Color c = berserkOverlayImage.color;
            while (c.a > targetAlpha + 0.05f)
            {
                c.a = Mathf.Lerp(c.a, targetAlpha, overlayFadeSpeed * Time.deltaTime);
                berserkOverlayImage.color = c;
                yield return null;
            }
            c.a = targetAlpha;
            berserkOverlayImage.color = c;
        }

        isBerserkActive = false;
    }

    private void HandleBerserkBarSmooth()
    {
        if (berserkFill == null) return;
        float targetFill = berserkCharge / maxBerserkCharge;
        berserkFill.fillAmount = Mathf.Lerp(berserkFill.fillAmount, targetFill, 10f * Time.deltaTime);

        berserkFill.color = (berserkCharge >= maxBerserkCharge) ? Color.white : Color.red;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead || isInvincible) return;

        HP -= damageAmount;
        StartCoroutine(BecomeInvincible());

        if (HP <= 0)
        {
            PlayerDead();
            isDead = true;
        }
        else
        {
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
        HUDManager.Instance.HideHUD();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        if (bloodyScreen.activeInHierarchy == false) bloodyScreen.SetActive(true);
        var image = bloodyScreen.GetComponentInChildren<Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (bloodyScreen.activeInHierarchy) bloodyScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterHand"))
        {
            if (!isDead && !isInvincible)
            {
                MonsterHand hand = other.gameObject.GetComponent<MonsterHand>();
                if (hand != null) TakeDamage(hand.damage);
            }
        }
    }

    private void HandleHealthBarSmooth()
    {
        if (healthFill == null) return;
        float targetFill = (float)HP / maxHP;
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, targetFill, 5f * Time.deltaTime);
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        HP += amount;
        if (HP > maxHP) HP = maxHP;
    }
}