using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [field: SerializeField] public int CheckpointNumInt { get; private set; }
    [field: SerializeField] public bool IsNextPlayerOneCheckpoint { get; set; }
    [field: SerializeField] public bool IsNextPlayerTwoCheckpoint { get; set; }

    private void OnValidate()
    {
        gameObject.name = CheckpointNumInt.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            int playerNumber = collision.gameObject.name == "PlayerOne" ? 1 : 2;
            CheckPointManager.Instance.CheckpointReached(CheckpointNumInt, playerNumber);
            if ((playerNumber == 1 && IsNextPlayerOneCheckpoint) || (playerNumber == 2 && IsNextPlayerTwoCheckpoint))
            {
            }
        }
    }

    public void ResetCheckpoint()
    {
        gameObject.name = CheckpointNumInt.ToString();
        if (CheckpointNumInt == 1)
        {
            IsNextPlayerOneCheckpoint = true;
            IsNextPlayerTwoCheckpoint = true;
        }
        else
        {
            IsNextPlayerOneCheckpoint = false;
            IsNextPlayerTwoCheckpoint = false;
        }
    }
}
