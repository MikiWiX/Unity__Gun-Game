using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour, ICloseable
{
    public void close()
    {
        MenuTools.ChangeTab(transform.parent.gameObject, "Main");
    }
}
