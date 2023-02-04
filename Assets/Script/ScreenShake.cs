using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    [SerializeField]
    private float duration = 0.25f;

    private float shakyNess = 0.1f;

    private Coroutine shakeCoroutine = null;

    private static ScreenShake instance = null;

    public static ScreenShake Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void ShakeScreen()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(StartShake());
    }

    private IEnumerator StartShake()
    {
        //Vector3 startPos = Camera.main.transform.position;
        float time = duration;
        while(time > 0)
        {
            Camera.main.transform.position = Camera.main.transform.position + Random.insideUnitSphere * shakyNess;
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
        }
        shakeCoroutine = null;
    }
}
