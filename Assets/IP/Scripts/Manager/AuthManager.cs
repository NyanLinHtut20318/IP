using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.UIElements;
using TMPro;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    //Pages
    Firebase.Auth.FirebaseAuth authInstance;
    public GameObject AuthPage;
    public GameObject MainMenu;

    //Login/Signup inputs
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;

    //Debug display
    public GameObject debugTextBox;
    public GameObject otherText;

    //Managers
    public GameManager gameManager;
    public FirebaseManager fbManager;

    public bool enterMainMenu = false;
    public GameObject Game;

    private void Awake()
    {
        authInstance = FirebaseAuth.DefaultInstance;
    }

    public void SignUpBtn()
    {
        if (ValidateEmail(emailInput.text.Trim()) && ValidatePassword(passwordInput.text.Trim()) && (usernameInput.text.Length > 0))
        {
            otherText.SetActive(true);
            debugTextBox.SetActive(false);
            SignUpNewUser(emailInput.text.Trim(), passwordInput.text.Trim(), usernameInput.text.Trim());
        }
        else
        {
            otherText.SetActive(false);
            debugTextBox.SetActive(true);
            debugTextBox.GetComponent<TMP_Text>().text = "Please check that your Email / Password / Username inputs are Properly Filled";
        }
    }

    public void LoginBtn()
    {
        if (ValidateEmail(emailInput.text.Trim()) && ValidatePassword(passwordInput.text.Trim()) && (usernameInput.text.Length > 0))
        {
            otherText.SetActive(true);
            debugTextBox.SetActive(false);
            LoginUser(emailInput.text.Trim(), passwordInput.text.Trim(), usernameInput.text.Trim());
        }
        else
        {
            otherText.SetActive(false);
            debugTextBox.SetActive(true);
            debugTextBox.GetComponent<TMP_Text>().text = "Please check that your Email / Password / Username inputs are Properly Filled";
        }
    }

    //create user
    public void SignUpNewUser(string email, string password, string username)
    {
        authInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                //display error message in console
                Debug.LogError("There was an error in creating a new account " + task.Exception);
                return;
            }

            else if(task.IsCompleted)
            {
                FirebaseUser newUser = task.Result;

                //create new User Log
                fbManager.AddToPlayerList(newUser.UserId, email, password, username);
                fbManager.CreateNewPlayer(newUser.UserId);
                Debug.Log("account created successfully...entering game");

                //set user id instance
                var currentUser = newUser.UserId;
                //gameManager.userID = currentUser;
                //gameManager.username = username;

                //enter game
                enterMainMenu = true;
            }
        });
    }

    public void LoginUser(string email, string password, string usernameInput)
    {
        authInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                //display error message in console
                Debug.LogError("There was an error in signing into the account");
                return;
            }

            else if (task.IsCompleted)
            {
                FirebaseUser existingUser = task.Result;
                Debug.Log("user logged in successfully...entering game");

                //set user id instance
                var currentUser = existingUser.UserId;
                //gameManager.userID = currentUser;
                //gameManager.username = usernameInput;

                //enter game
                enterMainMenu = true;
            }
        });
    }

    public void SignOutUser()
    {
        if (authInstance.CurrentUser != null)
        {
            authInstance.SignOut();
            //gameManager.userID = null;
        }
        Debug.Log("Signing user out of the game...");
        enterMainMenu = false;
    }

    private void Update()
    {
        //Auth on, main menu off
        if (!enterMainMenu)
        {
            MainMenu.SetActive(false);
            AuthPage.SetActive(true);
        }

        //auth off, main menu on
        if (enterMainMenu)
        {
            //enter main menu
            MainMenu.SetActive(true);
            AuthPage.SetActive(false);
        }

        //auth + main menu off, game on
        //if (enterMainMenu && gameManager.GameStart)
        //{
        //    MainMenu.SetActive(false);
        //    AuthPage.SetActive(false);
        //    Game.SetActive(true);
        //}
    }


    //VALIDATION//
    public bool ValidateEmail(string email)
    {
        bool isValid = false;

        const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

        if (email != "" && Regex.IsMatch(email, pattern, options))
        {
            isValid = true;
        }
        return isValid;
    }

    public bool ValidatePassword(string password)
    {
        bool isValid = false;
        if (password != "" && password.Length >= 2)
        {
            isValid = true;
        }
        return isValid;
    }
}