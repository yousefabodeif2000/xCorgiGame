using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using TMPro;
using Firebase.Extensions;
using System;
public class FirebaseManager : MonoBehaviour
{
    static public FirebaseManager Instance;

    /// <summary>
    /// our current user.
    /// </summary>
    static UserPlayer user;

    /// <summary>
    /// our current user property
    /// </summary>
    static public UserPlayer CurrentUser
    {
        get
        {
            return user;
        }
        set
        {

        }
    }

    static public Action OnLogin;

    #region Firebase Vars
    //Firebase variables
    [Header("Firebase Settings")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser FBUser;

    //Login variables
    protected TMP_InputField emailLoginField
    {
        get
        {
            return UIManager.Instance.emailLoginField;
        }
    }
    protected TMP_InputField passwordLoginField
    {
        get
        {
            return UIManager.Instance.passwordLoginField;
        }
    }
    //Register variables
    protected TMP_InputField usernameRegisterField
    {
        get
        {
            return UIManager.Instance.usernameRegisterField;
        }
    }
    protected TMP_InputField emailRegisterField
    {
        get
        {
            return UIManager.Instance.emailRegisterField;
        }
    }
    protected TMP_InputField passwordRegisterField
    {
        get
        {
            return UIManager.Instance.passwordRegisterField;
        }
    }
    protected TMP_InputField passwordRegisterVerifyField
    {
        get
        {
            return UIManager.Instance.passwordRegisterVerifyField;
        }
    }

    public static bool IsLoggedIn = false;
    #endregion


    private void Awake()
    {
        Instance = this;
        FirebaseApp.Create();
        print("Firebase app created");
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + dependencyStatus);
                UIManager.Instance.Notify("Connection Failed!", "There's a problem connecting to the server. Error code: " + dependencyStatus, UIManager.NotificationType.Error, true);
            }
        });
    }
    #region Firebase Methods
    private void InitializeFirebase()
    {
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }
    private IEnumerator LoggingIn(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        //yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        while (!LoginTask.IsCompleted)
        {
            UIManager.Instance.LoadingText = "Logging you in, Please wait...";
            yield return null;
        }
        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            // UIManager.Instance.HideLoading();
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            UIManager.Instance.LoadingText = "";
            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            //warningLoginText.text = message;
            UIManager.Instance.Notify("There was a problem signing you in!", message, UIManager.NotificationType.Error, true);
        }
        else
        {
            //User is now logged in
            //Now get the result
            FBUser = LoginTask.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})", FBUser.DisplayName, FBUser.Email);
            IsLoggedIn = true;
            OnLogin?.Invoke();

        }
    }

    private IEnumerator Registering(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            UIManager.Instance.Notify("Oops!", "Take another look on your data. Your username is missing.", UIManager.NotificationType.Error, true);
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            UIManager.Instance.Notify("Oops!", "Take another look on your data. Your passwords do not match.", UIManager.NotificationType.Error, true);
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            // yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            while (!RegisterTask.IsCompleted)
            {
                //UIManager.Instance.ShowLoading();
                yield return null;
            }
            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                //UIManager.Instance.HideLoading();
                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                UIManager.Instance.Notify("There was a problem registering your account.", "Take another look on your data. " + message, UIManager.NotificationType.Error, true);
            }
            else
            {
                //User has now been created
                //Now get the result
                FBUser = RegisterTask.Result;

                if (FBUser != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = FBUser.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        UIManager.Instance.Notify("There was a problem registering your account.", "We can't register your username. Choose another one or contact support.", UIManager.NotificationType.Error, true);
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.Instance.Notify("Thank you for registering!", "We hope you'll have a good time playing xCorgi! You can always send feedbacks to the developers.", UIManager.NotificationType.Normal, true);

                    }
                }
            }
        }
    }

    /// <summary>
    /// Retrieves user data including his type and trophies he has.
    /// </summary>
    /// <returns></returns>

    /// <summary>
    /// Logs the user in based on the login email text field and password text field.
    /// </summary>
    public void Login()
    {
        StartCoroutine(LoggingIn(emailLoginField.text, passwordLoginField.text));
    }

    /// <summary>
    /// Register the user in based on the register email text field and password text field.
    /// </summary>
    public void Register()
    {
        StartCoroutine(Registering(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    /// <summary>
    /// Logs our current user out
    /// </summary>
    public void Logout()
    {
        auth.SignOut();
        UIManager.Instance.Notify("You logged out successfully!", " ", UIManager.NotificationType.Normal, true);
    }
    #endregion

    /// <summary>
    /// Resets all of our data
    /// </summary>
    private void Reset()
    {
        emailLoginField.text = "";
        emailRegisterField.text = "";
        passwordLoginField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        usernameRegisterField.text = "";
        user = null;
        IsLoggedIn = false;
    }
}
