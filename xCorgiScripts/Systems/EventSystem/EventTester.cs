using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Dimensional.Events;

public class EventTester : MonoBehaviour, IEventable
{
    public Action OnStartTest;
    public Action OnStartTest2;
    public Action OnStartTest3;

    public List<Action> events = new List<Action>();

    EventExecutionConfiguration EventConfiguration;

    EventExecutor EventExecutor = new EventExecutor();
   // Dimensional.Events.Event eventTestObj;
    // Start is called before the first frame update
    void Start()
    {
        //eventTestObj = new Dimensional.Events.Event("Start Event");
        InitializeEvents();

        StartCoroutine(EventExecutor.ExecuteEvents(EventConfiguration));
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    void Printer1()
    {
        print("event 1 test is working");
    }
    void Printer2()
    {
        print("event2 test is working");
    }
    void Printer3()
    {
        print("event3 test is working");
    }

    public void InitializeEvents()
    {
        OnStartTest += Printer1;
        OnStartTest2 += Printer2;
        OnStartTest3 += Printer3;

        events.Add(OnStartTest);
        events.Add(OnStartTest2);
        events.Add(OnStartTest3);

        List<Dimensional.Events.Event> eventsToAdd = new List<Dimensional.Events.Event>();

        foreach(var action in events)
        {
            eventsToAdd.Add(new Dimensional.Events.Event(action, 3f));
        }

        EventConfiguration = new EventExecutionConfiguration(eventsToAdd);
    }

}

[Serializable]
public class EventExecutor
{
    public IEnumerator ExecuteEvents(EventExecutionConfiguration configuration)
    {
        foreach (Dimensional.Events.Event targetedEvent in configuration.events)
        {
            targetedEvent.Execute();
            yield return new WaitForSeconds(targetedEvent.eventTime);
            configuration.events.Dequeue();
        }
    }
}


