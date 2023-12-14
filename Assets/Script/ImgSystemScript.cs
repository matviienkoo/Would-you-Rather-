using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImgSystemScript : MonoBehaviour 
{
    public TextMeshProUGUI Img_Text;

    [Header("Первая картинка")]
    public Image Img_FirstQuestion;
    public Sprite Sprite_UpperGallery;
    public string String_FirstQuestion;
    public bool Bool_FirstQustion;

    [Header("Вторая картинка")]
    public Image Img_SecondQuestion;
    public Sprite Sprite_LowerGallery;
    public string String_SecondQuestion;
    public bool Bool_SecondQustion;

    [Header("Галерея")]
    public List<GalleryScript> GalleryGameobject = new List<GalleryScript>();

    [System.Serializable]
    public class GalleryScript
    {
        public GameObject GameObject_Gallery;
        public Button Button_Gallery;
    }

    [Header("Картинки людей")]
    public List<PeopleScript> PeopleImg = new List<PeopleScript>();

    [System.Serializable]
    public class PeopleScript
    {
        public Sprite Sprite_People;
        public Button Button_People;
        public string String_People;
    }

    [Header("Картинки животных")]
    public List<AnimalsScript> AnimalsImg = new List<AnimalsScript>();

    [System.Serializable]
    public class AnimalsScript
    {
        public Sprite Sprite_Animal;
        public Button Button_Animal;
        public string String_Animal;
    }

    [Header("Картинки вещей")]
    public List<ThingScript> ThingImg = new List<ThingScript>();

    [System.Serializable]
    public class ThingScript
    {
        public Sprite Sprite_Thing;
        public Button Button_Thing;
        public string String_Thing;
    }

    [Header("Картинки продуктов")]
    public List<FoodScript> FoodImg = new List<FoodScript>();

    [System.Serializable]
    public class FoodScript
    {
        public Sprite Sprite_Food;
        public Button Button_Food;
        public string String_Food;
    }

    [Header("Картинки ивентов")]
    public List<EventScript> EventImg = new List<EventScript>();

    [System.Serializable]
    public class EventScript
    {
        public Sprite Sprite_Event;
        public Button Button_Event;
        public string String_Event;
    }

    [Header("Картинки другого")]
    public List<OtherScript> OtherImg = new List<OtherScript>();

    [System.Serializable]
    public class OtherScript
    {
        public Sprite Sprite_Other;
        public Button Button_Other;
        public string String_Other;
    }

    [Tooltip("Create-Quesiton (Select first image)")]
    public void Button_Img_FirstQuestion()
    {
        Bool_FirstQustion = true;
        Img_Text.text = "Images for the first question";

        Bool_SecondQustion = false;
        DisabledSelection();
    }

    [Tooltip("Create-Quesiton (Select second image)")]
    public void Button_Img_SecondQuestion()
    {
        Bool_SecondQustion = true;
        Img_Text.text = "Images for the second question";

        Bool_FirstQustion = false;
        DisabledSelection();
    }

    [Tooltip("Gallery")]
    public void Button_Img_Gallery (int IntGallery)
    {
        for(int x = 0;x < GalleryGameobject.Count;x++){
        GalleryGameobject[x].GameObject_Gallery.SetActive(false);
        GalleryGameobject[x].Button_Gallery.interactable = true;
        }

        GalleryGameobject[IntGallery].GameObject_Gallery.SetActive(true);
        GalleryGameobject[IntGallery].Button_Gallery.interactable = false;
    }

    public void Button_People_Select (int IntImg)
    {
        DisabledSelection();
        PeopleImg[IntImg].Button_People.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = PeopleImg[IntImg].Sprite_People;
            String_FirstQuestion = PeopleImg[IntImg].String_People;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = PeopleImg[IntImg].Sprite_People;
            String_SecondQuestion = PeopleImg[IntImg].String_People;
        }
    } 

    public void Button_Animals_Select (int IntImg)
    {
        DisabledSelection();
        AnimalsImg[IntImg].Button_Animal.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = AnimalsImg[IntImg].Sprite_Animal;
            String_FirstQuestion = AnimalsImg[IntImg].String_Animal;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = AnimalsImg[IntImg].Sprite_Animal;
            String_SecondQuestion = AnimalsImg[IntImg].String_Animal;
        }
    }

    public void Button_Thing_Select (int IntImg)
    {
        DisabledSelection();
        ThingImg[IntImg].Button_Thing.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = ThingImg[IntImg].Sprite_Thing;
            String_FirstQuestion = ThingImg[IntImg].String_Thing;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = ThingImg[IntImg].Sprite_Thing;
            String_SecondQuestion = ThingImg[IntImg].String_Thing;
        }
    }

    public void Button_Food_Select (int IntImg)
    {
        DisabledSelection();
        FoodImg[IntImg].Button_Food.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = FoodImg[IntImg].Sprite_Food;
            String_FirstQuestion = FoodImg[IntImg].String_Food;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = FoodImg[IntImg].Sprite_Food;
            String_SecondQuestion = FoodImg[IntImg].String_Food;
        }
    }

    public void Button_Event_Select (int IntImg)
    {
        DisabledSelection();
        EventImg[IntImg].Button_Event.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = EventImg[IntImg].Sprite_Event;
            String_FirstQuestion = EventImg[IntImg].String_Event;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = EventImg[IntImg].Sprite_Event;
            String_SecondQuestion = EventImg[IntImg].String_Event;
        }
    }

    public void Button_Other_Select (int IntImg)
    {
        DisabledSelection();
        OtherImg[IntImg].Button_Other.interactable = false;

        if (Bool_FirstQustion == true){
            Img_FirstQuestion.sprite = OtherImg[IntImg].Sprite_Other;
            String_FirstQuestion = OtherImg[IntImg].String_Other;
        }
        if (Bool_SecondQustion == true){
            Img_SecondQuestion.sprite = OtherImg[IntImg].Sprite_Other;
            String_SecondQuestion = OtherImg[IntImg].String_Other;
        }
    }

    public void DisabledSelection ()
    {
        for(int x = 0;x < PeopleImg.Count;x++){
        PeopleImg[x].Button_People.interactable = true;
        }

        for(int y = 0;y < AnimalsImg.Count;y++){
        AnimalsImg[y].Button_Animal.interactable = true;
        }

        for(int c = 0;c < ThingImg.Count;c++){
        ThingImg[c].Button_Thing.interactable = true;
        }

        for(int g = 0;g < FoodImg.Count;g++){
        FoodImg[g].Button_Food.interactable = true;
        }

        for(int h = 0;h < EventImg.Count;h++){
        EventImg[h].Button_Event.interactable = true;
        }

        for(int j = 0;j < OtherImg.Count;j++){
        OtherImg[j].Button_Other.interactable = true;
        }
    }

    [Tooltip("Find Image from gallery")]
    public void FindImage_UpperQuestion ()
    {
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("People")){
            for(int x = 0;x < PeopleImg.Count;x++){
            if (PeopleImg[x].String_People == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = PeopleImg[x].Sprite_People;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("Animal")){
            for(int x = 0;x < AnimalsImg.Count;x++){
            if (AnimalsImg[x].String_Animal == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = AnimalsImg[x].Sprite_Animal;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("Thing")){
            for(int x = 0;x < ThingImg.Count;x++){
            if (ThingImg[x].String_Thing == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = ThingImg[x].Sprite_Thing;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("Food")){
            for(int x = 0;x < FoodImg.Count;x++){
            if (FoodImg[x].String_Food == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = FoodImg[x].Sprite_Food;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("Event")){
            for(int x = 0;x < EventImg.Count;x++){
            if (EventImg[x].String_Event == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = EventImg[x].Sprite_Event;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_FirstQuestion.Contains("Other")){
            for(int x = 0;x < OtherImg.Count;x++){
            if (OtherImg[x].String_Other == GetComponent<UgcScript>().SplitImg_FirstQuestion){
                Sprite_UpperGallery = OtherImg[x].Sprite_Other;
            }
            }
        }
    }
    
    public void FindImage_LowerQuestion ()
    {
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("People")){           
            for(int x = 0;x < PeopleImg.Count;x++){
            if (PeopleImg[x].String_People == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = PeopleImg[x].Sprite_People;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("Animal")){           
            for(int x = 0;x < AnimalsImg.Count;x++){
            if (AnimalsImg[x].String_Animal == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = AnimalsImg[x].Sprite_Animal;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("Thing")){           
            for(int x = 0;x < ThingImg.Count;x++){
            if (ThingImg[x].String_Thing == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = ThingImg[x].Sprite_Thing;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("Food")){           
            for(int x = 0;x < FoodImg.Count;x++){
            if (FoodImg[x].String_Food == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = FoodImg[x].Sprite_Food;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("Event")){           
            for(int x = 0;x < EventImg.Count;x++){
            if (EventImg[x].String_Event == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = EventImg[x].Sprite_Event;
            }
            }
        }
        if (GetComponent<UgcScript>().SplitImg_SecondQuestion.Contains("Other")){           
            for(int x = 0;x < OtherImg.Count;x++){
            if (OtherImg[x].String_Other == GetComponent<UgcScript>().SplitImg_SecondQuestion){
                Sprite_LowerGallery = OtherImg[x].Sprite_Other;
            }
            }
        }
    }
}
