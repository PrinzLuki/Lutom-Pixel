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

    public Image deadImage;
    [Header("Health UI Large")]
    public GameObject healthObjLarge;
    public Slider healthFillLarge;
    public TextMeshProUGUI healthValue;
    [Header("Health UI Small")]
    public GameObject healthObjSmall;
    public Slider healthFillSmall;

    [Header("Speed UI Large")]
    public GameObject specialObjLarge;
    public Slider specialFillLarge;
    public Image specialFill;
    public TextMeshProUGUI specialValue;
    [SerializeField] private Color speedColor = new Color(59, 210, 255);
    [SerializeField] private Color jumpPowerColor = new Color(255, 197, 59);


    [Header("Pause UI")]
    public GameObject pauseWindow;
    public bool paused;



    private void Start()
    {
        GetHealthStats();
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

    public void UpdateUIStats(PlayerStats stats, ItemType type)
    {
        if (!hasAuthority) return;
        specialObjLarge.SetActive(true);
        switch (type)
        {
            case ItemType.HealItem:
                break;
            case ItemType.SpeedItem:
                specialFill.color = speedColor;
                specialFillLarge.value = stats.Speed;
                specialValue.text = stats.Speed.ToString();
                break;
            case ItemType.JumpItem:
                specialFill.color = jumpPowerColor;
                specialFillLarge.value = stats.JumpPower;
                specialValue.text = stats.JumpPower.ToString();
                break;
            case ItemType.None:
                specialObjLarge.SetActive(false);
                break;
            default:
                break;
        }

    }

    public void TogglePaused()
    {
        paused = !paused;
    }

    public void Pause()
    {
        TogglePaused();
        pauseWindow.SetActive(paused);
    }

    public void Quit()
    {
        if (isServer)
        {
            NetworkServer.Shutdown();
        }
        NetworkClient.Disconnect();
    }

    #region Health Stats
    public void GetHealthStats()
    {
        stats = GetComponent<PlayerStats>();
        stats.onHealthChanged.AddListener(CmdSetHealthFill);
        if (!hasAuthority)
        {
            healthObjSmall.SetActive(true);
            healthObjLarge.SetActive(false);
        }
        else
        {
            healthObjSmall.SetActive(false);
            healthObjLarge.SetActive(true);
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
