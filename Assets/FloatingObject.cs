using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ArchimedForce archimedForce;
    [SerializeField] private DragForce dragForce;

    private void FixedUpdate()
    {
        float volumeUnderWater = archimedForce.GetVolumeUnderWater(transform);
        dragForce.SetupDrag(rb, volumeUnderWater);

        var archimedForceV = archimedForce.GetArchimedForce(volumeUnderWater, rb.velocity.magnitude, out Vector3 forceLocalPosition);
        rb.AddForceAtPosition(archimedForceV, forceLocalPosition + transform.position, ForceMode.Acceleration);
    }
}
