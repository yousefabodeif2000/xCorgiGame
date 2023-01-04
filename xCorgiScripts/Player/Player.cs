using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;

public class Player : CharacterActor
{

    public float emotingTime = 3f;

    [Header("References")]
    public Transform VFX_Holder;
    public GameObject hitEffect;
    public ParticleSystem walkEffect;
    public UltimateJoystick moveJoystick;
    public UltimateJoystick cameraJoystick;





    /// <summary>
    /// Gets the current brain actions CharacterBrain component of the gameObject.
    /// </summary>
    public CharacterActions CharacterActions;
   

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Emote();
        //}

        CharacterActions.movement.value.x = moveJoystick.GetHorizontalAxis();
        CharacterActions.movement.value.y= moveJoystick.GetVerticalAxis();
    }
    public void Emote(string name)
    {
        print("emoting");
        StartCoroutine(Emoting(name));
    }
    IEnumerator Emoting(string name)
    {
        PopupEmote Emoter = GetComponent<PopupEmote>();
        animator.SetTrigger("Emote");
        Emoter.ShowEmote(name);
        yield return new WaitForSeconds(emotingTime);
        PauseController(false);
        Emoter.CloseEmote();
    }
    public void PauseController(bool state)
    {
        brain.enabled = !state;
        if (!state)
        {
            PlayerState = State.None;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Hittable")
        {
            Transform go = Instantiate(hitEffect, collision.GetContact(0).point, Quaternion.identity).transform;
            go.localScale *= 2f;
        }
    }

    public void Spell(Spell spell)
    {
        spell.ExecuteSpell(true);
    }
    public void Stun()
    {
        PauseController(true);
        Emote("Dizzy");
        animator.SetTrigger("Sleep");
        PlayerState = State.Stunned;
    }
    public void Footstep()
    {
        walkEffect.Emit(5);
    }
}
