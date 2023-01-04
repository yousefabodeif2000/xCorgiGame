using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSender : MonoBehaviour
{
    Player player;
    public Phase phase; //reference the phase spell;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void Footstep()
    {
        player.Footstep();
    }
    public void Execute(string spellName)
    {
        switch (spellName)
        {
            case "Phase": phase.CastSpell() ; break;
        }
    }
    public void PauseMovement(int state)
    {
        if (state == 1)
            player.PauseController(true);
        else if (state == 2)
            player.PauseController(false);
    }
}
