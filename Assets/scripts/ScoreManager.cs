using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScore
{
    public string playerName;
    public int score;
}
public class ScoreManager : MonoBehaviourPunCallbacks
{
   public List<PlayerScore> playerScores = new List<PlayerScore>();
   
   public void AddScore(string playerName, int score) {
       if (playerName == null) Debug.Log("test");
       PlayerScore playerScore = new PlayerScore {
           playerName = playerName,
           score = score
       };
       var player = playerScores.FirstOrDefault(p => p.playerName == playerName);
       if(player != null)
       {
           player.playerName = playerScore.playerName;
           player.score = playerScore.score;
       }
       else
       {
           playerScores.Add(playerScore);
       }
       playerScores.Sort((x, y) => y.score.CompareTo(x.score));

       // Mettre à jour le score sur Photon
       //photonView.RPC("UpdateScoreOnNetwork", RpcTarget.All, playerName, score);
   }

   [PunRPC]
   public void UpdateScoreOnNetwork(string playerName, int score)
   {
       // Mettre à jour le score pour tous les joueurs connectés sur Photon
       AddScore(playerName, score);
   }
   
   private void Start() 
   {
       /*if (PhotonNetwork.IsConnected) {
           PhotonNetwork.Instantiate(this.gameObject.name, Vector3.zero, Quaternion.identity);
       } else {
           Debug.LogError("Could not instantiate ScoreManager, not connected to Photon.");
       }*/
   }
}
