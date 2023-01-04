using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerReferences : MonoBehaviour
{
    public static PlayerReferences Instance;

    public CinemachineVirtualCamera TopViewCam;
    public CinemachineFreeLook TPCam;
    public CinemachineImpulseSource CinemachineImpulseSource;

    public ParticleSystem moveEffect;

    private void Awake()
    {
        Instance = this;
    }
}
