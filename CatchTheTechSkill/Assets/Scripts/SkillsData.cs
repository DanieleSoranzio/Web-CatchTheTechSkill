using UnityEngine;

[CreateAssetMenu(fileName = "skillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillsData : ScriptableObject
{
    public Sprite sprite { private set; get; }
    public bool isCatchable=true;
}
