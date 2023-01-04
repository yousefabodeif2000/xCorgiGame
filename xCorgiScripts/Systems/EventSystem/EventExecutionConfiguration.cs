using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExecutionConfiguration : MonoBehaviour
{
    public Queue<Dimensional.Events.Event> events = new Queue<Dimensional.Events.Event>();

    public EventExecutionConfiguration(Queue<Dimensional.Events.Event> eventsAdded)
    {
        events = eventsAdded;
    }
    public EventExecutionConfiguration(List<Dimensional.Events.Event> eventsAdded)
    {
        foreach(var _event in eventsAdded)
        {
            events.Enqueue(_event);
        }
    }
    public EventExecutionConfiguration(Dimensional.Events.Event eventsAdded)
    {
        events.Enqueue(eventsAdded);
    }
}
