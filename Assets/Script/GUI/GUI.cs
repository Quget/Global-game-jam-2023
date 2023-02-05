using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField]
    private Healthbar playerHealthBar = null;

    [SerializeField]
    private Healthbar bossHealthBar = null;

    [SerializeField]
    private GameObject gameOverObject = null;

    [SerializeField]
    private TextMeshProUGUI gameOverText = null;

    [SerializeField]
    private TextMeshProUGUI bossKillTime = null;


    private void Awake()
    {
        gameOverObject.SetActive(false);
    }

    public void UpdateBossHealth(float percentage)
    {
        bossHealthBar.UpdateHealth(percentage);
    }

    public void UpdatePlayerHealth(float percentage)
    {
        playerHealthBar.UpdateHealth(percentage);
    }

    public void ShowGameOver(string text, float timeToKillBoss = -1)
    {
        gameOverText.text = text;
        if (timeToKillBoss > 0)
            bossKillTime.text = $"It took you {timeToKillBoss} seconds";
        else
            bossKillTime.text = "";

        gameOverObject.SetActive(true);
    }
}
