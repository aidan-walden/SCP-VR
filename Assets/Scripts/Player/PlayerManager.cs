using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(player);
            Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }
}
