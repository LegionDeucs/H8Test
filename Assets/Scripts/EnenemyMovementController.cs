using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenemyMovementController : MovementController
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Engine engine;
    [SerializeField] private DragForce dragForce;
    [SerializeField] private ArchimedForce archimedForce;

    [SerializeField] private float centerDot = .2f;

    public override Rigidbody RB => rb;

    public override float EngineUnderWaterPower => engine.GetEngineGroundedPower();

    public override float CurrentAcceleration => engine.CurrentAcceleration;

    protected float currentAcceleration;

    private Vector3 target;
    public void SetAimPoint(Vector3 target)
    {
        this.target = target;
        float dot = Vector3.Dot(transform.right, (target - transform.position).normalized);
        if (dot > centerDot)
            engine.RotateEngine(0);
        else if (dot < -centerDot)
            engine.RotateEngine(1);
        else
            engine.RotateEngine(.5f);

        engine.DoUpdate();
    }

    private void FixedUpdate()
    {
        float volumeUnderWater = archimedForce.GetVolumeUnderWater(transform);
        dragForce.SetupDrag(rb, volumeUnderWater);

        rb.AddForceAtPosition(engine.GetEngineAcceleration(), engine.EnginePoint.position, ForceMode.Acceleration);
        var archimedForceV = archimedForce.GetArchimedForce(volumeUnderWater, rb.velocity.magnitude, out Vector3 forceLocalPosition);

        rb.AddForceAtPosition(archimedForceV, forceLocalPosition + transform.position, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity * 5);

        Gizmos.DrawSphere(target, .5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        engine.DropAcceleration();
    }
}
