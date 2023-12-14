using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestionScript : MonoBehaviour 
{
    public List<SettingQuestion> GameQuestions = new List<SettingQuestion>();

    [System.Serializable]
    public class SettingQuestion
    {
        public string UpperQuestion;
        public Sprite UpperImage;
        public float UpperProcent = 50;
        public string LowerQuestion;
        public Sprite LowerImage;
        public float LowerProcent = 50;
    }

    private bool ContentUnloaded;
    private bool ContentReload;
    private int LoadQuestion;
    private int ChoiseInt;

    [Header("Количество удаленных вопросов")]
    public int RemoveQuestion;
    public bool RemoveDebug;

    [Header("Система предупреждений")]
    public GameObject Warning_Panel;
    public Animation Warning_Uploading_Animation;
    public Animation Warning_NoMoreQustions_Animation;
    public Animation Warning_BadInternet_Animation;

    [Header("Система обновления вопросов")]   
    public GameObject Refresh_Panel;
    public Animation RefreshWarning_Animation;
    public Animation RefreshRotate_Animation; 
    public Animation RefreshButton_Animation; 
    public Button Refresh_Button;
    
    [Header("Первый вопрос")]
    public Image UpperImage;
    public Button UpperButton;
    public GameObject UpperSelection;
    public TextMeshProUGUI UpperQuestion;
    private float UpperProcent;
    public TextMeshProUGUI UpperProcent_Text;

    [Header("Анимации для первого вопроса")]
    public Animation UpperChange_Animation;
    public Animation UpperLight_Animation;
    public Animation UpperText_Animation;

    [Header("Второй вопрос")]
    public Image LowerImage;
    public Button LowerButton;
    public GameObject LowerSelection;
    public TextMeshProUGUI LowerQuestion;
    private float LowerProcent;
    public TextMeshProUGUI LowerProcent_Text;

    [Header("Анимации для второго вопроса")]
    public Animation LowerChange_Animation;
    public Animation LowerLight_Animation;
    public Animation LowerText_Animation;

    [Header("Скрипты")]
    public TranstionScript TranstionSystem;

    [Tooltip("Player authorization system")]
    public void AuthorizationSystem ()
    {   
        // First download of questions from the server
        if (ContentUnloaded == false && GetComponent<UgcScript>().BoolAuthorization == true)
        {
            GetComponent<UgcScript>().FindContents();
        }

        // Unload questions from the save system || None internet
        if (ContentUnloaded == true || ContentReload == true || Application.internetReachability == NetworkReachability.NotReachable)
        {
            // Download all questions from the playerprefs
            LoadPlayerQuestion();

            // Delete all completed questions
            StartCoroutine(IEnumeratorRemove());RemoveDebug = true;
        }

        // Update question
        if (RemoveDebug == false){
        if (GameQuestions.Count > 0 && ContentReload == false)
        {
            UpdateQuestion();
        }
        else
        {
            ReloadQuestion();
        }
        }
    }

    IEnumerator IEnumeratorRemove()
    {
        yield return new WaitForSeconds(1.5f);

        int IntRemove = RemoveQuestion;
        if (GameQuestions.Count > 0){
        for (int i = 0;IntRemove > i;IntRemove--)
        {
            GameQuestions.RemoveAt(0);
        }
        }

        if (GameQuestions.Count > 0)
        {
            UpdateQuestion();
            RemoveDebug = false;
        }
        else
        {
            ReloadQuestion();
            RemoveDebug = false;
        }
    }

    [Tooltip("Question processing system")]
    public void UpdateQuestion ()
    {
        UpperImage.sprite = GameQuestions[0].UpperImage;
        UpperQuestion.text = GameQuestions[0].UpperQuestion;
        UpperProcent = GameQuestions[0].UpperProcent;

        LowerImage.sprite = GameQuestions[0].LowerImage;
        LowerQuestion.text = GameQuestions[0].LowerQuestion;
        LowerProcent = GameQuestions[0].LowerProcent;
    }

    public void AddPlayerQuestion ()
    {
        SettingQuestion PlayerSystem = new SettingQuestion();
        GameQuestions.Add(PlayerSystem);

        // Upper Question
        PlayerSystem.UpperQuestion = GetComponent<UgcScript>().SplitText_FirstQuestion; // Text
        GetComponent<ImgSystemScript>().FindImage_UpperQuestion();PlayerSystem.UpperImage = GetComponent<ImgSystemScript>().Sprite_UpperGallery; // Image
        PlayerSystem.UpperProcent = Random.Range(12, 89); // Procent

        // Lower Question
        PlayerSystem.LowerQuestion = GetComponent<UgcScript>().SplitText_SecondQuestion;
        GetComponent<ImgSystemScript>().FindImage_LowerQuestion();PlayerSystem.LowerImage = GetComponent<ImgSystemScript>().Sprite_LowerGallery;
        float Float_Numbers = 100;Float_Numbers -= PlayerSystem.UpperProcent;PlayerSystem.LowerProcent = Float_Numbers;
    }

    public void LoadPlayerQuestion ()
    {
        LoadQuestion = PlayerPrefs.GetInt("LoadQuestion");

        for (int IntLoad = 0; IntLoad < LoadQuestion; IntLoad++)
        {
            SettingQuestion PlayerSystem = new SettingQuestion();
            GameQuestions.Add(PlayerSystem);

            // Upper Question
            PlayerSystem.UpperQuestion = PlayerPrefs.GetString("UpperTextQuestion" + IntLoad); // Text
            GetComponent<UgcScript>().SplitImg_FirstQuestion = PlayerPrefs.GetString("UpperImgQuestion" + IntLoad); // Image
            GetComponent<ImgSystemScript>().FindImage_UpperQuestion();PlayerSystem.UpperImage = GetComponent<ImgSystemScript>().Sprite_UpperGallery;
            PlayerSystem.UpperProcent = Random.Range(12, 89); // Procent

            // Lower Question
            PlayerSystem.LowerQuestion = PlayerPrefs.GetString("LowerTextQuestion" + IntLoad);
            GetComponent<UgcScript>().SplitImg_SecondQuestion = PlayerPrefs.GetString("LowerImgQuestion" + IntLoad);
            GetComponent<ImgSystemScript>().FindImage_LowerQuestion();PlayerSystem.LowerImage = GetComponent<ImgSystemScript>().Sprite_LowerGallery;
            float Float_Numbers = 100;Float_Numbers -= PlayerSystem.UpperProcent;PlayerSystem.LowerProcent = Float_Numbers;
        }
    }

    [Tooltip("Question review system")]
    public void ButtonSelection (int IntSelect)
    {
        if (ChoiseInt == 2){
            LowerLight_Animation.Play("LightBlue (Close)");
            UpperButton.enabled = false;
            LowerButton.enabled = false;
            StartCoroutine(WaitAfterNewQuestion());
        }
        if (ChoiseInt == 1){
            UpperLight_Animation.Play("LightRed (Close)");
            UpperButton.enabled = false;
            LowerButton.enabled = false;
            StartCoroutine(WaitAfterNewQuestion());
        }

        if (ChoiseInt == 0)
        {
            // Panel selection (animation) -- Red
            if (IntSelect == 0) 
            {
                UpperLight_Animation.Play("LightRed (Open)");
                ChoiseInt = 1;
            }

            // Panel selection (animation) -- Blue
            if (IntSelect == 1) 
            {
                LowerLight_Animation.Play("LightBlue (Open)");
                ChoiseInt = 2;
            }

            // Show procent (animation)
            UpperText_Animation.Play("PushText_Red (Open)");
            LowerText_Animation.Play("PushText_Blue (Open)");
        }
    }

    IEnumerator WaitAfterNewQuestion()
    {
        yield return new WaitForSeconds(0.2f);
        if (GameQuestions.Count > 0)
        {
            // Remove questions
            RemoveQuestion ++; GameQuestions.RemoveAt(0);
            PlayerPrefs.SetInt("RemoveQuestion", RemoveQuestion);
        }

        if (GameQuestions.Count > 0) 
        {
            // Change question (animation)
            UpperChange_Animation.Play("UpperQuestionChange");
            LowerChange_Animation.Play("LowerQuestionChange");
        }
        else
        {
            // Hide selections, request to the server for questions
            UpperChange_Animation.Play("UpperQuestionChange_Hide");
            LowerChange_Animation.Play("LowerQuestionChange_Hide");
        }        

        yield return new WaitForSeconds(0.2f);
        if (GameQuestions.Count > 0) 
        {
            UpdateQuestion();
        }
        else
        {
            ReloadQuestion();
        }

        // Defualt text (animation)
        UpperText_Animation.Play("PushText_Red (Close)");
        LowerText_Animation.Play("PushText_Blue (Close)");

        yield return new WaitForSeconds(1f);
        UpperButton.enabled = true;LowerButton.enabled = true;ChoiseInt = 0;
    }

    [Tooltip("Reload system")]
    public void ReloadQuestion ()
    {
        // No more questions in the game
        ContentUnloaded = false;
        PlayerPrefs.SetInt("ContentUnloaded", (ContentUnloaded ? 1 : 0));

        // Enabled special (reload mode)
        ContentReload = true;
        PlayerPrefs.SetInt("ContentReload", (ContentReload ? 1 : 0));

        // Request to the server (If the player has internet)
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if(GetComponent<UgcScript>().BoolAuthorization == false && GetComponent<UgcScript>().BoolError == false) 
            {
                GetComponent<UgcScript>().InitSystem();
            }
            else
            {
                GetComponent<UgcScript>().FindContents();
            }
        }
        else
        {
            ReloadSystem_BadInternet();
        }
    }

    // Warning (Bad internet)
    public void ReloadSystem_BadInternet(){
        if (TranstionSystem.String_Transtion == "Question Scene" && Refresh_Button.interactable == true)
        {
            Warning_BadInternet_Animation.Play();
            RefreshRotate_Animation.Play();
        }

        StartCoroutine(IEnumerator_ReloadSystem());
        Refresh_Button.interactable = false;
    }

    // Warning (No more questions)
    public void ReloadSystem_NoMoreQuestions(){
        if (TranstionSystem.String_Transtion == "Question Scene" && Refresh_Button.interactable == true)
        {
            Warning_NoMoreQustions_Animation.Play();
            RefreshRotate_Animation.Play();
        }

        StartCoroutine(IEnumerator_ReloadSystem());
        Refresh_Button.interactable = false;
    }
    
    // Warning (Questions unload)
    public void SuccesfulReload(){
        Warning_Uploading_Animation.Play();

        StartCoroutine(IEnumerator_SuccesfulReload());
    }

    // Reload system
    IEnumerator IEnumerator_ReloadSystem()
    {
        yield return new WaitForSeconds(0.15f);
        UpperSelection.SetActive(false);
        LowerSelection.SetActive(false);
        Refresh_Panel.SetActive(true);

        yield return new WaitForSeconds(3.5f);
        Refresh_Button.interactable = true; Debug.Log("penis");
    }
    IEnumerator IEnumerator_SuccesfulReload()
    {
        yield return new WaitForSeconds(0.1f); // Update question, refresh system disable
        UpdateQuestion();

        RefreshWarning_Animation.Play("WarningRefreshClosed");
        RefreshButton_Animation.Play("ClosedRefresh");

        yield return new WaitForSeconds(1f); // Selections come back 
        UpperChange_Animation.Play("UpperQuestionChange_Return");
        LowerChange_Animation.Play("LowerQuestionChange_Return");
        UpperSelection.SetActive(true);
        LowerSelection.SetActive(true);
        Refresh_Panel.SetActive(false);
    }

    private void Update ()
    {
        ContentReload = (PlayerPrefs.GetInt("ContentReload") != 0);
        ContentUnloaded = (PlayerPrefs.GetInt("ContentUnloaded") != 0);
        RemoveQuestion = PlayerPrefs.GetInt("RemoveQuestion");
        UpperProcent_Text.text = UpperProcent.ToString() + "%";
        LowerProcent_Text.text = LowerProcent.ToString() + "%";
    }
}
