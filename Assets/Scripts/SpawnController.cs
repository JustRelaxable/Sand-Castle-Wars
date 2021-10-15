using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    Transform redSpawnPoint;

    [SerializeField]
    Transform blueSpawnPoint;

    private void Awake()
    {
        redSpawnPoint = GameObject.FindGameObjectWithTag("RedSpawn").transform;
        blueSpawnPoint = GameObject.FindGameObjectWithTag("BlueSpawn").transform;
    }

    public void ConfigureCastleTransform(Teams team,GameObject castle)
    {
        switch (team)
        {
            case Teams.Blue:
                castle.transform.position = blueSpawnPoint.position;
                castle.transform.rotation = blueSpawnPoint.rotation;
                break;
            case Teams.Red:
                castle.transform.position = redSpawnPoint.position;
                castle.transform.rotation = redSpawnPoint.rotation;
                break;
            default:
                break;
        }
    }
}
