using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_PauseButton : MonoBehaviour {

    [SerializeField]
    private Image ButtonImage;
    [SerializeField]
    private Sprite PauseTexture;
    [SerializeField]
    private Sprite PlayTexture;
    [SerializeField]
    private KeyCode Hotkey;

    private Button mButton;
    private bool mIsPaused = false;


    //-----------------------------Unity Functions-----------------------------

    private void Start()
    {
        mButton = GetComponent<Button>();
        SetPause(true);
    }

    private void Update()
    {
        
        if (GameInfo.State != GameState.Battle)
        {
            mButton.interactable = false;
            return;
        }
        else
            mButton.interactable = true;

        if (Input.GetKeyDown(Hotkey))
            TogglePause();
    }


    //----------------------------Public Functions-----------------------------

    public void TogglePause()
    {
        SetPause(!mIsPaused);
    }

    public void SetPause(bool value)
    {
        mIsPaused = value;

        Time.timeScale = mIsPaused ? 0f : 1f;
        ButtonImage.sprite = mIsPaused ? PlayTexture : PauseTexture;
    }
}
