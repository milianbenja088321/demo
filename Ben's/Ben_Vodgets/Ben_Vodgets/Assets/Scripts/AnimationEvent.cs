using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    AnotationManager anoManager;
    public Dictionary<GameObject, string> annotationDictionary;


    [SerializeField] short play = 0;

    private void Awake()
    {
        for (int i = 0; i < anoManager.annotations.Count; i++) { }
        //{
        //    annotationDictionary.Add(anoManager.annotations[i], anoManager.annotations[i].name);
        //}
    }
    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
        this.GetComponent<Animator>().speed = play;
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
