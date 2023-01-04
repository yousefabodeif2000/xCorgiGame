using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
public class TriggerInput : MonoBehaviour
{
    public SimpleInput.ButtonInput action = new SimpleInput.ButtonInput();
    Player player;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;

        player = other.GetComponent<Player>();
        action.value = true;


    }
    private void OnEnable()
    {
        action.StartTracking();
    }
    private void OnDisable()
    {
        action.StopTracking();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        player = other.GetComponent<Player>();
        action.value = false;
    }
    private void Update()
    {
        if (!player)
            return;
        player.CharacterActions.jetPack.value = action.value;
    }
}
