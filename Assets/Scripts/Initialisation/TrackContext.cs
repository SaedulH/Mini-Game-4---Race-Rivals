using System;
using UnityEngine;
using Utilities;

namespace CoreSystem
{
    [Serializable]
    public class TrackContext
    {
        public GameMode GameMode;
        [field: Range(min: 1, max: 2)] public int PlayerCount;
        public Vehicle VehicleOne;
        public Vehicle VehicleTwo;
        public int LapCount;
        public float TotalWeight;
    }
}

