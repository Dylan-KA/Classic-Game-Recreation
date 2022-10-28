using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class StartUIControl : MonoBehaviour
{
    [SerializeField] private GameObject HighScoreObj;
    private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject TimeObj;
    private TextMeshProUGUI timeText;

    private TimeSpan timeSpan;

    // Start is called before the first frame update
    void Start()
    {
        highscoreText = HighScoreObj.GetComponent<TextMeshProUGUI>();
        timeText = TimeObj.GetComponent<TextMeshProUGUI>();
        setHighScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHighScoreText()
    {
        highscoreText.text = "" + PlayerPrefs.GetInt("HighScore");
        timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Time"));
        String timerString = timeSpan.ToString("mm':'ss':'ff");
        timeText.text = "" + timerString;
    }
}
