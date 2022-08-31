using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    //Kills , and death will be shown here

    [SerializeField] TMP_Text killsTmp;
    [SerializeField] TMP_Text deathsTmp;

    [SerializeField] string killsTxt = "Kills: ";
    [SerializeField] string deathTxt = "Deaths: ";

    private void OnEnable()
    {
        PlayerStats.OnShowGameOverStats += ChangeText;
    }

    private void OnDisable()
    {
        PlayerStats.OnShowGameOverStats -= ChangeText;
    }

    void ChangeText(int kills, int deaths)
    {
        killsTmp.text = killsTxt + "" + kills.ToString();
        deathsTmp.text = deathTxt + "" + deaths.ToString();
    }


}
