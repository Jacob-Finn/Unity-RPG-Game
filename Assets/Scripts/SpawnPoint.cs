using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            print("Error finding gameManager with spawnpoint");
            return;
        }
        gameManager.spawnPoint = this.gameObject;
        gameManager.Init();
    }
}
