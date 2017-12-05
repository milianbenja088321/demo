using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{
    public enum InteractiveTypes { Button = 0, Grab, Marker, None = 50};
    public InteractiveTypes type = InteractiveTypes.None;

    // Use this for initialization
    public void something()
    {
        print("Something");
        Button butt = this.GetComponent<Button>();

        if (butt != null)
        {
            butt.onClick.Invoke();
        }
    }

    public void SetActive(GameObject _annotation)
    {
        _annotation.SetActive(true);
    }

    public void Deactivate(GameObject _annotation)
    {
        _annotation.SetActive(false);
    }

    // use generic names
    public virtual void Grab(Transform _controller)
    {
        this.transform.SetParent(_controller, true);
        Rigidbody rgb = this.GetComponent<Rigidbody>();
        rgb.isKinematic = true;
    }

    public virtual void Release(Vector3 _controller)
    {
        this.transform.SetParent(null);
        Rigidbody rgb = this.GetComponent<Rigidbody>();
        rgb.isKinematic = false;

        rgb.AddForce(_controller * 10, ForceMode.VelocityChange);

    }


}
