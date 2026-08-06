using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;
    private float lastHealthDisplayed = float.NaN;
    private bool isDead;

    public float CurrentHealth => health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (!isDead && health <= 0f)
        {
            Die();
        }

        float hFraction = health / maxHealth;
        bool barsAreLerping =
            backHealthBar.fillAmount > hFraction ||
            frontHealthBar.fillAmount < hFraction;

        if (barsAreLerping || health != lastHealthDisplayed)
        {
            UpdateHealthUI();
        }
    }
    public void UpdateHealthUI()
    {
        if (health != lastHealthDisplayed)
        {
            lastHealthDisplayed = health;
            healthText.SetText("{0}", health);
        }
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentCompleted = lerpTimer / chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentCompleted);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentCompleted = lerpTimer / chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentCompleted);
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        CameraShake.Instance.Shake(0.15f, 0.1f);
        float percent = health / maxHealth;

        float alpha = Mathf.Lerp(0.8f, 0.2f, percent);

        DamageOverlay.Instance.ShowDamage(alpha);
        lerpTimer = 0f;
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    /// <summary>
    /// Đặt thẳng lượng máu hiện tại, dùng khi khôi phục save.
    /// </summary>
    public void SetHealth(float value)
    {
        health = Mathf.Clamp(value, 0, maxHealth);
        isDead = health <= 0f;
        lerpTimer = 0f;

        frontHealthBar.fillAmount = health / maxHealth;
        backHealthBar.fillAmount = health / maxHealth;
        lastHealthDisplayed = float.NaN;
    }

    /// <summary>
    /// Báo cho GameManager nạp lại save gần nhất khi player hết máu.
    /// </summary>
    private void Die()
    {
        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDied();
        }
    }
}
