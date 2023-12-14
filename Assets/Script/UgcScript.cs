using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Ugc;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.IO;
using System;
using TMPro;
using System.Linq;

public class UgcScript : MonoBehaviour 
{   
    private string QuestionContent;
    private string[] SplitText;
    private int LoadQuestion;

    [Header("Текстовая информация загружаемая с сервера")]
    public string SplitText_FirstQuestion;
    public string SplitImg_FirstQuestion;

    public string SplitText_SecondQuestion;
    public string SplitImg_SecondQuestion;

    [Header("Текстовая информация для выгрузки на сервер")]
    public TextMeshProUGUI FirstQuestionText;
    public TextMeshProUGUI SecondQuestionText;

    [Header("Авторизация")]
    public bool BoolAuthorization;

    [Header("Ошибка авторизации")]
    public bool BoolError;

    [Header("Вопросы загрузились")]
    public bool ContentUnloaded;

    [Header("Обновления вопросов")]
    public bool ContentReload;

    [Header("Заблокированные Id")]
    public List<string> contentid_BLOCKED = new List<string>();
    private bool bool_sorting;

    [Header("Запрашиваемые Id")]
    public List<string> contentid_FIND = new List<string>();

    [Header("Id для скачивания контента")]
    public string contentid_DOWNLOAD;

    [Header("Id для удаления контента")]
    public string contentid_DELETE;
    public TextMeshProUGUI textid_DELETE; 

