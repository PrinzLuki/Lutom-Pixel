using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelInitializer : NetworkBehaviour
{
    //[SyncVar(hook = nameof(SyncIsPve))]
    private bool isPvE;

    public bool IsPvE { get => isPvE; set => isPvE = value; }


    private void Start()
    {
        GameManager.instance.RestartInputManager();

        if (!isServer) return;
        IsPvE = GameManager.instance.isPvE;
        CmdSetIsPve(IsPvE);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetIsPve(bool isPve)
    {
        RpcSetIsPve(isPve);
    }

    [ClientRpc]
    public void RpcSetIsPve(bool isPve)
    {
        IsPvE = isPve;
        GameManager.instance.isPvE = isPvE;
        Debug.Log($"Is PvE = {isPve}");
    }

    //[Command(requiresAuthority = false)]
    //public void SyncIsPve(bool oldValue, bool newValue)
    //{
    //    IsPvE = newValue;
    //}
}
