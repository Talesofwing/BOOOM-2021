using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "Scenario/Scenario", order = 2)]
public class Scenario : ScriptableObject
{
    [SerializeField]private int id;
    public int ID => id;
    [SerializeField]private string title;
    public string Title => title;   // 暂时没用
    [SerializeField]private Sprite cg;
    public Sprite CG => cg;
    [SerializeField]private List<string> description;
    public List<string> Desc => description;
    [SerializeField]private bool isOnlyShowOnce = false;
    public bool IsOnlyShowOnce => isOnlyShowOnce;

    [SerializeField] private Scenario nextScenario;
    public Scenario NextScenario => nextScenario;   // 下一条剧本
}
