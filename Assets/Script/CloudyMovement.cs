using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 speed = Vector2.zero;

    [SerializeField]
    private float timeToStayAliveWhenHidden = 10f;

    private float timeHidden = 0;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Action<CloudyMovement> onTimeOut = null;

    public void SetUp(Sprite sprite, Action<CloudyMovement> onTimeOut = null)
    {
        spriteRenderer.sprite = sprite;
        this.onTimeOut = onTimeOut;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime);

        if (!spriteRenderer.isVisible)
        {
            timeHidden += Time.deltaTime;
            if (timeHidden > timeToStayAliveWhenHidden)
            {
                onTimeOut?.Invoke(this);
            }
        }
        else
        {
            timeHidden = 0;//reset;
        }

    }
}
