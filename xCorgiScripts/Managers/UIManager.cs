using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    [Header("Panels References")]
    public List<Panel> Panels = new List<Panel>();
    public TMP_Text LoadingTextHolder;

    [Header("Notifications References")]
    public TMP_Text NotificationTextHolder;
    public TMP_Text NotificationTitleHolder;
    public Image NotificationIconHolder;
    public Button NotificationOKButton;

    [Header("Authentication References")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;

    [Header("Resources")]
    public Sprite ErrorNotificationIcon;
    public Sprite NormalNotificationIcon;


    #region Events

    #endregion

    private void OnEnable()
    {
        FirebaseManager.OnLogin += Login;
    }

    public string LoadingText
    {
        set
        {
            ShowPanel("Loading", false);
            LoadingTextHolder.text = value;
            if (value == "")
                Panels.Where(p => p.PanelName == "Loading").FirstOrDefault().gameObject.SetActive(false);
        }
    }

    public enum NotificationType
    {
        Normal, Error
    }
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Shows a panel
    /// </summary>
    /// <param name="panelName"> the name of the panel you want to show</param>
    /// <param name="cancelOtherPanels">close other panels when opening this panel?</param>
    /// <param name="hidePlayerAvatar"> whether to hide the player avatar in the main menu or not</param>
    public void ShowPanel(string panelName, bool cancelOtherPanels)
    {
        Panels.Where(panel => panel.PanelName == panelName).FirstOrDefault().gameObject.SetActive(true);
        List<Panel> panelsToDisable = Panels.Where(panel => panel.PanelName != panelName).ToList();

        if (cancelOtherPanels)
        {
            foreach (var panel in panelsToDisable)
                panel.gameObject.SetActive(false);
        }

    }
    public void Notify(string title, string message, NotificationType notificationType, bool showOkButton)
    {
        NotificationTextHolder.text = message;
        NotificationTitleHolder.text = title;
        switch (notificationType)
        {
            case NotificationType.Normal:
                NotificationIconHolder.sprite = NormalNotificationIcon;
                NotificationTitleHolder.color = Color.green;
                NotificationOKButton.gameObject.SetActive(true);
                break;

            case NotificationType.Error:
                NotificationIconHolder.sprite = ErrorNotificationIcon;
                NotificationTitleHolder.color = Color.red;
                NotificationOKButton.gameObject.SetActive(false);
                break;
        }
        NotificationOKButton.gameObject.SetActive(showOkButton);
        ShowPanel("Notification", false);
    }

    public void Login()
    {
        //LOGIN TO MAIN MENU
        SceneManager.LoadSceneAsync(1);
    }
    private void OnLevelWasLoaded(int level)
    {
        LoadingText = "";
    }
    public void RefreshPanelList() 
    {
        foreach(var panel in Panels)
        {
            if (panel == null)
                Panels.Remove(panel);
        }
    }

}
