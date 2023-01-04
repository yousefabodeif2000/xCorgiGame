using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : GameScene
{

    [Header("Camera References")]
    public CinemachineVirtualCamera MainLobbyCamera;
    public CinemachineVirtualCamera ShopPanelCamera;
    public CinemachineVirtualCamera SpellPanelCamera;
    public CinemachineVirtualCamera RacePanelCamera;

    [Header("Other References")]
    [SerializeField]
    private PlayableDirector Director;
    [SerializeField]
    private PlayableAsset AvatarStand;    
    [SerializeField]
    private PlayableAsset AvatarFall;

    public override void Initialize()
    {
        print("menu initialized");
    }

    public enum PanelType
    {
        MainLobby,
        ShopPanel,
        SpellPanel,
        RacePanel
    }

    PanelType panel;

    public PanelType CurrentPanel
    {
        get
        {
            return panel;
        }
        set
        {
            panel = value;
            switch (value)
            {
                case PanelType.MainLobby:
                    UIManager.Instance.ShowPanel("Main Lobby", true);
                    Director.playableAsset = AvatarStand;
                    Director.Play();
                    break;
                case PanelType.ShopPanel:
                    UIManager.Instance.ShowPanel("Shop", true);
                    Director.playableAsset = AvatarFall;
                    Director.Play();
                    break;
                case PanelType.SpellPanel:
                    UIManager.Instance.ShowPanel("Spell", true);
                    Director.playableAsset = AvatarFall;
                    Director.Play();
                    break;
                case PanelType.RacePanel:
                    // UIManager.Instance.ShowPanel("Race", true);
                    Director.playableAsset = AvatarFall;
                    Director.Play();
                    break;
            }
        }
    }

   
    void PrioritizeCamera(CinemachineVirtualCamera camera)
    {
        List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
        cameras.Add(MainLobbyCamera);
        cameras.Add(ShopPanelCamera);
        cameras.Add(SpellPanelCamera);
        cameras.Add(RacePanelCamera);

        cameras.Where(c => c == camera).FirstOrDefault().Priority = 10;

        List<CinemachineVirtualCamera> otherCams = cameras.Where(c => c != camera).ToList();
        foreach(var cam in otherCams)
        {
            cam.Priority = 0;
        }
    }

    public void ShowPanel(string panelName)
    {
        switch (panelName)
        {
            case "main":
                CurrentPanel = PanelType.MainLobby;
                break;
            case "shop":
                CurrentPanel = PanelType.ShopPanel;
                break;
            case "spell":
                CurrentPanel = PanelType.SpellPanel;
                break;
            case "race":
                CurrentPanel = PanelType.RacePanel;
                break;
        }
    }
}
