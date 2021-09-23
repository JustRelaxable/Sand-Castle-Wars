using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleStatsUI : MonoBehaviour
{
    public TeamStatsUI blueTeamStats;
    public TeamStatsUI redTeamStats;

    private CastleStats[] castleStats;

    public void OnGameStarted()
    {
        castleStats = FindObjectsOfType<CastleStats>();
        foreach (var castleStat in castleStats)
        {
            castleStat.OnStatChanged += CastleStat_OnStatChanged;
        }
        UpdateUI();
    }

    private void CastleStat_OnStatChanged()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (var castleStat in castleStats)
        {
            if(castleStat.team == Teams.Blue)
            {
                blueTeamStats.UpdateTeamStat(castleStat);
            }
            else
            {
                redTeamStats.UpdateTeamStat(castleStat);
            }
        }
    }
}
