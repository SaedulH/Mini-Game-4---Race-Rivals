using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private string checkpointNum;
    [SerializeField] private int checkpointNumInt;
    [SerializeField] private int playerNumInt;
    // Start is called before the first frame update
    void Start()
    {
        checkpointNum = gameObject.name;
        checkpointNumInt = int.Parse(checkpointNum);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            string playerNum = collision.gameObject.name;
            string numericPart = playerNum.Substring("player".Length);
            playerNumInt = int.Parse(numericPart);
            
            CheckPointManager.Instance.checkpointReached(checkpointNumInt, playerNumInt);
        }
    }
}
