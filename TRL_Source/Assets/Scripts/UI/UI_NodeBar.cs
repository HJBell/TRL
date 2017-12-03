using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NodeBar : MonoBehaviour {

    [SerializeField]
    private GameObject NodePrefab;
    [SerializeField]
    private Vector2 PixelOffset = new Vector2(0f, 40f);

    private List<GameObject> mForegroundNodes = new List<GameObject>();
    private List<GameObject> mBackgroundNodes = new List<GameObject>();


    //----------------------------Public Functions-----------------------------

    public void SetValue(int value, int maxValue, Color col)
    {
        if (mForegroundNodes.Count == value) return;
         
        foreach (var node in mForegroundNodes)
            Destroy(node);
        mForegroundNodes.Clear();
        foreach (var node in mBackgroundNodes)
            Destroy(node);
        mBackgroundNodes.Clear();

        int padding = 3;
        float width = this.GetComponent<RectTransform>().rect.width;
        float step = width / maxValue;
        float nodeWidth = step - padding;
        float startX = transform.position.x - width / 2 + step / 2;

        for (int i = 0; i < maxValue; i++)
        {
            // Background.
            var objBackground = Instantiate(NodePrefab) as GameObject;
            objBackground.transform.SetParent(transform);

            var pos = transform.position;
            pos.x = startX + step * i;
            objBackground.transform.position = pos;

            var scale = objBackground.transform.localScale;
            scale.x = nodeWidth / objBackground.GetComponent<RectTransform>().rect.width;
            objBackground.transform.localScale = scale;

            objBackground.GetComponent<Image>().color = Color.black;

            mBackgroundNodes.Add(objBackground);

            if(i < value)
            {
                // Foreground.
                var objForeground = Instantiate(NodePrefab) as GameObject;
                objForeground.transform.SetParent(transform);

                objForeground.transform.SetAsLastSibling();
                objForeground.transform.position = pos;
                objForeground.transform.localScale = scale;
                objForeground.GetComponent<Image>().color = col;

                mForegroundNodes.Add(objForeground);
            }
        }
    }

    public void SetWorldPos(Vector3 worldPos)
    {
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        this.transform.position = screenPos + new Vector3(PixelOffset.x, PixelOffset.y, 0f);
    }
}
