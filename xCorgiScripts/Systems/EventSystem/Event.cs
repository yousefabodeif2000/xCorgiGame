using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Dimensional.Events
{
    public class Event
    {
        public event Action action;
        public float eventTime = 0f;
        public List<EventVFX> eventVFX = new List<EventVFX>();
        public float Time => eventTime;

        /// <summary>
        /// Creates a new event object.
        /// </summary>
        /// <param name="eventAction">The action you want to create this opbject for</param>
        public Event(Action eventAction)
        {
            action = eventAction;
        }
        /// <summary>
        /// Creates a new event object.
        /// </summary>
        /// <param name="eventAction">The action you want to create this opbject for</param>
        /// <param name="time">The time this event wil take to finish executing</param>
        public Event(Action eventAction, float time)
        {
            action = eventAction;
            eventTime = time;
        }


        /// <summary>
        /// Creates a new event object.
        /// </summary>
        /// <param name="eventAction">The action you want to create this opbject for</param>
        /// <param name="time">The time this event wil take to finish executing</param>
        /// <param name="vfx">Add a list of event VFX if you want</param>
        public Event(Action eventAction, List<EventVFX> vfx)
        {
            action = eventAction;
            eventVFX = vfx;
        }


        /// <summary>
        /// Executes a certain vfx in the event vfx list
        /// </summary>
        /// <param name="vfxName"> The name of the vx you want to spwn (should add the vx name to the list)</param>
        /// <param name="pos">Position where you want the vfx to spawn</param>
        public void ExecuteVFX(string vfxName, Vector3 pos)
        {
            foreach (var vfx in eventVFX)
            {
                if (vfx.effectName == vfxName)
                    vfx.SpawnVFX(pos);
            }
        }




        /// <summary>
        /// Executes the action referenced to the object.
        /// </summary>
        public void Execute()
        {
            action?.Invoke();
        }

    }
}
