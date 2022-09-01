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
    public AudioManager audioManager;

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

    [Header("Kill Counter UI")]
    public TMP_Text killCounterTxt;

    [Header("Pause UI")]
    public GameObject pauseWindow;
    public GameObject[] windows;
    public bool paused;

    [Header("Game Over UI")]
    public GameObject gameOverDisplay;

    [Header("Music UI")]
    public TextMeshProUGUI musicTitle;

    [Header("Other")]
    private ChatUI chatUI;

    private void OnEnable()
    {
        EnemyStats.OnKill += CountKills;
    }
    private void OnDisable()
    {
        EnemyStats.OnKill -= CountKills;
    }

    private void Start()
    {
        chatUI = GetComponent<ChatUI>();
        audioManager = FindObjectOfType<AudioManager>();
        gameOverDisplay.SetActive(false);
        if (audioManager == null)
        {
            Debug.Log("AudioManager has not been found");
        }
        else
        {
            musicTitle.text = audioManager.GetSongName();
        }

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

        if (audioManager.otherSongPlaying)
        {
            musicTitle.text = audioManager.GetSongName();
            audioManager.otherSongPlaying = false;
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

    #region Kill Counter

    void CountKills(GameObject playerObj)
    {
        var stats = playerObj.GetComponent<PlayerStats>();
        stats.KillCount++;
        ShowKillsInUI(stats.KillCount);
        SaveData.PlayerProfile.kills += stats.KillCount;
        SerializationManager.Save("playerData", SaveData.PlayerProfile);
    }

    void ShowKillsInUI(int kills)
    {
        killCounterTxt.text = kills.ToString();
    }

    #endregion

    public void Pause()
    {
        if (!paused && chatUI.chatIsOpened)
        {
            chatUI.SendPlayerMessage();
            return;
        }

        TogglePaused();
        pauseWindow.SetActive(paused);


        foreach (var window in windows)
        {
            window.SetActive(false);
        }
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


    public void GetNextSong()
    {
        musicTitle.text = audioManager.ChangeToNextSong();
    }

    public void GetPrevSong()
    {
        musicTitle.text = audioManager.ChangeToPrevSong();
    }

}
