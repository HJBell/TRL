using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_FadeIn : MonoBehaviour {

    [SerializeField]
    private float FadeDuration = 1f;

    private float mFadeStartTime = 0f;

    private void OnEnable()
    {
        mFadeStartTime = Time.unscaledTime;
    }

    private void Update()
    {
        var alpha = 2f - (Time.unscaledTime - mFadeStartTime) / FadeDuration;
        GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);
        if (alpha <= 0f)
            Destroy(gameObject);
    }
}
