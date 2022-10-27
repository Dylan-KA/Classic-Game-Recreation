using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    private int playerScore = 0;

    [SerializeField] private GameObject timeObj;

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
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        scoreText.text = "" + playerScore;
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
        //count down from 10
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
        if (lifeCount == 1)
        {
            Destroy(hearts[0].gameObject);
        }
    }
}
