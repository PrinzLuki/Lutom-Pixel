using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool isLocalhealth;
    public Image healthFill;

    public PlayerStats stats;

    private void Start()
    {
        SetPlayerStatsTohealth();
        stats.onHealthChanged.AddListener(SetHealthFill);
    }

    public void SetPlayerStatsTohealth()
    {
        if (isLocalhealth)
            stats = GetComponentInParent<Transform>().GetComponentInParent<PlayerStats>();
    }

    public void SetHealthFill(float health, float maxHealth)
    {
        healthFill.fillAmount = health / maxHealth;
    }
}
