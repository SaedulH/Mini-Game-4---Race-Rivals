using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Spawnpoint Step", menuName = "Levels/InitSteps/InitSpawnpointStep")]
    public class InitSpawnpointStepSO : LevelInitStepSO
    {
        [SerializeField] private Vector3 PlayerOneSpawnPosition;
        [SerializeField] private Quaternion PlayerOneSpawnRotation;

        [SerializeField] private Vector3 PlayerTwoSpawnPosition;
        [SerializeField] private Quaternion PlayerTwoSpawnRotation;

        public override async Task Run(TrackContext context)
        {
            GameManager.Instance.PlayerOne.transform.SetPositionAndRotation(PlayerOneSpawnPosition, PlayerOneSpawnRotation);
            if (GameManager.Instance.PlayerTwo != null)
            {
                GameManager.Instance.PlayerTwo.transform.SetPositionAndRotation(PlayerTwoSpawnPosition, PlayerTwoSpawnRotation);
            }

            await Task.CompletedTask;
        }
    }
}

