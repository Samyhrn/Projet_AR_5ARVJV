﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkManager : MonoBehaviourPunCallbacks {

    public static NetworkManager instance;
    private void Awake() {
        if (instance != null && instance != this) {
            gameObject.SetActive(false);
        }
        else {
            instance = this; DontDestroyOnLoad(gameObject);
        }
    }
    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName) {
        PhotonNetwork.CreateRoom(roomName);
    }
    public void JoinRoom(string roomName) {
        if (PhotonNetwork.PlayerList.Length <= 4) {
            PhotonNetwork.JoinRoom(roomName);
        }
    }
    [PunRPC]
    public void ChangeScene(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
