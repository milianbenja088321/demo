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
    public Animation scene;

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
                Text.text = "A key element of conductive education is that individuals work as part of a small group of children with the same condition. This offers an opportunity for individuals to face challenges, share solutions and reward efforts to learn new skills within the dynamics of a group.";
                break;
            case 1:
                Text.text = "Many of the activities are done with the use of simple folk songs that relate to the activity. By pairing rhythm with movement, movements become more fluid and the lyrics provide verbal cues to the child.";
                break;
            case 2:
                Text.text = "Conductive education helps these students build their cognitive skills and helps them learn to use alternate strategies to accomplish common motor tasks such as sitting, standing, walking, dressing, eating.";
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

    public void PauseRewind()
    {
        if (model.speed == -1)
        {
            model.speed = (int)Options.Pause;
            isPlaying = false;
        }
    }

    public void HideAllObject()
    {
        foreach (GameObject obj in anoObjList)
        {
            obj.SetActive(false);
        }
    }


}
