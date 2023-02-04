using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bonusitem", menuName = "ScriptableObjects/BonusItemSetting", order = 1)]
public class PowerUpSetting : ScriptableObject
{
    [SerializeField]
    private PlayerStats playerStats;

    public PlayerStats PowerUp => playerStats;

    [SerializeField]
    private Sprite itemSprite;
    public Sprite ItemSprite => itemSprite;

    [SerializeField]
    private Color color = Color.black;

    public Color Color => color;

    [SerializeField]
    private new string name = "item";

    public string Name => name;
}
