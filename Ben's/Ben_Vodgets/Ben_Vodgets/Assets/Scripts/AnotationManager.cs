using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotationManager : MonoBehaviour
{
    private enum Options { Pause = 0, Play }
    public bool isPlaying = false;
    public GameObject Canvas = null;
    public PlayableClip model = null;
    public AnimationState state = null;
    public Text Text = null;
    public GameObject[] anoObjList = null;
    public AudioSource load;

    private void Start()
    {
        model.speed = 0;
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

    public void PlayAudio()
    {
        load.Play();
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
