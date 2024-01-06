using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObjects/EntityData", order = 2)]
public class ScreenEntityData : ScriptableObject
{
    [SerializeField]private int id;
    public int ID => id;
    [SerializeField]private new string name;
    public string Name => name;
    [SerializeField]private Sprite icon;
    public Sprite Icon => icon;

    // [SerializeField] private ItemData item;
    // public ItemData ItemData => item;
    // [SerializeField] private int itemCount = 1;
    // public int ItemCount => itemCount;
}
