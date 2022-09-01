using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    //Kills , and death will be shown here
    [Header("TMP References")]
    [SerializeField] TMP_Text headerTmp;
    [SerializeField] TMP_Text killsTmp;
    [SerializeField] TMP_Text deathsTmp;

    [Header("PvP Headers")]
    [SerializeField] string winnerTxt = "You WIN!";
    [SerializeField] string loserTxt = "You LOSE!";

    [Header("PvE Header")]
    [SerializeField] string gameOverTxt = "Game Over";

    [Header("Stat Text")]
    [SerializeField] string killsTxt = "Kills: ";
    [SerializeField] string deathTxt = "Deaths: ";

    private void OnEnable()
    {
        //PvE
        PlayerStats.OnPvEShowGameOverStats += ChangeText;
        //PvP
        PlayerStats.OnPvPShowGameOverStats += ChangeText;
    }

    private void OnDisable()
    {
        //PvE
        PlayerStats.OnPvEShowGameOverStats -= ChangeText;
        //PvP
        PlayerStats.OnPvPShowGameOverStats -= ChangeText;
    }


    /// <summary>
    /// IsCalled OnGameOver, displays local player stats PvE
    /// </summary>
    /// <param name="kills"></param>
    /// <param name="deaths"></param>
    void ChangeText(int kills, int deaths)
    {
        headerTmp.text = gameOverTxt;
        killsTmp.text = killsTxt + "" + kills.ToString();
        deathsTmp.text = deathTxt + "" + deaths.ToString();
    }

    /// <summary>
    /// IsCalled IfPlayerWins game PvP
    /// </summary>
    /// <param name="didWin"></param>
    /// <param name="kills"></param>
    /// <param name="deaths"></param>
    void ChangeText(bool didWin, int kills, int deaths)
    {
        headerTmp.text = didWin == true ? winnerTxt : loserTxt;

        killsTmp.text = killsTxt + "" + kills.ToString();
        deathsTmp.text = deathTxt + "" + deaths.ToString();
    }
}
