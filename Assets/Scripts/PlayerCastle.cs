using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PlayerCastle : NetworkBehaviour
{
    [SerializeField]
    GameObject meshObjects;

    [SerializeField]
    Transform maxHeight;

    [SerializeField]
    Transform minHeight;

    [SerializeField]
    Material redMat;

    [SerializeField]
    Material blueMat;

    [SerializeField]
    MeshRenderer[] flagMeshRenderers;
    public Teams Team { get { return castleStats.team; } set { castleStats.team = value; } }
    public static CastleStats PlayerCastleStats { get; private set; }

    private Animator animator;
    private CastleStats castleStats;
    private void Awake()
    {
        castleStats = GetComponent<CastleStats>();
    }
    private void Start()
    {

        UpdateMeshHeight(castleStats.castleHeight);
        animator = GetComponent<Animator>();
    }
    [Command]
    public void CmdGetTeam(NetworkInstanceId id)
    {
        var team = NetworkServer.FindLocalObject(id).GetComponent<PlayerCastle>().Team;
    }
    [ClientRpc]
    public void RpcUpdateTeams(Teams team)
    {
        UpdateTeamFlagMaterials(team);
    }

    public void UpdateTeamFlagMaterials(Teams team)
    {
        Team = team;
        if (team == Teams.Blue)
        {
            for (int i = 0; i < flagMeshRenderers.Length; i++)
            {
                flagMeshRenderers[i].material = blueMat;
            }
        }
        else
        {
            for (int i = 0; i < flagMeshRenderers.Length; i++)
            {
                flagMeshRenderers[i].material = redMat;
            }
        }
    }

    private void UpdateMeshHeight(int height)
    {
        meshObjects.transform.localPosition = GetPositionOfHeight(height);
    }

    private Vector3 GetPositionOfHeight(int height)
    {
        return Vector3.Lerp(minHeight.localPosition, maxHeight.localPosition, (height / 100.0f));
    }

    private void UpdateMeshHeightWithAnim(int newHeight)
    {
        StartCoroutine(CastleHeightChange(1f, newHeight));
    }

    public void OnCastleHeightChanged(int newHeight)
    {
        UpdateMeshHeightWithAnim(newHeight);
        castleStats.castleHeight = newHeight;
    }

    IEnumerator CastleHeightChange(float duration, int newHeight)
    {
        animator.SetBool("Shake", true);
        float currentTime = 0f;
        Vector3 heightPosToGo = GetPositionOfHeight(newHeight);

        while (currentTime <= duration)
        {
            meshObjects.transform.localPosition = Vector3.Lerp(meshObjects.transform.localPosition, heightPosToGo, (currentTime / duration));
            currentTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("Shake", false);
    }

    public void SetStaticCastleStat()
    {
        if (!hasAuthority)
            return;
        SetStaticCastleStatSingle();
    }

    public void SetStaticCastleStatSingle()
    {
        PlayerCastleStats = castleStats;
    }

    [ClientRpc]
    public void RpcGameFinished()
    {
        var turnIndicator = FindObjectOfType<TurnIndicatorUI>();
        if (hasAuthority)
        {
            turnIndicator.SetIndicatorText("You win!");
        }
        else
        {
            turnIndicator.SetIndicatorText("You lose!");
        }
    }

    [Command]
    public void CmdRequestBonusCard(NetworkInstanceId id)
    {
        GameManager.instance.GetBonusCard(id);
    }

    [Command]
    public void CmdTellAdsFinished()
    {
        GameManager.instance.AdsFinished(netId);
    }
}
