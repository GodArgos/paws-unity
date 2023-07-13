using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Animator anim_loadingScreen;
    [SerializeField] private float timer = 5f;

    private bool inRespawning = false;

    // Update is called once per frame
    void Update()
    {
        if (!playerController.alive && !inRespawning)
        {
            loadingScreen.SetActive(true);
            anim_loadingScreen.SetBool("isLoading", true);

            inRespawning = true;
        }

        if (inRespawning)
        {
            if (timer >= 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                
                SpawnPlayer();
            }
        }
    }

    private void SpawnPlayer()
    {
        GameObject sp = GetNearestSpawnPoint();
        player.transform.position = sp.transform.position;

        anim_loadingScreen.SetBool("isLoading", false);

        timer = 5f;
        inRespawning = false;

        playerController.currentHealth = 100;
        playerController.alive = true;
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
