using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button playButton;
    public Button playLevel2;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        playLevel2.onClick.AddListener(PlayTest);
    }
    void PlayGame()
    {
        Debug.Log("Starting game!");
        ChangeScene("test");

    }
    void ChangeScene (string levelName)
    {
        SceneManager.LoadScene(levelName);
	}
    void PlayTest()
    {
        SceneManager.LoadScene("AI Army");
    }


}
