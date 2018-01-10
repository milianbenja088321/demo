using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    Vector3 startPos;
    // Use this for initialization
    void Awake()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 80 * Time.deltaTime);

        if (this.gameObject.name == "Arrow")
        {
            if (this.transform.position.x < -1.5f)
                this.transform.Translate(Time.deltaTime * 1, 0, 0, Space.World);
            else
                this.transform.position = startPos;
        }

        if (this.gameObject.name == "Down Arrow")
        {
            if (this.transform.position.x < 0f && this.transform.position.y > -0.2)
                this.transform.Translate(Time.deltaTime * .5f, Time.deltaTime * -.5f, 0, Space.World);
            else
                this.transform.position = startPos;
        }
    }
}
