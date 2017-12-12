using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    AnotationManager anoManager;
    public Dictionary<GameObject, string> annotationDictionary;


    [SerializeField] short play = 1;

    private void Awake()
    {

    }
    public void PrintEvent(string s)
    {
       
    }

    public void ShowPresentation(string _value)
    {
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (play == 0)
                play = 1;
            else
                play = 0;

            this.GetComponent<Animator>().speed = play;

        }
    }
}
