using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int currentLevel = 0;
    public bool randomInit = false;

    void Start()
    {
        if (randomInit) {
            currentLevel = Random.Range(0, 11);
        }
        
        GameObject child = gameObject.GetNamedChild("Content");
        child.transform.localPosition += new Vector3(0, 0.05f * currentLevel, 0);
        child.transform.localScale += new Vector3(0, 0.1f * currentLevel, 0);
    }
}
