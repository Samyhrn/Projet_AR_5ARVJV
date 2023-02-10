using System;
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
        string textToShow = "";
        //scoreTextPrefab = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        foreach (var Player in GameManager.instance.playerScores) {
            textToShow += $"{Player.playerName} : {Player.score} \n";
        }
        scoreTextPrefab.SetText(textToShow);
    }
    
}
