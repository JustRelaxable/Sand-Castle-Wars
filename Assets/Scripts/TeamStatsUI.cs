using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TeamStatsUI
{
    public Text sandProduce;
    public Text sand;

    public Text waterProduce;
    public Text water;

    public Text magicProduce;
    public Text magic;

    public Text castleHeight;
    public Text wallHeight;

    public void UpdateTeamStat(CastleStats castleStat)
    {
        sandProduce.text = castleStat.sandProduce.ToString();
        sand.text =  castleStat.sandResource.ToString();

        waterProduce.text =  castleStat.waterProduce.ToString();
        water.text = castleStat.waterResource.ToString();

        magicProduce.text = castleStat.magicProduce.ToString();
        magic.text = castleStat.magicResource.ToString();

        castleHeight.text = "Castle Height:" + castleStat.castleHeight.ToString();
        wallHeight.text = "Wall Thickness:" + castleStat.wallHeight.ToString();
    }
}
