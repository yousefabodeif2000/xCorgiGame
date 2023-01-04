using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    static public PlayerAvatar Instance;

    private void Awake()
    {
        Instance = this;
    }
}
