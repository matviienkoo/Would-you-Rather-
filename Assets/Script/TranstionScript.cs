using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TranstionScript : MonoBehaviour 
{
    private float Timer;

    [Header("Настройка перехода")]
    public GameObject Transtion_Panel;
    public Animation Transtion_Animation;

    public string String_Transtion;
    public bool Bool_Transtion;
    public bool Bool_LoginSelection;

    [Header("Панели для перехода")]
    public GameObject StartScene;
    public GameObject QuestionScene;
    public GameObject CreateQuestionScene;
    public GameObject SelectImgScene;
    public GameObject LoadingScene;

    [Header("Скрипты")]
    public QuestionScript QuestionSystem;

    // - Переход на стартовую сцену
    public void OpenStartScene ()
    {   
        Transtion_Panel.SetActive(true);
        Transtion_Animation.Play();

        String_Transtion = "Start Scene";
        Bool_Transtion = true;
    }

    // - Переход на сцену вопросов
    public void OpenQuestionScene ()
    {   
        Transtion_Panel.SetActive(true);
        Transtion_Animation.Play();

        String_Transtion = "Question Scene";
        Bool_Transtion = true;
        Bool_LoginSelection = true;
    }

    // - Переход на сцену загрузки
    public void OpenLoadingScene ()
    {   
        Transtion_Panel.SetActive(true);
        Transtion_Animation.Play();

        String_Transtion = "Loading Scene";
        Bool_Transtion = true;
    }

    // - Переход на сцену создания вопроса
    public void OpenCreateQuestionScene ()
    {   
        Transtion_Panel.SetActive(true);
        Transtion_Animation.Play();

        String_Transtion = "Create question Scene";
        Bool_Transtion = true;
    }

    // - Переход на сцену выбора картинки для создания вопроса
    public void OpenSelectionImgScene ()
    {   
        Transtion_Panel.SetActive(true);
        Transtion_Animation.Play();

        String_Transtion = "Selection img Scene";
        Bool_Transtion = true;
    }

    private void DisabledScene ()
    {
        StartScene.SetActive(false);
        QuestionScene.SetActive(false);
        CreateQuestionScene.SetActive(false);
        SelectImgScene.SetActive(false);
        LoadingScene.SetActive(false);
    }

    private void Update ()
    {
        if (Bool_Transtion == true){
        Timer += Time.deltaTime;
        if (Timer >= 0.4f)
        {
            if (String_Transtion == "Start Scene")
            {   
                DisabledScene();
                StartScene.SetActive(true);
            }

            if (String_Transtion == "Question Scene")
            {   
                DisabledScene();
                QuestionScene.SetActive(true);
            }

            if (String_Transtion == "Loading Scene")
            {
                DisabledScene();
                LoadingScene.SetActive(true);
            }

            if (String_Transtion == "Create question Scene")
            {   
                DisabledScene();
                CreateQuestionScene.SetActive(true);
            }

            if (String_Transtion == "Selection img Scene")
            {   
                DisabledScene();
                CreateQuestionScene.SetActive(true);
                SelectImgScene.SetActive(true);
            }
        }

        if (Timer >= 0.8f)
        {
            Bool_Transtion = false;
            Bool_LoginSelection = false;
            Transtion_Panel.SetActive(false);
            Timer = 0;
        }
        }

        // Loading System
        if (String_Transtion == "Loading Scene")
        {
            bool ContentUnloaded = (PlayerPrefs.GetInt("ContentUnloaded") != 0);
            bool ContentReload = (PlayerPrefs.GetInt("ContentReload") != 0);

            // (loading the question panel) if the Internet does not work
            if(Application.internetReachability == NetworkReachability.NotReachable && QuestionSystem.RemoveDebug == false)
            {
                String_Transtion = "";
                OpenQuestionScene();
            }
            // (loading the question panel) when loading questions for the first time
            if (ContentUnloaded == true && QuestionSystem.RemoveDebug == false)
            {
                String_Transtion = "";
                OpenQuestionScene();
            }
            // (loading the question panel) when reloading questions
            if (ContentReload == true && QuestionSystem.RemoveDebug == false)
            {
                String_Transtion = "";
                OpenQuestionScene();
            }
        }
    }

}
