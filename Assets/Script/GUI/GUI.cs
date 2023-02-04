using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField]
    private Healthbar healthbar = null;

    public void UpdateHealth(float percentage)
    {
        healthbar.UpdateHealth(percentage);
    }
}
