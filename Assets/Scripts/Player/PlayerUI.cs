using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : NetworkBehaviour
{
    [Header("References")]
    public PlayerStats stats;

    [Header("Health UI Large")]
    public GameObject healthObjLarge;
    public Slider healthFillLarge;
    public TextMeshProUGUI healthValue;
    public Image deadImage;
    [Header("Health UI Small")]
    public GameObject healthObjSmall;
    public Slider healthFillSmall;


    [Header("Pause UI")]
    public GameObject pauseWindow;
    public bool paused;

    private void Start()
    {
        GetStats();
    }

    private void Update()
    {
        if (!hasAuthority) return;
        if (InputManager.instance == null) { Debug.LogWarning("Input Instance missing!"); return; }

        if (InputManager.instance.Pause() && isLocalPlayer)
        {
            Pause();
        }
    }

    public void Pause()
    {
        paused = !paused;
        pauseWindow.SetActive(paused);
    }


    #region Health Stats
    public void GetStats()
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

    #endregion
}
