using System;
using UnityEngine;

[Serializable]
public class Vehicle
{
    [field: SerializeField] public string VehicleName { get; set; }
    [field: SerializeField] public VehicleStats Stats { get; set; }
    [field: SerializeField] public VehicleVisualSettings VisualSettings { get; set; }

    [field: Header("Player One Settings")]
    [field: SerializeField] public Sprite PlayerOneVehicleDisplayIcon { get; set; }
    [field: SerializeField] public Sprite PlayerOneVehicleChassisSprite { get; set; }

    [field: Header("Player Two Settings")]
    [field: SerializeField] public Sprite PlayerTwoVehicleDisplayIcon { get; set; }
    [field: SerializeField] public Sprite PlayerTwoVehicleChassisSprite { get; set; }
}