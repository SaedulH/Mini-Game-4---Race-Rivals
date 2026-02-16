using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "ScenePreset", menuName = "Presets/ScenePreset")]
    public class ScenePreset : ScriptableObject
    {
        [field: SerializeField] public string SceneName { get; set; }
        [field: SerializeField] public LoadSceneMode LoadMode { get; set; } = LoadSceneMode.Additive;
        [field: SerializeField] public TrackInfo TrackInfo { get; set; }
        [field: SerializeField] public TrackContext TrackContext { get; set; }
        [field: SerializeField] public VehicleSelector Vehicles { get; set; }
        [field: SerializeField] public PresetVehicle PlayerOneVehicle { get; set; }
        [field: SerializeField] public PresetVehicle PlayerTwoVehicle { get; set; }

        private void OnValidate()
        {
            if (TrackInfo == null || TrackContext == null || Vehicles == null) return;

            TrackContext.TotalWeight = (int)TrackInfo.StepOrder.Sum(s => s.Weight);
            TrackContext.LapCount = TrackInfo.GetLapCountForMode(TrackContext.GameMode);

            TrackContext.LapCount = TrackInfo.GetLapCountForMode(TrackContext.GameMode);

            if(TrackContext.VehicleOne.VehicleName != Vehicles.AvailableVehicles[(int)PlayerOneVehicle].VehicleName)
            {
                TrackContext.VehicleOne = Vehicles.AvailableVehicles[(int)PlayerOneVehicle];
            }

            if(TrackContext.VehicleTwo.VehicleName != Vehicles.AvailableVehicles[(int)PlayerTwoVehicle].VehicleName)
            {
                TrackContext.VehicleTwo = Vehicles.AvailableVehicles[(int)PlayerTwoVehicle];
            }
        }
    }
}