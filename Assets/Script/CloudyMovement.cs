using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 speed = Vector2.zero;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime);
    }
}
