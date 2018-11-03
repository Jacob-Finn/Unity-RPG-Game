using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    static public Dictionary<GameObject, Actor> linkedActors = new Dictionary<GameObject, Actor>();
    static public Dictionary<GameObject, WeaponContainer> linkedWeapons = new Dictionary<GameObject, WeaponContainer>();
    static public Dictionary<GameObject, ItemContainer> linkedItems = new Dictionary<GameObject, ItemContainer>();
    static public GameObject pauseMenu;
    public GameObject spawnPoint;
    public GameObject player;
    public PlayerControl playerControl;
    public Actor playerActor;
    public GameObject playerCanvas;
    public Slider playerHealthBar; 
    static public bool created = false;
    // Use this for initialization
    void Awake()
    {
        if (created == true)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }
    private void Start()
    {
        if (spawnPoint == null) return;
        Init();
    }
    public void Init()
    {
        print("Initating");
        player = (GameObject)Resources.Load("Player");
        player = Instantiate(player, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCanvas = GameObject.FindWithTag("UI");
        pauseMenu = playerCanvas.transform.Find("Popup Menu").gameObject;
        playerHealthBar = playerCanvas.transform.Find("HealthBar").gameObject.GetComponent<Slider>();
        playerControl = player.GetComponentInChildren<PlayerControl>();
        playerActor = player.GetComponentInChildren<Actor>();
        playerActor.healthBar = playerHealthBar;
    }

    public static void OpenMenu(PlayerControl playerControl)
    {
        playerControl.enabled = false;  
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        GameManager.pauseMenu.SetActive(true);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.pauseMenu.SetActive(false);
        Time.timeScale = 1;
        playerControl.enabled = true;

    }
}
	