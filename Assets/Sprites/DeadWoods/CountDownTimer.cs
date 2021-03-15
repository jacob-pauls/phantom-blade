using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDownTimer : MonoBehaviour
{

    public int levelToLoad;
    public float timer = 34f;
    private Text timerSeconds;
    private LevelControl levelController;

    // Start is called before the first frame update
    void Start()
    {
        levelController = new LevelControl();
        timerSeconds = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("f2");
        if (timer <= 0)
        {
            levelController.LoadLevel(levelToLoad);
        }
    }
}
