using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseButton : MonoBehaviour {

    [SerializeField]
    private Image ButtonImage;
    [SerializeField]
    private Sprite PauseTexture;
    [SerializeField]
    private Sprite PlayTexture;
    [SerializeField]
    private KeyCode Hotkey;

    private bool mIsPaused = false;


    //-----------------------------Unity Functions-----------------------------

    private void Update()
    {
        if (Input.GetKeyDown(Hotkey))
            TogglePause();
    }


    //----------------------------Public Functions-----------------------------

    public void TogglePause()
    {
        mIsPaused = !mIsPaused;

        Time.timeScale = mIsPaused ? 0f : 1f;
        ButtonImage.sprite = mIsPaused ? PlayTexture : PauseTexture;
    }
}
