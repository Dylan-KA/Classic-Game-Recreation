using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject timeObj;
    [SerializeField] private GameObject vulnerableObj;
    private int playerScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        scoreText.text = "" + playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int score)
    {
        playerScore += score;
        scoreText.text = "" + playerScore;
    }
}
