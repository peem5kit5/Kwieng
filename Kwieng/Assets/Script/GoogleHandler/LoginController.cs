using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginController : MonoBehaviour
{
    public void GoogleLogin() => PlayGamesPlatform.Instance.Authenticate(Processing);
    internal void Processing(SignInStatus _status)
    {
        if (_status == SignInStatus.Success)
        {

        }
        else
        {
            GuestLogin();
            return;
        }  
    }

    public void GuestLogin()
    {

    }
}
