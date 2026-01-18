using System;
using UnityEngine;

[Serializable]
public class Vehicle
{
    [field: SerializeField] public string PlayerOneName { get; set; }
    [field: SerializeField] public Sprite PlayerOneIcon { get; set; }
    [field: SerializeField] public string PlayerTwoName { get; set; }
    [field: SerializeField] public Sprite PlayerTwoIcon { get; set; }
    [field: SerializeField] public VehicleStats Stats { get; set; }

}