using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skillList",menuName ="skills",order = 0)]
public class SkillList : ScriptableObject
{
    public Skill[] skill;
}
