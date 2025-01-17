using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    public abstract Rigidbody RB { get; }
    public abstract float EngineUnderWaterPower { get; }
    public abstract float CurrentAcceleration { get; }
}
