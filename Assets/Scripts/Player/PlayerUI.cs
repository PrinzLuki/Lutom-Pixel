using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : NetworkBehaviour
{
    [Header("Health UI Large")]
    public GameObject healthObjLarge;
    public Slider healthFillLarge;
    public TextMeshProUGUI healthValue;
    public Image deadImage;
    [Header("Health UI Small")]
    public GameObject healthObjSmall;
    public Slider healthFillSmall;

    public PlayerStats stats;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        stats.onHealthChanged.AddListener(CmdSetHealthFill);
        if (!hasAuthority)
        {
            healthObjSmall.SetActive(true);
            healthObjLarge.SetActive(false);
        }

    }

    [Command]
    public void CmdSetHealthFill(float health, float maxHealth)
    {
        RpcSetHealthFill(health, maxHealth);
    }

    [ClientRpc]
    public void RpcSetHealthFill(float health, float maxHealth)
    {
        healthFillLarge.value = health / maxHealth;
        healthFillSmall.value = health / maxHealth;
        healthValue.text = health.ToString();
    }
}
