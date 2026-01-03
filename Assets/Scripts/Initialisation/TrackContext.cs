using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using static Utilities.Constants;

namespace CoreSystem
{
    public class TrackContext
    {
        public GameManager Manager;
        public AsyncOperationHandle<SceneInstance> SceneHandle;
        public GameMode GameMode;
        public int PlayerCount;
        public int VehicleOneIndex;
        public int VehicleTwoIndex;
        public int LapCount;
        public float TotalWeight;
    }
}

