using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace CoreSystem
{
    public class TrackContext
    {
        public GameManager Manager;
        public AsyncOperationHandle<SceneInstance> SceneHandle;
        public string GameMode;
        public int PlayerCount;
        public int VehicleOneIndex;
        public int VehicleTwoIndex;
        public float TotalWeight;
    }
}

