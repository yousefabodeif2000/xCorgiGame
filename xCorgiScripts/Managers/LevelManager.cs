using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance;

    public List<Transform> SpawnPoints;


    private void Awake()
    {
        Instance = this;
    }

}
