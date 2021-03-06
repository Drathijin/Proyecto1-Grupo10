using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    UIManager theUIManager;
    WinManager theWinManager;
    SaveManager theSaveManager;
    const float loseTime = 0.8f;
    static bool videoOnce = true;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (theWinManager != null && theWinManager.maxSeconds > -1 && theUIManager != null) theUIManager.SeeTime(theWinManager.GetTime(), theWinManager.maxSeconds);
    }


    public void SetSaveManager(SaveManager saveManager)
    {
        theSaveManager = saveManager;
    }
    public void SetUIManager(UIManager UIManager)
    {
        theUIManager = UIManager;
        Pause();
        theUIManager.StartLevel();

    }

    public void SetWinManager(WinManager winMan)
    {
        theWinManager = winMan;
    }


    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public void FinishLevel(bool win)
    {
        theWinManager.PlayEndSound(win);
        theUIManager.FinishLevel(win, theSaveManager.GetAct()+2); //al sumar dos ignoramos el menu y el tutorial
        if (!win) Invoke("Pause", loseTime);
        else Invoke("Pause", 0);

        // provisional, es para que funcione el tutorial
        if (SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "Menu")
        {
            theSaveManager.FinishLevel(win);
            theSaveManager.SaveGame();
        }
    }

    public void LoadLevel(int n)
    {
        Pause();
        SceneManager.LoadScene(n);
    }
    public void LoadLevel2(int n)
    {
        Pause();
        SceneManager.LoadScene(n+1);
    }
    public void AddCollectable()
    {
        theWinManager.AddCollectable();
        theUIManager.PlayerCollected(theWinManager.GetCollectables(), theWinManager.maxCollectables);
    }
    public void KillEnemy()
    {
        if (theWinManager.maxKills > -1)
        {
            theWinManager.SubKillCount();
            theUIManager.PlayerKills(theWinManager.GetKillCount());
        }
    }
    public void LoadGame()
    {
        theSaveManager.LoadGame();
        LoadLevel(theSaveManager.GetAct()+2);
        print(theSaveManager.GetAct());
    }
    public void NewGame()
    {
        theSaveManager.NewSave();
        LoadLevel(2); //empieza en la escena 2 porque la 1 es el men� principal
    }

    public void Pause() 
    {
        Time.timeScale = 0f;
    }
    public void Play() 
    {
        Time.timeScale = 1f;
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public float GetLoseTime() {return loseTime;}

    public bool GetIniVideoPlay()
    {
        return videoOnce;
    }

    public static void SetIniVideoPlay()
    {
        videoOnce = false;
    }
}