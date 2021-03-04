using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text lblScore;
    public Text lblGameOver;
    public RectTransform dimPanel;
    public RectTransform bgPanel;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void IncrementScore() {
        score++;
        lblScore.text = "Score: " + score.ToString();
    }

    public void GameOver() {
        dimPanel.gameObject.SetActive(true);
        bgPanel.gameObject.SetActive(true);
        lblGameOver.text = "Game Over\n Killed By A Zombie\n Final Score: " + score;
        Time.timeScale = 0;
    }
}
