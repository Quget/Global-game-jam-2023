using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField]
    private Healthbar playerHealthBar = null;

    [SerializeField]
    private Healthbar bossHealthBar = null;

    public void UpdateBossHealth(float percentage)
    {
        bossHealthBar.UpdateHealth(percentage);
    }

    public void UpdatePlayerHealth(float percentage)
    {
        playerHealthBar.UpdateHealth(percentage);
    }
}
