using System.Collections;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    private int currentLevel = 4;
    private bool emptyInProgress = false;
    
    public bool UpdateLevel()
    {
        if (currentLevel < 1 || emptyInProgress)
        {
            return false;
        }
        
        DisableChildrenByName("liquide" + currentLevel);
        currentLevel -= 1;
        
        StartCoroutine(StartEmpty(3f));
        return true;
    }
 
    private IEnumerator StartEmpty(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        emptyInProgress = false;
    }
    
    
    
    void DisableChildrenByName(string name)
    {
        Transform child = transform.Find(name);
        
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
    }
}