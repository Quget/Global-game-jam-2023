using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickable : MonoBehaviour
{

    [SerializeField]
    private PowerUpSetting powerUpSetting;
    public PowerUpSetting PowerUpSetting => powerUpSetting;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;


    public void SetUp(PowerUpSetting powerUpSetting)
    {
        this.powerUpSetting = powerUpSetting;
    }

    private void Start()
    {
        spriteRenderer.sprite = powerUpSetting.ItemSprite;
        spriteRenderer.color = powerUpSetting.Color;
        gameObject.name = powerUpSetting.Name;
    }
}
