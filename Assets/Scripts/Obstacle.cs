using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Trigger playerInteractionTrigger;

    private void Start()
    {
        playerInteractionTrigger.OnInteractionStay += PlayerInteractionTrigger_OnInteractionStay;
    }

    private void PlayerInteractionTrigger_OnInteractionStay()
    {
    }
}
