using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : StandardMenu
{
    public TPB_Character player;


    public override void Initialize()
    {
        

        if (!isInitialized)
        {
            player.onDeath?.AddListener(Dying);
            animator = GetComponent<Animator>();
            isInitialized = true;
        }
    }
    

    void Dying()
    {
        Show();
        //Time.timeScale = 0;
    }

}
