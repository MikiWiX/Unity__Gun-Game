using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopUp : MonoBehaviour, ICloseable
{
    public UnityEvent onClose;
    void Awake()
    {
        noEventClose();
    }

    private void noEventClose()
    {
        gameObject.SetActive(false);
    }
    public void close()
    {
        noEventClose();
        onClose.Invoke();
    }
    public void open()
    {
        gameObject.SetActive(true);
    }
}
