using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreScript : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject nameText;
    public GameObject rankText;

    public void SetScore(string name, string score, string rank)
    {
        this.rankText.GetComponent<TMP_Text>().text = rank;
        this.nameText.GetComponent<TMP_Text>().text = name;
        this.scoreText.GetComponent<TMP_Text>().text = score;
    }

}
