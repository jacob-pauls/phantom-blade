﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ResurrectionButtonScript : MonoBehaviour
{
    
    public void restartScene()
    {
        SceneManager.LoadScene("graveYard");
    }
}
