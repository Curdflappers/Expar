using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    static int slide = 0;
    public GameObject button;
    float timer;
    void Start()
    {
        ButtonEnable(false);
        timer = 3f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) || timer < 0)
        {
            timer = 3f;
            Intro.slide++;
            if (slide < 4)
            {
                GameObject.Find("Intro Image").GetComponent<Image>().sprite = 
                    (Sprite)Resources.Load("Sprites/Intro/Slide" + slide, typeof(Sprite));
            }
            else
            {
                ButtonEnable(true);
            }
        }
    }

    void ButtonEnable(bool enable)
    {
        button.GetComponent<Image>().enabled = enable;
        button.GetComponentInChildren<Text>().enabled = enable;
    }
}