using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "GameData/Creature/Collection")]
public class CreatureDataCollection : ScriptableObject
{
    public List<CreatureDataEntry> Creatures;
}