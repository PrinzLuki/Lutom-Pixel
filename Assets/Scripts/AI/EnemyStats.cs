using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    //public SpawnManager spawnManager;

    [SerializeField] float health = 10;
    [SerializeField] float attackDmg = 1;
    [SerializeField] float speed = 1;
    [SerializeField] float chaseSpeed = 2;
    [SerializeField] float jumpPower = 2;

    public static Action<GameObject> OnKill;

    bool isDead;

    [Header("Effects")]
    [Header("Get Damage Effect")]
    public ParticleSystem getDamageEffect;
    public ParticleSystem dieEffect;

    public float Speed { get => speed; }
    public float AttackDmg { get => attackDmg; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float JumpPower { get => jumpPower; }
    public float Health { get => health; }

    public void GetDamage(float dmg, GameObject playerObj)
    {
        health -= dmg;
        Debug.Log("GetDamage");
        CmdPlayGetDamageVFX();
        if (health <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                if (playerObj.GetComponent<PlayerStats>() != null)
                {

                    OnKill?.Invoke(playerObj);

                }
            }
            CmdPlayKillVFX();
            NetworkServer.Destroy(gameObject);

        }
    }

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

    [Command(requiresAuthority = false)]
    public void CmdPlayGetDamageVFX()
    {
        RpcPlayGetDamageVFX();
    }
    [ClientRpc]
    public void RpcPlayGetDamageVFX() => PlayEffect(getDamageEffect);

    [Command(requiresAuthority = false)]
    public void CmdPlayKillVFX()
    {
        RpcPlayDieVFX();
    }
    [ClientRpc]
    public void RpcPlayDieVFX() => PlayEffect(dieEffect);

    #endregion
}
