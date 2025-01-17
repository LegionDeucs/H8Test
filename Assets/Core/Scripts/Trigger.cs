using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected LayerMask interactionLayer;

    public event System.Action OnInteractionEnter;
    public event System.Action OnInteractionExit;
    public event System.Action OnInteractionStay;

    private void OnTriggerEnter(Collider other)
    {
        if (interactionLayer.Includes(other.gameObject.layer))
            OnInteractionEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactionLayer.Includes(other.gameObject.layer))
            OnInteractionExit?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (interactionLayer.Includes(other.gameObject.layer))
            OnInteractionStay?.Invoke();
    }
}
