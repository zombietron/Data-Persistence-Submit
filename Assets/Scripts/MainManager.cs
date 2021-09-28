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
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public Text playerName;
    private int m_highScore;
    public Text highScoreText;
    public Text inputText;
    public GameObject pNameInput; 

    
    // Start is called before the first frame update
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
        LoadGameData();
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
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(m_Points > m_highScore)
        {
            pNameInput.SetActive(true);
            m_highScore = m_Points;
            
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int highScore;
    }

    
    public void SaveGameData()
    {
        SaveData d = new SaveData();
        d.highScore = m_Points;
        d.name = playerName.text;
        string path = Application.persistentDataPath + "/SaveData.txt";
        string dataToWrite = JsonUtility.ToJson(d);
        File.WriteAllText(path, dataToWrite);
    }

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/SaveData.txt";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData d = JsonUtility.FromJson<SaveData>(json);
            m_highScore = d.highScore;
            highScoreText.text = "Best Score: " + m_highScore + " " + d.name;

        }

    }

    public void InputPlayerName()
    {
        playerName = inputText;
        SaveGameData();
    }
}
