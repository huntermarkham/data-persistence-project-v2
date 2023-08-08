using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text topScoreText;

    public GameObject GameOverText;
    public GameObject highScoreTable;
    public GameObject scorePrefab;

    public Transform scoreParent;

    public int LineCount = 6;
    public int m_Points;

    private bool m_Started = false;
    private bool m_GameOver = false;

    private HighScoreManager highScoreManager;
    private MenuManager menuManager;

    
    // Start is called before the first frame update
    void Start()
    {
        //set variable to allow use to trigger events in HighScoreManager script
        highScoreManager = GameObject.Find("High Score Manager").GetComponent<HighScoreManager>();

        //set menu manager to retrieve player name
        menuManager = GameObject.Find("Menu Manager Container").GetComponent<MenuManager>();

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

                //retrieve and set top score
                DisplayTopScore();
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

    public void DisplayTopScore()
    {
        highScoreManager.GetTopScore();
        topScoreText.text = "Best Score: " + highScoreManager.topName + " : " + highScoreManager.topScore;
    }

    private void ShowScores()
    {
        //grab scores here for sorting
        highScoreManager.GetTopTen();

        for (int i = 0; i < highScoreManager.highScores.Count; i++)
        {
            GameObject temporaryObject = Instantiate(scorePrefab);

            temporaryObject.transform.SetParent(scoreParent);

            temporaryObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            HighScore temporaryScore =highScoreManager.highScores[i];

            temporaryObject.GetComponent<HighScoreScript>().SetScore(temporaryScore.Name, temporaryScore.Score.ToString(), "#" + (i + 1).ToString());
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
        highScoreTable.SetActive(true);

        //insert high score into DB
        highScoreManager.InsertScore(menuManager.playerName, m_Points);

        //displays scores in high score table
        ShowScores();
    }
}
