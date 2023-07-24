using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{

    void Awake()
    {
        
        //Find the object in the hierarchy called ScenePersist
        int numGamePersists = FindObjectsOfType<ScenePersist>().Length;
        if (numGamePersists > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }

}
