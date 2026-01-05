using System;
using UnityEngine;

[Serializable]
public class Vehicle
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Sprite Icon { get; set; }
    [field: SerializeField] public int Speed { get; set; }
    [field: SerializeField] public int Acceleration { get; set; }
    [field: SerializeField] public int Handling { get; set; }
    [field: SerializeField] public int Braking { get; set; }
}