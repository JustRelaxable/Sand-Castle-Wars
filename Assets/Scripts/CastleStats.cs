using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;

public class CastleStats : MonoBehaviourPun
{
    public event Action OnStatChanged;

    public Teams team;

    //[SyncVar(hook = "OnCastleHeightChanged")]
    public int castleHeight = 30;
    //[SyncVar(hook = "OnWallHeightChanged")]
    public int wallHeight = 10;

    //[SyncVar(hook = "OnSandProduceChanged")]
    public int sandProduce = 2;
    //[SyncVar(hook = "OnSandResourceChanged")]
    public int sandResource = 5;

    //[SyncVar(hook = "OnWaterProduceChanged")]
    public int waterProduce = 2;
    //[SyncVar(hook = "OnWaterResourceChanged")]
    public int waterResource = 5;


    //[SyncVar(hook = "OnMagicProduceChanged")]
    public int magicProduce = 2;
    //[SyncVar(hook = "OnMagicResourceChanged")]
    public int magicResource = 5;

    private PlayerCastle playerCastle;
    private void Start()
    {
        playerCastle = GetComponent<PlayerCastle>();
    }

    private void Update()
    {
        //if (!hasAuthority)
        //    return;
        //if (Input.GetKeyDown(KeyCode.E))
        //CmdChangeCastleHeight(5);
    }
    private void CmdChangeCastleHeight(int v)
    {
        castleHeight += v;
    }

    private void OnCastleHeightChanged(int newHeight)
    {
        playerCastle.OnCastleHeightChanged(newHeight);
        InvokeOnStatChanged();
    }

    private void OnWallHeightChanged(int newValue)
    {
        wallHeight = newValue;
        InvokeOnStatChanged();
    }
    private void OnWaterProduceChanged(int newValue)
    {
        waterProduce = newValue;
        InvokeOnStatChanged();
    }

    private void OnWaterResourceChanged(int newValue)
    {
        waterResource = newValue;
        InvokeOnStatChanged();
    }
    private void OnSandProduceChanged(int newValue)
    {
        sandProduce = newValue;
        InvokeOnStatChanged();
    }
    private void OnSandResourceChanged(int newValue)
    {
        sandResource = newValue;
        InvokeOnStatChanged();
    }
    private void OnMagicProduceChanged(int newValue)
    {
        magicProduce = newValue;
        InvokeOnStatChanged();
    }
    private void OnMagicResourceChanged(int newValue)
    {
        magicResource = newValue;
        InvokeOnStatChanged();
    }
    private void InvokeOnStatChanged()
    {
        OnStatChanged?.Invoke();
    }
    [PunRPC]
    public void HandleNextTurnResourcesRpc()
    {
        HandleNextTurnResources();
    }

    public void HandleNextTurnResources()
    {
        sandResource += sandProduce;
        waterResource += waterProduce;
        magicResource += magicProduce;
        InvokeOnStatChanged();
    }

    [PunRPC]
    public void SetResourceRpc(byte resourceType, int value)
    {
        SetResource(resourceType, value);
    }

    public void SetResource(byte resourceType, int value)
    {
        var type = (ResourceType)resourceType;
        switch (type)
        {
            case ResourceType.Sand:
                sandResource = value;
                OnSandResourceChanged(sandResource);
                break;
            case ResourceType.Water:
                waterResource = value;
                OnWaterResourceChanged(waterResource);
                break;
            case ResourceType.Magic:
                magicResource = value;
                OnMagicResourceChanged(magicResource);
                break;
            default:
                break;
        }
    }
    [PunRPC]
    public void SetProduceRpc(byte resourceType, int value)
    {
        SetProduce(resourceType, value);
    }

    public void SetProduce(byte resourceType, int value)
    {
        var type = (ResourceType)resourceType;
        switch (type)
        {
            case ResourceType.Sand:
                sandProduce = value;
                OnSandProduceChanged(sandProduce);
                break;
            case ResourceType.Water:
                waterProduce = value;
                OnWaterProduceChanged(waterProduce);
                break;
            case ResourceType.Magic:
                magicProduce = value;
                OnMagicProduceChanged(magicProduce);
                break;
            default:
                break;
        }
    }

    [PunRPC]
    public void SetHeightOfBuildingRpc(byte buildingType, int value)
    {
        SetHeightOfBuilding(buildingType, value);
    }

    public void SetHeightOfBuilding(byte buildingType, int value)
    {
        BuildingType type = (BuildingType)buildingType;
        switch (type)
        {
            case BuildingType.Wall:
                wallHeight = value;
                OnWallHeightChanged(wallHeight);
                break;
            case BuildingType.Castle:
                castleHeight = value;
                OnCastleHeightChanged(castleHeight);
                break;
            default:
                break;
        }
    }
    [PunRPC]
    public void BasicAttackRpc(int attackTime)
    {
        BasicAttack(attackTime);
    }

    public void BasicAttack(int attackTime)
    {
        bool wall = false;
        bool castle = false;
        for (int i = 0; i < attackTime; i++)
        {
            if (wallHeight > 0)
            {
                wallHeight--;
                wall = true;
            }
            else if (castleHeight > 0)
            {
                castleHeight--;
                castle = true;
            }
        }

        if (wall)
            OnWallHeightChanged(wallHeight);
        if (castle)
            OnCastleHeightChanged(castleHeight);
    }
}
