using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

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

    private readonly string translations = "Translations";
    private readonly string castleHeightLocalization = "CASTLE_HEIGHT";
    private readonly string wallThicknessLocalization = "WALL_THICKNESS";

    public void UpdateTeamStat(CastleStats castleStat)
    {
        sandProduce.text = castleStat.sandProduce.ToString();
        sand.text =  castleStat.sandResource.ToString();

        waterProduce.text =  castleStat.waterProduce.ToString();
        water.text = castleStat.waterResource.ToString();

        magicProduce.text = castleStat.magicProduce.ToString();
        magic.text = castleStat.magicResource.ToString();


        var ch = LocalizationSettings.StringDatabase.GetLocalizedString(translations, castleHeightLocalization);
        var wt = LocalizationSettings.StringDatabase.GetLocalizedString(translations, wallThicknessLocalization);

        castleHeight.text = ch + castleStat.castleHeight.ToString();
        wallHeight.text = wt + castleStat.wallHeight.ToString();
    }
}
