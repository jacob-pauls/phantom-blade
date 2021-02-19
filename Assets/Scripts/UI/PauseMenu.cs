using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : StandardMenu
{


    public override void Show()
    {
        base.Show();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }
}
