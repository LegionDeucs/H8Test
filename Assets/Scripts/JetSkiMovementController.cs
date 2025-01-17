using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSkiMovementController : MovementController
{
    //TMP
    public static JetSkiMovementController Instance;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Engine engine;
    [SerializeField] private DragForce dragForce;
    [SerializeField] private ArchimedForce archimedForce;

    public override Rigidbody RB => rb;

    public override float EngineUnderWaterPower => engine.GetEngineGroundedPower();

    public override float CurrentAcceleration => engine.CurrentAcceleration;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.touchCount != 1 && !Input.GetMouseButton(0))
            engine.RotateEngine(.5f);
        else if (Input.mousePosition.x > Screen.width / 2)
            engine.RotateEngine(0);
        else
            engine.RotateEngine(1);

        engine.DoUpdate();
    }

    private void FixedUpdate()
    {
        float volumeUnderWater = archimedForce.GetVolumeUnderWater(transform);
        dragForce.SetupDrag(rb, volumeUnderWater);

        rb.AddForceAtPosition(engine.GetEngineAcceleration(), engine.EnginePoint.position, ForceMode.Acceleration);

        var archimedForceV = archimedForce.GetArchimedForce(volumeUnderWater, rb.velocity.magnitude, out Vector3 forceLocalPosition);
        print(archimedForceV.magnitude + " archimedForceV");
        rb.AddForceAtPosition(archimedForceV, forceLocalPosition + transform.position, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity * 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        engine.DropAcceleration();
    }
}

[System.Serializable]
public class Engine
{
    [Header("Engine Rotation")]
    [SerializeField] private Transform enginePoint;
    [SerializeField] private Vector3 engineRotationAxis;
    [SerializeField] private Quaternion defaultRotation;
    [SerializeField] private float maximumAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Engine force")]
    [SerializeField] private float accelerationAcecleration = .2f;
    [SerializeField] private float maxAcceleration = 50;

    [SerializeField] private Vector2 minMaxYEngineToWork;
    [SerializeField] private Vector2 minMaxInfluence;

    [SerializeField] protected float speedDropPercent;

    public Transform EnginePoint => enginePoint;

    public float CurrentAcceleration { get; private set; }

    public void DoUpdate()
    {
        CurrentAcceleration = Mathf.Clamp(CurrentAcceleration + maxAcceleration * accelerationAcecleration * Time.deltaTime, 0, maxAcceleration);
    }

    public void RotateEngine(float targetRotation)
    {
        Quaternion targetQuaternionRotation = Quaternion.Lerp(Quaternion.AngleAxis(-maximumAngle, engineRotationAxis),
            Quaternion.AngleAxis(maximumAngle, engineRotationAxis), targetRotation);

        float targetRotationDuration = Quaternion.Angle(targetQuaternionRotation, enginePoint.localRotation) / rotationSpeed;

        enginePoint.localRotation = Quaternion.Lerp(enginePoint.localRotation, targetQuaternionRotation, Mathf.InverseLerp(0, targetRotationDuration, Time.deltaTime));
    }
    public Vector3 GetEngineAcceleration()
    {
        float engineGroundedPower = GetEngineGroundedPower();

        return enginePoint.forward * CurrentAcceleration * Mathf.Lerp(minMaxInfluence.x, minMaxInfluence.y, engineGroundedPower);
    }

    public float GetEngineGroundedPower() => Mathf.InverseLerp(minMaxYEngineToWork.x, minMaxYEngineToWork.y, enginePoint.position.y);

    internal void DropAcceleration()
    {
        CurrentAcceleration -= CurrentAcceleration * speedDropPercent;
    }
}

[System.Serializable]
public class DragForce
{
    [Header("Drag")]
    [SerializeField] private Vector2 minMaxDrag;
    [SerializeField] private Vector2 minMaxDragUnderWater;
    [SerializeField] private Vector2 minMaxAngularDrag;

    [SerializeField] private Vector2 minMaxDragSpeed;

    public void SetupDrag(Rigidbody rb, float volumeUnderWater)
    {
        float dragSpeed = Mathf.InverseLerp(minMaxDragSpeed.x, minMaxDragSpeed.y, rb.velocity.magnitude);
        rb.drag = Mathf.Lerp(minMaxDrag.x, minMaxDrag.y, dragSpeed) + Mathf.Lerp(minMaxDragUnderWater.x, minMaxDragUnderWater.y, volumeUnderWater);
        rb.angularDrag = Mathf.Lerp(minMaxAngularDrag.x, minMaxAngularDrag.y, dragSpeed);
    }
}

[System.Serializable]
public class ArchimedForce
{
    [Header("Archimed")]
    [SerializeField] private Vector2 minMaxArchimedForce;
    [SerializeField] private Vector2 minMaxYPosition;
    [SerializeField] protected AnimationCurve archimedForceCurve;

    [SerializeField] private Vector2 minMaxSpeedForArchimed;
    [SerializeField] private Vector2 minMaxArchimedSpeedForce;

    [Header("Sinusoida")]
    [SerializeField] private float maxSin = .2f;
    [SerializeField] private float frequency = 1; //(1 = pi(~1.6))


    public Vector3 GetArchimedForce(float volumeUnderWater, float speed, out Vector3 forceLocalPosition)
    {
        float sinInf = SinInfluence();

        float forceUnderWater = Mathf.Lerp(minMaxArchimedForce.x, minMaxArchimedForce.y, archimedForceCurve.Evaluate(Mathf.Clamp01(volumeUnderWater - sinInf)));
        float highSpeedForce = Mathf.Lerp(minMaxArchimedSpeedForce.x, minMaxArchimedSpeedForce.y, Mathf.InverseLerp(minMaxSpeedForArchimed.x, minMaxSpeedForArchimed.y, speed));

        return (forceUnderWater + highSpeedForce) * GetWhaterNormalVector(out forceLocalPosition);
    }

    public float GetVolumeUnderWater(Transform transform)
    {
        return Mathf.InverseLerp(minMaxYPosition.x, minMaxYPosition.y, transform.position.y);
    }

    private float SinInfluence() => Extensions.Sinusoida(maxSin / 2, frequency, Time.time, 0, maxSin / 2);
    private Vector3 GetWhaterNormalVector(out Vector3 applyLocalPosition)
    {
        applyLocalPosition = Vector3.zero;
        return Vector3.up;
    }
}