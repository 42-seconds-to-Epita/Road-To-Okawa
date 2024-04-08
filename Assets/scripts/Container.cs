using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Container : MonoBehaviour
{
    private int currentStatus = 1;
    public int containerId = 0;

    public void FillContainer()
    {
        if (currentStatus >= 11)
        {
            return;
        }

        GameObject child = gameObject.GetNamedChild("Content");
        if (child == null)
        {
            child.SetActive(false);
        }

        child.transform.localPosition += new Vector3(0, 0.05f, 0);
        child.transform.localScale += new Vector3(0, 0.1f, 0);
        currentStatus += 1;
    }
}