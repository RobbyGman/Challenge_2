using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControleScript : MonoBehaviour
{

    public static MusicControleScript instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 
   
}
