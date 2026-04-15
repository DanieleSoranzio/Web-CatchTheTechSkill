using UnityEngine;

[CreateAssetMenu(fileName = "skillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillsData : ScriptableObject
{
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public bool isCatchable { get; private set; } = true;
}
