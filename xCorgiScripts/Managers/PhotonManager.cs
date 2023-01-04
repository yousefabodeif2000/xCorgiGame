using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    static public PhotonManager Instance;

    public Action OnLogin; 
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        UIManager.Instance.LoadingText = "Connecting to server...";
    }
    public override void OnEnable()
    {
        FirebaseManager.OnLogin += Login;
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        UIManager.Instance.LoadingText = "Connected Successfully! Please wait...";
        UIManager.Instance.ShowPanel("Main Screen", true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        UIManager.Instance.Notify("Network Connection Failed", "Please check your cellular or Wi-Fi connection and retry.", UIManager.NotificationType.Error, false);
    }
    void Login()
    {
        AuthenticationValues authValues = new AuthenticationValues();
        authValues.AuthType = CustomAuthenticationType.Custom;
        //authValues.AddAuthParameter("user", userId);
        //authValues.AddAuthParameter("pass", pass);
        //authValues.UserId = userId; // this is required when you set UserId directly from client and not from web service
        PhotonNetwork.AuthValues = authValues;
    }

}
