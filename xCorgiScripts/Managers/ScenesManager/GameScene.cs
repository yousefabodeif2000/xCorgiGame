using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameScene : MonoBehaviour
{
    public abstract void Initialize();
    public List<Panel> scenePanels;
    public virtual void Start()
    {
        StartCoroutine(Initializing());
    }
    IEnumerator Initializing()
    {
        while(GameManager.Instance == null)
        {
            print("Waiting on managers...");
            UIManager.Instance.LoadingText = "Retrieving user data, please wait...";
            yield return null;
        }
        UIManager.Instance.LoadingText = "";
        foreach(var panel in scenePanels)
        {
            UIManager.Instance.Panels.Add(panel);
        }
        UIManager.Instance.RefreshPanelList();
        Initialize();
    }
}
