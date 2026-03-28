using System;
using UnityEngine;
using Utilities;

public class BridgeTrigger : MonoBehaviour 
{
    [field: SerializeField] public BridgeTriggerType TriggerType  {  get; set; }
    [field: SerializeField] public BridgeTriggerDirection TriggerDirection  {  get; set; }

    public Action<GameObject, BridgeTriggerType, BridgeTriggerDirection> TriggerEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            BridgeTriggerEvent(collision.gameObject);
        }
    }

    private void BridgeTriggerEvent(GameObject playerObject)
    {
        TriggerEvent?.Invoke(playerObject, TriggerType, TriggerDirection);
    }
}