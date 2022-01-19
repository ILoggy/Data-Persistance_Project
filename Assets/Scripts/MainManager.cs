using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text hightscoreText;

    public int bestScore;
    public string bestPlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    // Start is called before the first frame update
    private void Awake()
    {
        LoadData();
        DataBase data = new DataBase();
        Debug.LogWarning(data.bestPlayerName);
        Debug.LogWarning(data.bestScore);
        Debug.LogWarning(FindObjectOfType<MenuManager>().PlayerName);
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        SaveData();
        hightscoreText.text = "Best score: " + bestPlayerName + " : " + bestScore;
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class DataBase
    {
        public string bestPlayerName;
        public int bestScore;
    }

    void SaveData()
    {
        DataBase DataBase = new DataBase();
        if (m_Points > bestScore)
        {
            DataBase.bestScore = m_Points;
            bestScore = m_Points;
            DataBase.bestPlayerName = FindObjectOfType<MenuManager>().PlayerName;
            bestPlayerName = FindObjectOfType<MenuManager>().PlayerName;

            string json = JsonUtility.ToJson(DataBase);
            string savefile = Application.persistentDataPath + "/Save/savefile.json";
            if (File.Exists(savefile))
            {
                File.WriteAllText(savefile, json);
            }
        }
    }

    void LoadData()
    {
        DataBase DataBase = new DataBase();
        string path = Application.persistentDataPath + "/Save/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, DataBase);
            bestPlayerName = DataBase.bestPlayerName;
            bestScore = DataBase.bestScore;
            hightscoreText.text = "Best score: " + bestPlayerName + " : " + bestScore;
        }


    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
