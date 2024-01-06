using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField]private int id;
    public int ID => id;
    [SerializeField]private new string name;
    public string Name => name;
    [SerializeField]private string description;
    public string Desc => description;
    [SerializeField]private int maxStack = 99;
    public int MaxStack => maxStack;
    [SerializeField]private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private List<CraftMaterial> craftRecipe;
    public List<CraftMaterial> CraftRecipe => craftRecipe;
    
    [Serializable]
    public struct CraftMaterial
    {
        public ItemData item;
        public int itemId => item?.ID ?? -1;
        public int cost;
    }

    
    [SerializeField]private int craftCount = 1;
    public int CraftCount => craftCount;
    
    [SerializeField]private int craftSpeed = 1;
    public int CraftSpeed => craftSpeed;

    [SerializeField]private int soundID = 1;
    public int SoundID => soundID;
}
