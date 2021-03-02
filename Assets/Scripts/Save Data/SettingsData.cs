using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData : GameSaveFile
{
    public bool isAmbienceEnabled;
    public bool isMusicEnabled;
    public bool isSFXEnabled;
    public bool isUIEnabled;

    public override void UpdateBuildVersion()
    {

    }
}
