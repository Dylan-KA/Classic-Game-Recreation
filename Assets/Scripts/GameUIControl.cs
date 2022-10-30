using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameUIControl : MonoBehaviour
{
    [SerializeField] private GameObject startCountdown;
    private TextMeshProUGUI countdownText;
    float timer = 4.0f;
    private int timerIntSecs = 4;
    public bool timerStart = true;

    [SerializeField] private GameObject gameoverObj;
    private TextMeshProUGUI gameovertext;

    [SerializeField] private GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    private int playerScore = 0;

    [SerializeField] private GameObject timeObj;
    private TextMeshProUGUI timeText;
    private float timeElapsed = 0.0f;
    public bool timerGoing;
    private TimeSpan timeSpan;

    [SerializeField] private GameObject vulnerableLabelObj;
    [SerializeField] private GameObject vulnerableObj;
    private TextMeshProUGUI vulnerableText;
    private float vulnTimer = 11.0f;
    private int seconds = 11;
    private bool vulnTimerStart = false;

    [SerializeField] private GameObject[] hearts;
    private int lifeCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        countdownText = startCountdown.GetComponent<TextMeshProUGUI>();

        gameovertext = gameoverObj.GetComponent<TextMeshProUGUI>();
        gameoverObj.SetActive(false);

        timeText = timeObj.GetComponent<TextMeshProUGUI>();
        timerGoing = true;

        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        scoreText.text = "00:00:00";

        vulnerableText = vulnerableObj.GetComponent<TextMeshProUGUI>();
        vulnerableText.text = "";
        vulnerableObj.SetActive(false);
        vulnerableLabelObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (vulnTimerStart)
        {
            vulnTimer -= Time.deltaTime;
            seconds = (int)vulnTimer;
            vulnerableText.text = "" + seconds;
        }
        if (seconds <= 0)
        {
            vulnTimerStart = false;
            vulnerableObj.SetActive(false);
            vulnerableLabelObj.SetActive(false);
        }
        if (timerStart)
        {
            timer -= Time.deltaTime;
            timerIntSecs = (int)timer;
            countdownText.text = "" + timerIntSecs;
            if (timerIntSecs <= 0)
            {
                countdownText.text = "GO!";
            }
            if (timerIntSecs <= -1)
            {
                countdownText.text = "";
                startCountdown.SetActive(false);
                timerStart = false;
                StartCoroutine(updateGameTimer());
            }
        }
    }

    private IEnumerator updateGameTimer()
    {
        while (timerGoing)
        {
            timeElapsed += Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(timeElapsed);
            String timerString = timeSpan.ToString("mm':'ss':'ff");
            timeText.text = timerString;
            yield return null;
        }
    }

    public void addScore(int score)
    {
        playerScore += score;
        scoreText.text = "" + playerScore;
    }

    public void vulnerableCountdown()
    {
        vulnerableObj.SetActive(true);
        vulnerableLabelObj.SetActive(true);

        vulnTimerStart = true;
        vulnTimer = 11.0f;
    }

    public Boolean vulnTimerGoing()
    {
        return vulnTimerStart;
    }

    public void removeLife()
    {
        lifeCount -= 1;
        if (lifeCount == 2)
        {
            Destroy(hearts[2].gameObject);
        }
        if (lifeCount == 1)
        {
            Destroy(hearts[1].gameObject);
        }
        if (lifeCount == 0)
        {
            Destroy(hearts[0].gameObject);
        }
    }

    public int getScore()
    {
        return playerScore;
    }

    public float getTime()
    {
        return timeElapsed;
    }

    public IEnumerator displayGameover()
    {
        gameovertext.text = "Game Over";
        gameoverObj.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        gameoverObj.SetActive(false);
    }

    public Boolean noLivesLeft()
    {
        if (lifeCount == 0)
        {
            return true;
        }
        return false;
    }
}
