using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.IO;
using System;
using TMPro;
using System.Collections;

public class FacebookScript : MonoBehaviour 
{
    [Header("Данные игрока")]
    public TextMeshProUGUI FirstName;
    public TextMeshProUGUI LastName;
    public GameObject PlayerInformation;
    public GameObject LoginText;

    [Header("Токен пользователя")]
    public string myFbId;

    private void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Logged is Successfuly!!");
        }
        else
        Debug.Log("FB is not loggid in");
    }
    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    public class DeviceLoginStatus_Response 
    {
        public string access_token;
        public int data_access_expiration_time;
        public int expires_in;
    }
    void OnHideUnity(bool isGameShown)
    {

    }

    private async void UnityAuthWithFacebook(string token)
    {
        await UnityServices.InitializeAsync();

        try 
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);
            Debug.Log("SignIn with Facebook successful");

            Debug.Log("PlayerId " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    // --- Авторизация через фейсбука
    public void FBLogin()
    {
        List<string> permession = new List<string>();
        permession.Add("public_profile");
        FB.LogInWithReadPermissions(permession, AuthCallResult);
    }

    void AuthCallResult(ILoginResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FB logged in");
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                myFbId = aToken.UserId;

                FB.API("/me?fields=first_name", HttpMethod.GET, callbackData);
            }
            else
            {
                Debug.Log("Login Failed!");
            }
        }
    }

    void callbackData(IResult res)
    {
        if (res.Error != null)
        {
            Debug.Log("Error getting data from FACEBOOK!");
        }
        else
        {
            LoginText.SetActive(false);
            PlayerInformation.SetActive(true);
            FirstName.text = res.ResultDictionary["first_name"].ToString();
            LastName.text = res.ResultDictionary["last_name"].ToString();
        }
    }

    // --- Запросить друзей которые играли в эту игру
    public void GetFriendsPlayingThisGame(string FriendsString)
    {
        FB.API("/me?fields=" + FriendsString, HttpMethod.GET);
    }
}
