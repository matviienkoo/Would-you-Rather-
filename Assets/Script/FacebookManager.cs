using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using TMPro;

public class FacebookManager : MonoBehaviour
{
    [Header("Интиализация")]
    public bool IsSDKReady;

    [Header("Система входа")]
    public bool AmILoggedIn;
    public bool AutoLoginEnabled;

    [Header("Разришение на передачу информации игрока")]
    public bool fbCanAccessProfile = true;
    public bool PERM_PUBLICPROFILE = false;

    [Header("Получена ли информация")]
    public bool _ReqName = true;

    [Header("Информация")]
    public string myFirstName = "";
    public TextMeshProUGUI FirstName;
    public string myLastName = "";
    public TextMeshProUGUI LastName;
    public string myFbId = "";
    public GameObject PlayerInformation;
    public GameObject LoginText;

    void Awake()
    {
        if (!FB.IsLoggedIn)
        {
            FB.Init(OnFBSDKInit, OnHideUnity, null);
        }
        else if (FB.IsLoggedIn)
        {
            AmILoggedIn = true;
        }
    }

    private void OnFBSDKInit()
    {
        if (FB.IsInitialized)
        {
            IsSDKReady = true;LogIn(false);
            Debug.Log("Ready!");
        }
        else
        {
            IsSDKReady = false;
            Debug.Log("Failed To Initialize");
        }
    }

    private void OnHideUnity(bool isGAMEReady)
    {

    }


    public void LogIn(bool IsClicked)
    {
        Debug.Log("Logging....");
        List<string> fbPerms = new List<string>();
        if (fbCanAccessProfile)
        {
            fbPerms.Add("public_profile");
        }
        if (IsClicked)
        {
            FB.LogInWithReadPermissions(fbPerms, OnLoggedIn);
        }
        else
        {
            if (AutoLoginEnabled)
            {
                FB.LogInWithReadPermissions(fbPerms, OnLoggedIn);
            }
        }
    }

    private void OnLoggedIn(ILoginResult _LoginResponse)
    {
        if (_LoginResponse.Error != null)
        {
            Debug.Log("Failed To LOG IN");
            Debug.Log(_LoginResponse.Error);
        }
        else
        {
            AmILoggedIn = FB.IsLoggedIn;
            if (AmILoggedIn)
            {
                Debug.Log("Logged In");
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                myFbId = aToken.UserId;

                RequestUserInfo();
            }
            else
            {
                Debug.Log("User Cancelled");
            }
        }
    }

    public void RequestUserInfo()
    {
        List<string> myPerms = new List<string>();
        if (_ReqName)
        {
            if (PERM_PUBLICPROFILE)
            {
                myPerms.Add("first_name");
                myPerms.Add("last_name");
            }
            else
            {
                Debug.Log("PUBLIC PROFILE : Permission Required");
            }
        }
        string myPermsStr = string.Join(",", myPerms.ToArray());
        Debug.Log(myPermsStr);
        FB.API("/me?fields=" + myPermsStr, HttpMethod.GET, OnUserInfoGrabbed);
    }

    private void OnUserInfoGrabbed(IResult _FbUserResp)
    {
        if (_FbUserResp.Error == null)
        {
            Debug.Log(_FbUserResp.RawResult);
            if (_ReqName)
            {
                myFirstName = _FbUserResp.ResultDictionary["first_name"].ToString();
                myLastName = _FbUserResp.ResultDictionary["last_name"].ToString();
                FirstName.text = myFirstName;
                LastName.text = myLastName;
                LoginText.SetActive(false);
                PlayerInformation.SetActive(true);
            }
        }
        else
        {
            Debug.Log(_FbUserResp.Error);
        }
    }
}