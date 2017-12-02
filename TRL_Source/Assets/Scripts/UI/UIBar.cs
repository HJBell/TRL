using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {

    [SerializeField]
    private Transform ForegroundTrans;
    [SerializeField]
    private Vector2 PixelOffset = new Vector2(0f, 40f);


    //----------------------------Public Functions-----------------------------

    public void SetValue(float value, float maxValue)
    {
        var foregroundScale = ForegroundTrans.localScale;
        foregroundScale.x = value / maxValue;
        ForegroundTrans.localScale = foregroundScale;
    }

    public void SetWorldPos(Vector3 worldPos)
    {
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        this.transform.position = screenPos + new Vector3(PixelOffset.x, PixelOffset.y, 0f);
    }
}
