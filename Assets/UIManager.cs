using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIManager : MonoBehaviour 
{ 
    public Image localPlayerHealthImg;

    public Image secondPlayerHealthImg;

    public void UpdatePlayerHealthUI(float health, float maxHealth)
    {
        localPlayerHealthImg.fillAmount = health / maxHealth;
        SyncPlayersHealth(localPlayerHealthImg.fillAmount);
    }

    public void SyncPlayersHealth(float fill)
    {
        ClientSyncHealth(fill);
    }

    public void ClientSyncHealth(float fill)
    {
        secondPlayerHealthImg.fillAmount = fill;

    }
}
