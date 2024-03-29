using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : NetworkBehaviour, IDamageable
{

    [Header("References")]
    [SerializeField] private GameObject _playerSFX;

    [Header("Stats")]
    [SerializeField] private bool isImmortal;
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncMaxHealth))] private float maxHealth;
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncHealth))] private float health;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float minSpeed = 5.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxJumpPower = 12.0f;
    [SerializeField] private float minJumpPower = 6.0f;
    [SerializeField] private float jumpPower = 6.0f;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] int killCount;
    [SerializeField] int deathCount;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos;


    [Header("Respawn/Death")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float respawnDelay;
    public bool isDead;
    public bool isGameOver;
    public bool isMatchPlaying = true;

    [Header("Effects")]
    [Header("Get Damage Effect")]
    public ParticleSystem getDamageEffect;
    public ParticleSystem dieEffect;

    public UnityEvent<float, float> onHealthChanged;
    public static event Func<bool> OnGameOver;
    public static event Action<int, int> OnPvEShowGameOverStats;
    public static event Action<bool, int, int> OnPvPShowGameOverStats;
    public static event Func<int, bool> OnPlayerWin;

    public float Health { get => health; set => health = value; }
    public float Speed { get => speed; set => speed = value; }
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float MaxHealth { get => maxHealth; }
    public float MaxSpeed { get => maxSpeed; }
    public float MinSpeed { get => minSpeed; }
    public float MaxJumpPower { get => maxJumpPower; }
    public float MinJumpPower { get => minJumpPower; }
    public int KillCount { get => killCount; set => killCount = value; }
    public int DeathCount { get => deathCount; set => deathCount = value; }

    [Header("Current Item Type Collected")]
    public ItemType currentItemType;

    #region Damage/Heal

    /// <summary>
    /// Players gets dmg amount of damage back
    /// </summary>
    /// <param name="dmg"></param>
    public void GetDamage(float dmg, GameObject playerObj)
    {
        if (!hasAuthority) return;
        if (isImmortal) return;

        health -= dmg;

        CmdPlayGetDamageVFX();
        CmdPlayGetDamageSFX();

        if (health <= 0)
        {
            health = 0;
            CmdKillPlayer(this.gameObject);
            CmdPlayKillVFX();
            //OnKillPlayer();
            isDead = true;
            CmdSyncPlayerIsDead(this, isDead);
            PvPKillCheck(this);
            PvEKillCheck(this);
            StartCoroutine(WaitTillRespawn());

            if (playerObj.GetComponent<PlayerStats>() != null)
            {
                CmdKillCountSet(playerObj);
            }
        }
        onHealthChanged?.Invoke(Health, MaxHealth);
    }

    void PvEKillCheck(PlayerStats stats)
    {
        if (GameManager.instance.isPvE)
            CmdPvEDeath(this);
    }

    [Command(requiresAuthority = false)]
    public void CmdPvEDeath(PlayerStats stats)
    {
        RpcPvEDeath(stats);
    }

    [ClientRpc]
    public void RpcPvEDeath(PlayerStats stats)
    {
        stats.DeathCount++;
    }

    [Command(requiresAuthority = false)]
    public void CmdKillCountSet(GameObject playerObj)
    {
        RpcKillCountSet(playerObj);
    }

    [ClientRpc]
    public void RpcKillCountSet(GameObject playerObj)
    {
        playerObj.GetComponent<PlayerStats>().KillCount++;
    }

    void PvPKillCheck(PlayerStats player)
    {
        if (GameManager.instance.isPvE) return;

        CmdPvPKill(player);
    }

    [Command]
    public void CmdPvPKill(PlayerStats player)
    {
        RpcPvPKill(player);
    }

    [ClientRpc]
    public void RpcPvPKill(PlayerStats player)
    {
        player.DeathCount++;
    }

    void GameOverCheck()
    {

        if (!GameManager.instance.isPvE)
        {
            bool didPlayerWin = OnPlayerWin.Invoke(KillCount);
            //Debug.Log("any Player win = " + didPlayerWin);
            if (didPlayerWin)
            {

                CmdGameOverPvP();
            }
        }
        else
        {
            isGameOver = OnGameOver.Invoke();


            if (isGameOver && isServer)
            {
                CmdGameOverPvE();
                //this.enabled = false;
            }

        }
    }

    [Command(requiresAuthority = false)]
    void CmdSyncPlayerIsDead(PlayerStats stats, bool isDead)
    {
        stats.isDead = isDead;
    }

    /// <summary>
    /// Player gets healValue amount of health back
    /// </summary>
    /// <param name="healValue"></param>
    public void GetHealth(float healValue)
    {
        if (!hasAuthority) return;
        if (isImmortal) return;

        health += healValue;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        onHealthChanged?.Invoke(Health, MaxHealth);
    }

    #endregion

    #region Kill Player

    /// <summary>
    /// Kills the player on the server
    /// </summary>
    /// <param name="player"></param>
    [Command]
    private void CmdKillPlayer(GameObject player)
    {
        RpcKillPlayer(player);
    }

    /// <summary>
    /// Kills the player on all clients (deactivates all scripts and sets booleans to false)
    /// </summary>
    /// <param name="player"></param>
    [ClientRpc]
    private void RpcKillPlayer(GameObject player)
    {
        var playerUI = player.GetComponent<PlayerUI>();
        var playerGun = player.GetComponent<PlayerGun>();
        playerUI.deadImage.gameObject.SetActive(true);

        playerUI.enabled = false;

        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        //player.GetComponent<PlayerStats>().enabled = false;
        playerGun.CmdDropGunOnServer(this.gameObject, playerGun.throwDirection);
        playerGun.enabled = false;

        player.GetComponent<SpriteRenderer>().sprite = playerUI.deadImage.sprite;

        player.GetComponent<Animator>().enabled = false;

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    #endregion

    #region Respawn Player

    /// <summary>
    /// Respawns player on server
    /// </summary>
    /// <param name="player"></param>
    [Command]
    private void CmdRespawnPlayer(GameObject player)
    {
        RpcRespawnPlayer(player);
    }

    /// <summary>
    /// Respawns player on all clients (every script and booleans are true and enabled again)
    /// </summary>
    /// <param name="player"></param>
    [ClientRpc]
    private void RpcRespawnPlayer(GameObject player)
    {
        player.transform.position = player.GetComponent<PlayerStats>().spawnPoint;

        var playerUI = player.GetComponent<PlayerUI>();
        var playerStats = player.GetComponent<PlayerStats>();

        playerUI.deadImage.gameObject.SetActive(false);

        playerUI.enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        playerStats.enabled = true;
        player.GetComponent<PlayerGun>().enabled = true;

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        playerStats.ResetStats();
        playerStats.GetHealth(MaxHealth);
        isDead = false;
        CmdSyncPlayerIsDead(this, isDead);
    }

    /// <summary>
    /// Waits till player can respawn - respawns player
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitTillRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        if (!isGameOver)
            CmdRespawnPlayer(this.gameObject);
    }

    #endregion

    #region ResetStats

    /// <summary>
    /// Resets players stats
    /// </summary>
    public void ResetStats()
    {
        speed = minSpeed;
        jumpPower = minJumpPower;

        GetComponent<PlayerUI>().UpdateUIStats(this, ItemType.None);
    }
    #endregion

    #region Health

    /// <summary>
    /// Syncs the players health on the server
    /// </summary>
    /// <param name="oldHealth"></param>
    /// <param name="newHealth"></param>
    [Command(requiresAuthority = false)]
    public void CmdServerSyncHealth(float oldHealth, float newHealth)
    {
        health = (int)newHealth;
    }

    /// <summary>
    /// Syncs the players health on all clients
    /// </summary>
    /// <param name="oldHealth"></param>
    /// <param name="newHealth"></param>
    [Command]
    public void CmdServerSyncMaxHealth(float oldHealth, float newHealth)
    {
        maxHealth = (int)newHealth;
    }

    #endregion


    #region SFX

    [Command]
    public void CmdPlayGetDamageSFX()
    {
        RpcPlayGetDamageSFX();
    }

    [ClientRpc]
    public void RpcPlayGetDamageSFX() => AudioManager.instance.PlayOnObject("getHitEffect", _playerSFX);


    #endregion

    #region VFX

    public void PlayEffect(ParticleSystem ps)
    {
        ps.Play();
        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(DamageVFX());
    }

    IEnumerator DamageVFX()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    [Command]
    public void CmdPlayGetDamageVFX()
    {
        RpcPlayGetDamageVFX();
    }
    [ClientRpc]
    public void RpcPlayGetDamageVFX() => PlayEffect(getDamageEffect);

    [Command]
    public void CmdPlayKillVFX()
    {
        RpcPlayDieVFX();
    }
    [ClientRpc]
    public void RpcPlayDieVFX() => PlayEffect(dieEffect);

    #endregion




    private void Start()
    {
        spawnPoint = transform.position;

        OnLoad();
        OnNewGame();
        GameManager.players.Add(this);
    }

    [Client]
    private void Update()
    {
        IsInteracting();
        GameOverCheck();
    }

    [Command(requiresAuthority = false)]
    void CmdGameOverPvP()
    {
        RpcGameOverPvP();
    }

    [ClientRpc]
    void RpcGameOverPvP()
    {
        Debug.Log("RPC Game Over");

        for (int i = 0; i < GameManager.players.Count; i++)
        {
            if (GameManager.players[i] == null) return;

            if (!GameManager.players[i].netIdentity.hasAuthority) continue;

            if (GameManager.players[i].KillCount < GameManager.instance.killsToWin)
            {
                Debug.LogWarning($"Player Loses with {GameManager.players[i].KillCount} kills and {GameManager.players[i].DeathCount} deaths.");

                OnPvPShowGameOverStats?.Invoke(false, GameManager.players[i].KillCount, GameManager.players[i].DeathCount);
                GameManager.players[i].GetComponent<PlayerUI>().gameOverDisplay.SetActive(true);
                GameManager.players[i].GetComponent<PlayerStats>().isGameOver = true;
                if (isMatchPlaying)
                {
                    OnMatchLose();
                    OnMatchEnd();
                }
                break;
            }
            else if (GameManager.players[i].KillCount >= GameManager.instance.killsToWin)
            {
                Debug.LogWarning($"Player Win with {GameManager.players[i].KillCount} kills and {GameManager.players[i].DeathCount} deaths.");

                OnPvPShowGameOverStats?.Invoke(true, GameManager.players[i].KillCount, GameManager.players[i].DeathCount);
                GameManager.players[i].GetComponent<PlayerUI>().gameOverDisplay.SetActive(true);
                GameManager.players[i].GetComponent<PlayerStats>().isGameOver = true;
                if (isMatchPlaying)
                {
                    OnMatchWin();
                    OnMatchEnd();
                }

                break;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdGameOverPvE()
    {
        RpcGameOverPvE();
    }

    [ClientRpc]
    public void RpcGameOverPvE()
    {
        for (int i = 0; i < GameManager.players.Count; i++)
        {
            if (GameManager.players[i] == null) return;

            if (!GameManager.players[i].netIdentity.hasAuthority) continue;

            GameManager.players[i].GetComponent<PlayerUI>().gameOverDisplay.SetActive(true);
            GameManager.players[i].GetComponent<PlayerStats>().isGameOver = true;
            GameManager.players[i].GetComponent<PlayerStats>().isDead = true;
            OnPvEShowGameOverStats?.Invoke(GameManager.players[i].KillCount, GameManager.players[i].DeathCount);
        }
        if (isMatchPlaying)
            OnMatchEnd();
    }

    /// <summary>
    /// Checks if there is something interactable in the players radius
    /// </summary>
    private void IsInteracting()
    {
        var collider = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionMask);

        if (collider == null) return;

        if (collider.GetComponent<IInteractable>() != null)
        {
            if (InputManager.instance.Interact())
            {
                collider.GetComponent<IInteractable>().Interact(this.gameObject);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

    #region Save Stats

    public void OnMatchWin()
    {
        SaveData.PlayerProfile.matchesWon += 1;
    }

    public void OnMatchLose()
    {
        SaveData.PlayerProfile.matchesLost += 1;
    }


    public void OnMatchEnd()
    {

        SaveData.NewGameData.deaths = DeathCount;
        SaveData.NewGameData.kills = KillCount;


        SaveData.PlayerProfile.kills += KillCount;
        SaveData.PlayerProfile.deaths += DeathCount;

        SaveData.PlayerProfile.matchesPlayed += 1;

        OnSave();

        isMatchPlaying = false;


    }


    /// <summary>
    /// Saves file (PlayerProfile) and everything that is in SaveData
    /// </summary>
    public void OnSave()
    {
        SerializationManager.Save("playerData", SaveData.PlayerProfile);
        SerializationManager.Save("newGameData", SaveData.NewGameData);
    }

    public void OnNewGame()
    {

        SaveData.NewGameData.deaths = 0;
        SaveData.NewGameData.kills = 0;


        SerializationManager.Save("newGameData", SaveData.NewGameData);
    }

    /// <summary>
    /// Gets the SaveData file 
    /// </summary>
    public void OnLoad()
    {
        SaveData.PlayerProfile = (PlayerProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/playerData.lutompixel");
        SaveData.NewGameData = (NewGameData)SerializationManager.Load(Application.persistentDataPath + "/saves/newGameData.lutompixel");
    }

    #endregion

    #region Cheats


    /// <summary>
    /// Toggles Player Immortality On Server
    /// </summary>
    [ContextMenu("Toggle Immortality")]
    [Command]
    public void CmdToggleImmortality()
    {
        RpcToggleImmortality();
    }

    /// <summary>
    /// Toggles Player Immortality On All Clients
    /// </summary>
    [ClientRpc]
    public void RpcToggleImmortality()
    {
        isImmortal = !isImmortal;
    }

    #endregion

}

/// <summary>
/// Interactable Item types
/// </summary>
public enum ItemType
{
    HealItem,
    SpeedItem,
    JumpItem,
    None
}
