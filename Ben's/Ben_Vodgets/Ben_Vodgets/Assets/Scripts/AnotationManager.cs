﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotationManager : MonoBehaviour
{
    private enum Options { Pause = 0, Play }
    public GameObject Canvas = null;
    public PlayableClip model = null;
    public Animation anim = null;
    public AnimationState state = null;
    public Text Text = null;
    public bool isPlaying = false;

    private void Start()
    {
        model.speed = 0;
        anim = GetComponent<Animation>();
    }

    public void View(GameObject _object)
    {
        _object.SetActive(true);
    }

    public void Hide(GameObject _object)
    {
        _object.SetActive(false);
    }

    public void SetText(int _index)
    {
        switch (_index)
        {
            case 0:
                Text.text = "Test Scenario One";
                break;
            case 1:
                Text.text = "Test Scenario Two";
                break;
            case 2:
                Text.text = "Test Scenario Three";
                break;
        }
    }

    public void Play()
    {
       // model.playableGraph.Play();
        model.speed = (int)Options.Play;
        isPlaying = true;
    }

    public void StartAnimation()
    {
        model.speed = (int)Options.Play;
    }

    public void Pause()
    {
        if (model.speed != 0)
        {
            model.speed = (int)Options.Pause;
            isPlaying = false;
        }
    }

    public void Rewind()
    {
        print("Rewind");
        model.speed = -1;
    }


}
