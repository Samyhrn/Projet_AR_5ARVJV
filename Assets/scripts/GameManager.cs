using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine.UIElements;

public class PlayerScore
{
    public string playerName;
    public int score;
}
public class GameManager : MonoBehaviourPunCallbacks {
    
    [Header("Status")] 
    public bool gameEnded = false;
    [Header("Reference")] 
    public GameObject imageTarget;
    public GameObject endScreen;
    public List<PlayerScore> playerScores = new List<PlayerScore>();
    //instance
    public static GameManager instance;
    private void Awake(){
        instance = this;
    }

    private void Update(){
        // Debug.Log("is tracking "  + DefaultObserverEventHandler.isTracking);
        foreach (GameObject gameObj in GameObject.FindObjectsOfType(typeof(GameObject))){
            if (gameObj.name == "Player(Clone)"){
                gameObj.transform.SetParent(imageTarget.transform);}
        }

        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (!playerScores.Any(x => x.playerName == p.NickName))
            {
                playerScores.Add(new PlayerScore { playerName = p.NickName, score = 0 });
            }
        }

        foreach (var player in playerScores)
        {
            if (player.score == 10)
            {
                gameEnded = true;
                StartCoroutine(OnGameEnded());
                
            }
        }
    }
 

    
    public void UpdateScore(string playerName, int score)
    {
        photonView.RPC("UpdateScoreOnNetwork", RpcTarget.All, playerName, score);
    }
 

    [PunRPC]
    void UpdateScoreOnNetwork(string playerName, int score)
    {
        var player = playerScores.FirstOrDefault(p => p.playerName == playerName);
       
        player.playerName = playerName;
        player.score = score;
       
        playerScores.Sort((x, y) => y.score.CompareTo(x.score));

       
    }
    
    public IEnumerator OnGameEnded()
    {
        yield return new WaitForSeconds(3);
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "main");
       
    }
   
 
   
}