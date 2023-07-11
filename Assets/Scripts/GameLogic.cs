using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] spawnPoints;

    // Update is called once per frame
    void Update()
    {
        if (!playerController.alive)
        {
            playerController.currentHealth = 100;
            playerController.alive = true;
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        GameObject sp = GetNearestSpawnPoint();
        player.transform.position = sp.transform.position;
    }

    private GameObject GetNearestSpawnPoint()
    {
        float min = Mathf.Infinity;
        int minIndex = 0;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float sqrDistance = (player.transform.position - spawnPoints[i].transform.position).sqrMagnitude;

            if (sqrDistance < min)
            {
                min = sqrDistance;
                minIndex = i;
            }
        }

        return spawnPoints[minIndex];
    }
}
