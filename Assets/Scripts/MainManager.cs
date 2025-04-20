using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using static MainManager;

public class MainManager : MonoBehaviour
{
   private ButtonCon ButtonCon;
    private PlayerDataController playerDataController;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    private int best = 0;
    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private void Awake()
    {
        playerDataController=GetComponent<PlayerDataController>();
        ButtonCon = GetComponent<ButtonCon>();
    }

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
        if(m_Points>ButtonCon.maxscore)
        {
            ButtonCon.maxscore = m_Points;
            playerDataController.SavePlayerData("John",ButtonCon.maxscore);
           


        }
    }
    [System.Serializable]
    class PlayerData
    {
        public string playername; // 封装一个玩家名字数据
        public int bestscore;     // 封装一个玩家最高分数据
    }

    public class PlayerDataController : MonoBehaviour
    {
        public void SavePlayerData(string name, int score)
        {
            PlayerData data = new PlayerData(); // 实例化数据
            data.playername = name;
            data.bestscore = score;

            string json = JsonUtility.ToJson(data); // 数据转换为json

            File.WriteAllText(Application.persistentDataPath + "/playerData.json", json); // 写入路径
        }

        public void LoadPlayerData()
        {
            string path = Application.persistentDataPath + "/playerData.json";
            if (File.Exists(path)) // 检测路径是否存在
            {
                string json = File.ReadAllText(path); // 从路径找到json文件
                PlayerData data = JsonUtility.FromJson<PlayerData>(json); // 将json转回数据

                // 将加载的数据赋值给相应的变量或UI组件
                Debug.Log("Loaded Player Name: " + data.playername);
                Debug.Log("Loaded Best Score: " + data.bestscore);
            }
        }
    }



}