    private void Start ()
    {
        for (int i = 0; PlayerPrefs.HasKey("id_BLOCKED" + i); i++)
        {
            contentid_BLOCKED.Add(PlayerPrefs.GetString("id_BLOCKED" + i));
            contentid_BLOCKED = contentid_BLOCKED.Distinct().ToList(); // Remove duplicates
        }
    }
    private void Awake()
    {
        InitAndSignIn();
    }
    public void InitSystem()
    {
        InitAndSignIn();
    }
    private async void InitAndSignIn()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            BoolAuthorization = true; GetComponent<QuestionScript>().AuthorizationSystem();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);GetComponent<QuestionScript>().AuthorizationSystem(); BoolError = true;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);GetComponent<QuestionScript>().AuthorizationSystem();
        }
    }

    [Tooltip("Create question in the text document, and load into server")]
    public void CreateTextDocument ()
    {
        //Create and File if it doesn't exist
        string path = Application.dataPath + "myQuestion.txt";

        if (!File.Exists(path)){
            File.WriteAllText(path, "");
        }
        File.WriteAllText(path, "");

        //Content of the file
        string FirstQuestion_Text = FirstQuestionText.text.ToString();
        string FirstQuestion_Img = GetComponent<ImgSystemScript>().String_FirstQuestion;

        string SecondQuestion_Text = SecondQuestionText.text.ToString();
        string SecondQuestion_Img = GetComponent<ImgSystemScript>().String_SecondQuestion;

        QuestionContent = FirstQuestion_Text + "|" + FirstQuestion_Img + "|" + SecondQuestion_Text + "|" + SecondQuestion_Img;

        //Add some to text to it
        File.AppendAllText(path, QuestionContent);

        // Load content into server
        LoadContent();
    }
    private async void LoadContent()
    {
        using FileStream contentFileStream = 
        File.Open(Application.dataPath + "myQuestion.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

        try
        {
            Content content = 
            await UgcService.Instance.CreateContentAsync(new CreateContentArgs("(Вопрос)", QuestionContent, contentFileStream) {
            IsPublic = true,  
            });

            Debug.Log("Created Content " + content.Id);
        }
        catch (UgcException e){
        Debug.Log(e);
        }
    }

    [Tooltip("Find question in the server, and download")]
    public async void FindContents()
    {
        try 
        {
            PagedResults<Content> contentPagedResults = await UgcService.Instance.GetContentsAsync();
            foreach (Content content in contentPagedResults.Results)
            {
                Debug.Log(content.Id);

                // Sorting find ids
                contentid_FIND.Add(content.Id);
                for(int x = 0;x < contentid_BLOCKED.Count;x++){
                contentid_FIND.Remove(contentid_BLOCKED[x]);
                }

                // Remove duplicates
                contentid_FIND = contentid_FIND.Distinct().ToList();

                // Enable sorting system
                if (contentid_FIND.Count > 0){
                StartCoroutine(SortingContents());
                Debug.Log("AAAA");
                }

                // Enable refresh system
                if (contentid_FIND.Count == 0 && ContentReload == true){
                GetComponent<QuestionScript>().ReloadSystem_NoMoreQuestions();
                Debug.Log("asdfasf");
                }
            }
        }
        catch (UgcException e){
        Debug.Log(e);
        }
    }
    IEnumerator SortingContents ()
    {
        yield return new WaitForSeconds(0.05f);
        contentid_DOWNLOAD = contentid_FIND.Last();

        //Looks at available id
        for(int x = 0;x < contentid_BLOCKED.Count;x++){
        if (contentid_BLOCKED[x] != contentid_DOWNLOAD && bool_sorting == false)
        {
            bool_sorting = true;

            // Id download
            DownloadContent();

            // Id add in the blocked list
            contentid_BLOCKED.Add(contentid_DOWNLOAD);

            // Id remove in the finds list
            contentid_FIND.Remove(contentid_DOWNLOAD);
        }
        }
    }

    public async void DownloadContent()
    {
        // Download information into server
        try
        {
            Content content = await UgcService.Instance.GetContentAsync(new GetContentArgs(contentid_DOWNLOAD));
            await UgcService.Instance.DownloadContentDataAsync(content, true, false);

            string modText = System.Text.Encoding.UTF8.GetString(content.DownloadedContent);

            // Split getting strings 
            SplitText = modText.Split('|');
            for (int i = 0; i < SplitText.Length; i++)
            {
                SplitText_FirstQuestion = SplitText[0];
                SplitImg_FirstQuestion = SplitText[1];
                SplitText_SecondQuestion = SplitText[2];
                SplitImg_SecondQuestion = SplitText[3];
            }
            PlayerPrefs.SetString("UpperTextQuestion" + LoadQuestion, SplitText_FirstQuestion);
            PlayerPrefs.SetString("UpperImgQuestion" + LoadQuestion, SplitImg_FirstQuestion);
            PlayerPrefs.SetString("LowerTextQuestion" + LoadQuestion, SplitText_SecondQuestion);
            PlayerPrefs.SetString("LowerImgQuestion" + LoadQuestion, SplitImg_SecondQuestion);
            LoadQuestion++;PlayerPrefs.SetInt("LoadQuestion", LoadQuestion);

            // Getting string converted into questions (And enable sorting bool)
            GetComponent<QuestionScript>().AddPlayerQuestion(); bool_sorting = false;
            
            // Save blocked id's
            for (int i = 0;i < contentid_BLOCKED.Count;i++){
                PlayerPrefs.SetString("id_BLOCKED" + i, contentid_BLOCKED[i].ToString());
            }

            // Add new questions, if they are on the find list
            if (contentid_FIND.Count > 0)
            {
                StartCoroutine(SortingContents());
            }
            else
            {
                ContentUnloaded = true;
                PlayerPrefs.SetInt("ContentUnloaded", (ContentUnloaded ? 1 : 0));

                // Disabled special (reload mode)
                if (ContentReload == true)
                {
                    ContentReload = false;
                    PlayerPrefs.SetInt("ContentReload", (ContentReload ? 1 : 0));

                    // Update questions
                    GetComponent<QuestionScript>().SuccesfulReload();
                }
            }
            
        }
        catch (UgcException e){
        Debug.Log(e);
        }
    }

    [Tooltip("Delete Question")]
    public async void DeleteContentAsync()
    {
        contentid_DELETE = textid_DELETE.text;
        await UgcService.Instance.DeleteContentAsync(contentid_DELETE);
        Debug.Log($"Deleted content with id: {contentid_DELETE}.");
    }

    [Tooltip("Internet System")]
    private void Update ()
    {
        ContentUnloaded = (PlayerPrefs.GetInt("ContentUnloaded") != 0);
        ContentReload = (PlayerPrefs.GetInt("ContentReload") != 0);
    }
}
