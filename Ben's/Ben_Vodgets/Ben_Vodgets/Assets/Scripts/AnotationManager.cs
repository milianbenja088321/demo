using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotationManager : MonoBehaviour
{
    private enum Options { backButton = 0, moreButton, }
    public List<GameObject> annotations;
    public GameObject single;

    public void DeactivateAll()
    {
        foreach (GameObject anot in annotations)
        {
            anot.SetActive(false);
        }
    }

    public void View(int index)
    {
        annotations[index].SetActive(true);
    }

    public void Hide(int index)
    {
        annotations[index].SetActive(false);
    }


}
