using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField]
    private RectTransform fill = null;

    private RectTransform rectTransform = null;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        fill.sizeDelta = new Vector2(rectTransform.rect.width, fill.sizeDelta.y);
    }

    public void UpdateHealth(float percentage)
    {
        float onePercent = rectTransform.rect.width / 100f;
        fill.sizeDelta = new Vector2(onePercent * percentage, fill.sizeDelta.y);
    }

}
