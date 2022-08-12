using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BreakableObjects : NetworkBehaviour, IDamageable
{

    public float health;
    public Animator animator;

    public GameObject miniHealthbarObj;
    public Slider healthSlider;


    public float timeToAppear = 4f;
    private float timeWhenDisappear;

    private void Start()
    {
        healthSlider.maxValue = health;
    }

    public void GetDamage(float dmg)
    {
        CmdAppear();
        health -= dmg;

        CmdSetHealthFill(health);
        if (health <= 0)
        {
            health = 0;

            animator.enabled = true;
            Destroy(gameObject, animator.runtimeAnimatorController.animationClips[0].length);
        }

    }

    private void Update()
    {
        if (miniHealthbarObj.activeSelf && (Time.time >= timeWhenDisappear))
        {
            CmdDisappear();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdDisappear()
    {
        RpcDisappear();
    }

    [ClientRpc]
    public void RpcDisappear()
    {
        miniHealthbarObj.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    public void CmdAppear()
    {
        RpcAppear();
    }

    [ClientRpc]
    public void RpcAppear()
    {
        timeWhenDisappear = Time.time + timeToAppear;
        miniHealthbarObj.SetActive(true);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetHealthFill(float health)
    {
        RpcSetHealthFill(health);
    }

    [ClientRpc]
    public void RpcSetHealthFill(float health)
    {
        healthSlider.value = health;

    }
}
