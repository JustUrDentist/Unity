using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    private bool isCoroutineStarted = false;
    private Vector3 spawnPosition = Vector3.zero;
    private GameObject inGamePlayer;
    private GameObject[] enemys;

    private Canvas canvas;
    private GameObject bt;
    private GameObject btq;
    private bool isGameRestarted = false;
    private bool gameIsRunning;
    private Text showPoints, showHighscore;
    private static long points = 0;
    private long highscore;

    public GameObject player;
    public GameObject enemy;

	// Use this for initialization
	void Start ()
    {
        load();

        enemys = new GameObject[0];

        InitializePlayer();

        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        bt = GameObject.Find("RestartButton");
        btq = GameObject.Find("Quit");
        bt.SetActive(false);
        btq.SetActive(false);
        bt.GetComponent<Button>().enabled = false;
        btq.GetComponent<Button>().enabled = false;
        btq.GetComponent<Button>().onClick.AddListener(quit);
        bt.GetComponent<Button>().onClick.AddListener(restartGame);

        showPoints = GameObject.Find("Points").GetComponentInChildren<Text>();
        showHighscore = GameObject.Find("Highscore").GetComponentInChildren<Text>();
        points = 0;

        gameIsRunning = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isCoroutineStarted && inGamePlayer != null && enemys.Length < 6)
        {
            spawnPosition = new Vector3(Random.Range(-6, 6), this.transform.position.y, -this.transform.position.z);
            StartCoroutine(spawnTimer());
        }

        if (inGamePlayer == null)
        {
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemys)
            {
                e.GetComponent<Enemy>().stopMoving = true;
            }

            bt.SetActive(true);
            bt.GetComponent<Button>().enabled = true;
            btq.SetActive(true);
            btq.GetComponent<Button>().enabled = true;
            
            gameIsRunning = false;
        }

        if (!isGameRestarted)
        {
            isGameRestarted = true;
            
        }

        showPoints.text = "Points: " + points.ToString();
        showHighscore.text = "Highscore: " + highscore.ToString();

    }

    private void restartGame()
    {
        bt.SetActive(false);
        bt.GetComponent<Button>().enabled = false;
        btq.SetActive(false);
        btq.GetComponent<Button>().enabled = false;
        
        if (points > highscore)
        {
            save();
        }
        load();
        points = 0;

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemys)
        {
            Destroy(e);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject b in bullets)
        {
            Destroy(b);
        }

        InitializePlayer();

        isGameRestarted = false;
        gameIsRunning = true;
    }

    private void quit()
    {
        Application.Quit();
    }

    private void InitializePlayer()
    {
        inGamePlayer = Instantiate<GameObject>(player, new Vector3(0.0f, -4.0f, 0.0f), Quaternion.Euler(Vector3.zero));
    }

    IEnumerator spawnTimer()
    {
        isCoroutineStarted = true;
        yield return new WaitForSeconds(5);
        if (gameIsRunning)
            Instantiate<GameObject>(enemy, spawnPosition, Quaternion.Euler(Vector3.zero));
        yield return new WaitForSeconds(1);
        if (gameIsRunning)
            Instantiate<GameObject>(enemy, spawnPosition, Quaternion.Euler(Vector3.zero));
        yield return new WaitForSeconds(1);
        if (gameIsRunning)
            Instantiate<GameObject>(enemy, spawnPosition, Quaternion.Euler(Vector3.zero));
        yield return new WaitForSeconds(1);
        if (gameIsRunning)
            Instantiate<GameObject>(enemy, spawnPosition, Quaternion.Euler(Vector3.zero));
        yield return new WaitForSeconds(1);
        if (gameIsRunning)
            Instantiate<GameObject>(enemy, spawnPosition, Quaternion.Euler(Vector3.zero));
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        isCoroutineStarted = false;
    }

    public static void increasePoints()
    {
        GameManager.points += 1;
    }

    public void save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highscore.dat");

        bf.Serialize(file, GameManager.points);
        file.Close();
    }

    public void load()
    {
        if (File.Exists(Application.persistentDataPath + "/highscore.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highscore.dat", FileMode.Open);
            highscore = (long)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            highscore = 0;
        } 
    }
}
