using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class GameCard : GameCardBase,ICard
{
    public GameObject animationPrefab;
    public string cardName;
    public int resourceCost;
    public ResourceType resourceType;
    public int botPriority;
    public Sprite cardIcon;
    [TextArea]
    public string cardDescription;
    public virtual void UseTheCard(CastleStats executor,CastleStats enemy)
    {
        base.UseTheCard(executor, enemy, this);
    }

    public virtual void HandleCardCost(CastleStats executor)
    {
        base.HandleCardCost(executor, this);
    }

    public virtual void BasicAttack(CastleStats enemy, int attackTime)
    {
        base.BasicAttack(enemy, attackTime);
    }

    public virtual void ChangeResource(CastleStats target,ResourceType type,int amount)
    {
        base.ChangeResource(target, type, amount);
    }

    public virtual void ChangeProduce(CastleStats target, ResourceType type, int amount)
    {
        base.ChangeProduce(target, type, amount);
    }

    public virtual void ChangeHeightOfBuilding(CastleStats target,BuildingType type,int amount)
    {
        base.ChangeHeightOfBuilding(target, type, amount);
    }
}

public enum ResourceType
{
    Sand, Water, Magic,Null
}

public enum BuildingType
{
    Wall,Castle
}