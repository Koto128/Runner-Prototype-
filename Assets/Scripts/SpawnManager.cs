using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    //public
    public GameObject[] obstaclePrefab;
    public GameObject[] playerBuffPrefab;
    public float repeatTime = 2;
    public float repeatRate = 2;
    public float buffRarity;
    public float spawnPositionOffset;
    //private
    private Vector3 spawnPosition = new Vector3(27,1.4f,0);
    private PlayerController playerControllerScript;
    void SpawnerFunctionObstacles()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate<GameObject>(obstaclePrefab[Random.Range(0, obstaclePrefab.Length)], spawnPosition, Quaternion.identity);

        }
    }
    void SpawnerFunctionBuffs()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate<GameObject>(playerBuffPrefab[Random.Range(0, playerBuffPrefab.Length)], spawnPosition * spawnPositionOffset, Quaternion.identity);
        }
    }
    private void Awake()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Start()
    {
        InvokeRepeating("SpawnerFunctionObstacles", repeatTime, repeatRate);
        InvokeRepeating("SpawnerFunctionBuffs", repeatTime, 1 * buffRarity);
    }
}
