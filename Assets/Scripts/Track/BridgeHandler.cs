using System;
using UnityEngine;
using Utilities;

public class BridgeHandler : MonoBehaviour
{
    [field: Header("Triggers")]
    [field: SerializeField] public BridgeTrigger UnderPassInTrigger { get; set; }
    [field: SerializeField] public BridgeTrigger UnderPassOutTrigger { get; set; }
    [field: SerializeField] public BridgeTrigger OverPassInTrigger { get; set; }
    [field: SerializeField] public BridgeTrigger OverPassOutTrigger { get; set; }

    [field: Header("Colliders")]
    [field: SerializeField] public BoxCollider2D UnderPassInCollider { get; set; }
    [field: SerializeField] public BoxCollider2D UnderPassOutCollider { get; set; }
    [field: SerializeField] public BoxCollider2D OverPassInCollider { get; set; }
    [field: SerializeField] public BoxCollider2D OverPassOutCollider { get; set; }

    private void OnEnable()
    {
        SubscribeTriggerEvent(UnderPassInTrigger);
        SubscribeTriggerEvent(UnderPassOutTrigger);
        SubscribeTriggerEvent(OverPassInTrigger);
        SubscribeTriggerEvent(OverPassOutTrigger);
    }

    private void OnDisable()
    {
        UnsubscribeTriggerEvent(UnderPassInTrigger);
        UnsubscribeTriggerEvent(UnderPassOutTrigger);
        UnsubscribeTriggerEvent(OverPassInTrigger);
        UnsubscribeTriggerEvent(OverPassOutTrigger);
    }

    private void SubscribeTriggerEvent(BridgeTrigger bridgeTrigger)
    {
        if (bridgeTrigger != null)
        {
            bridgeTrigger.TriggerEvent += OnBridgeTriggerEvent;
        }
    }

    private void UnsubscribeTriggerEvent(BridgeTrigger bridgeTrigger)
    {
        if (bridgeTrigger != null)
        {
            bridgeTrigger.TriggerEvent -= OnBridgeTriggerEvent;
        }
    }

    private void OnBridgeTriggerEvent(GameObject player, BridgeTriggerType type, BridgeTriggerDirection direction)
    {
        Debug.Log($"Bridge Trigger Event {type}:{direction} for player {player.name}");

        bool isPlayerOne = player.name == "PlayerOne";
        bool isUnderPass = type == BridgeTriggerType.Underpass;

        if (player.TryGetComponent(out LayerHandler layerHandler))
        {
            layerHandler.SetBridgeSortingLayer(isUnderPass);
        }
        if (player.TryGetComponent(out CapsuleCollider2D collider))
        {
            Physics2D.IgnoreCollision(collider, UnderPassInCollider, isUnderPass);
            Physics2D.IgnoreCollision(collider, UnderPassOutCollider, isUnderPass);
            Physics2D.IgnoreCollision(collider, OverPassInCollider, !isUnderPass);
            Physics2D.IgnoreCollision(collider, OverPassOutCollider, !isUnderPass);
        }
    }
}
