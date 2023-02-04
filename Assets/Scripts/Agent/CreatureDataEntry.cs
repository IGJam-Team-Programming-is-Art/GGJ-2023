using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Creature/Data Entry")]
[Serializable]
public class CreatureDataEntry : ScriptableObject
{
    public Creature Type;
    public GameObject Prefab;
}