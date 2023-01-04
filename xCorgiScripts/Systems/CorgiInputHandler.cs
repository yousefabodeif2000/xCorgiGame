using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Implementation;
public class CorgiInputHandler : InputHandler
{
    public override bool GetBool(string actionName)
    {
        return SimpleInput.GetButton(actionName);
    }

    public override float GetFloat(string actionName)
    {
        return SimpleInput.GetAxis(actionName);
    }

    public override Vector2 GetVector2(string actionName)
    {
        return new Vector2(SimpleInput.GetAxis(actionName + " X"), SimpleInput.GetAxis(actionName + " Y"));
    }
}
