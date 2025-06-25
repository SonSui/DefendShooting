using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleTMPController : MonoBehaviour
{
    public TextMeshProUGUI text;
    float alpha = 1f;
    bool dir = true;
    public float deltaAlpha = 0.01f;
    Color color;
    private void Start()
    {
        if (text == null)
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogWarning("No TMP component");
                color = Color.red;
                return;
            }
        }
        color = text.color;
    }
    private void Update()
    {
        if (dir)
        {
            alpha -= deltaAlpha;
            if (alpha <= 0f)
            {
                alpha = 0f;
                dir = false;
            }
        }
        else
        {
            alpha += deltaAlpha;
            if (alpha >= 1f)
            {
                alpha = 1f;
                dir = true;
            }
        }
        Color tc = new Color(color.r, color.g, color.b, alpha);
        text.color = tc;
    }
}
