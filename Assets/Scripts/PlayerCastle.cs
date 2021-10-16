using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;
// player castle castle stats player cards castle turn
public class PlayerCastle : MonoBehaviourPun
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
    public void CmdGetTeam(NetworkInstanceId id)
    {
        var team = NetworkServer.FindLocalObject(id).GetComponent<PlayerCastle>().Team;
    }
    [PunRPC]
    public void UpdateTeamsRpc(Teams team)
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
        if (!photonView.IsMine)
            return;
        SetStaticCastleStatSingle();
    }

    public void SetStaticCastleStatSingle()
    {
        PlayerCastleStats = castleStats;
    }

    public void RpcGameFinished()
    {
        //var turnIndicator = FindObjectOfType<TurnIndicatorUI>();
        //if (hasAuthority)
        //{
        //    turnIndicator.SetIndicatorText("You win!");
        //}
        //else
        //{
        //    turnIndicator.SetIndicatorText("You lose!");
        //}
    }

    public void CmdRequestBonusCard(NetworkInstanceId id)
    {
        GameManager.instance.GetBonusCard(id);
    }
    [PunRPC]
    public void TellAdsFinishedRpc(int viewID)
    {
        GameManager.instance.AdsFinished(viewID);
    }

    [PunRPC]
    public void UpdateTransformsRpc(Vector3 position,Vector3 rotation)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
