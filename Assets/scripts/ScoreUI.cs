using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ScoreUI : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI scoreTextPrefab;

    void Update() {
        
        ScoreManager scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        
        string textToShow = "";
        //scoreTextPrefab = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        foreach (PlayerScore playerScore in scoreManager.playerScores) {
            textToShow += $"{playerScore.playerName} : {playerScore.score} points \n";
        }
        scoreTextPrefab.SetText(textToShow);
    }
}