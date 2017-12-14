using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AnotationManager anoManager = null;

    void Pause()
    {
        anoManager.Pause();
    }

    void Show()
    {
        anoManager.View(anoManager.Canvas);
    }

    void StepText(int index)
    {
        anoManager.SetText(index);
    }

    void ShowAnnotations(GameObject _obj)
    {
        anoManager.View(_obj);
    }

}
