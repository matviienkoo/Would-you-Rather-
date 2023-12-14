using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperementalScript : MonoBehaviour 
{
    public bool bool_test;
    public List<string> LIST = new List<string>();
    public int fps;
    public Text text_fps;

    public string string_text = "dsfsdfsdfsdlf34k34iojr34oji34jk534lkj534l5j43";

    public void TestButton ()
    {
        bool_test = true;
    }

    private void FixedUpdate ()
    {
        
    }

    private void Update ()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);

        text_fps.text = fps.ToString();

        if (bool_test == true)
        {
            LIST.Add(string_text);
        }
    }
}
