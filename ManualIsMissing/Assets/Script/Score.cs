using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text ScoreText;

    private void Awake()
    {
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();
    }

    private void Update()
    {
        ScoreText.text = Level.GetInstance().GetCastlePassedCount().ToString();
    }
}
