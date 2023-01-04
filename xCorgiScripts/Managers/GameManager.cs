using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager Instance;

    public enum States
    {
        InitializeMenu,
        LobbyView,
        ShopView,
        SpellView,
        InRoom,
        LoadingMatch
    }

    States state;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }
    public States GameState
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            switch (state)
            {
                case States.InitializeMenu:

                    break;
                case States.LobbyView:

                    break;
                case States.ShopView:

                    break;
                case States.SpellView:

                    break;
                case States.InRoom:

                    break;
                case States.LoadingMatch:

                    break;
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    private void OnLevelWasLoaded(int level)
    {

    }

}
