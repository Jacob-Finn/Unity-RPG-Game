using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindWithTag("GameManager").gameObject.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	public void LoadMenu()
    {
        gameManager.LoadMenu(); // Move Game Manager loading scripts here later.
    }
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }
}
