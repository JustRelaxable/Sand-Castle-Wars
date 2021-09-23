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
        sandProduce.text = "Sand Produce:"+ castleStat.sandProduce.ToString();
        sand.text = "Sand:" + castleStat.sandResource.ToString();

        waterProduce.text = "Water Produce:" + castleStat.waterProduce.ToString();
        water.text = "Water:" + castleStat.waterResource.ToString();

        magicProduce.text = "Magic Produce:" + castleStat.magicProduce.ToString();
        magic.text = "Magic:" + castleStat.magicResource.ToString();

        castleHeight.text = "Castle Height:" + castleStat.castleHeight.ToString();
        wallHeight.text = "Wall Height:" + castleStat.wallHeight.ToString();
    }
}
