using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private JetSkiMovementController movementController;
    [SerializeField] private Transform target;
    [SerializeField] private float staticYPosition = 1;


    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform lowSpeedPosition;
    [SerializeField] private Transform highSpeedPosition;
    [SerializeField] private AnimationCurve cameraReactToSpeedCurve;

    [SerializeField] private Vector2 minMaxSpeed;
    [SerializeField] private float localPositionChangeSpeed = 2;
    [SerializeField] private float localRotationChangeSpeed = 180;

    [Space]
    [SerializeField] private Vector2 minMaxDistance;
    [SerializeField] private Vector2 minMaxPositionLerpStrenght;
    [SerializeField] private AnimationCurve positoinLerpCurve;

    [Space]
    [SerializeField] private Vector2 minMaxRotationAngle;
    [SerializeField] private Vector2 minMaxRotationLerpStrenght;
    [SerializeField] private AnimationCurve rotationLerpCurve;

    void LateUpdate()
    {
        float distance = (transform.position - target.position.WithY(staticYPosition)).magnitude;
        float angle = Quaternion.Angle(transform.rotation, target.rotation);

        float mainPositionLerpStrenght = Mathf.Lerp(minMaxPositionLerpStrenght.x, minMaxPositionLerpStrenght.y, 
                                positoinLerpCurve.Evaluate(Mathf.InverseLerp(minMaxDistance.x, minMaxDistance.y, distance)));

        float rotationLerpStrenght = Mathf.Lerp(minMaxRotationLerpStrenght.x, minMaxRotationLerpStrenght.y,
                                rotationLerpCurve.Evaluate(Mathf.InverseLerp(minMaxRotationAngle.x, minMaxRotationAngle.y, angle)));

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, target.position.WithY(staticYPosition), mainPositionLerpStrenght * Time.deltaTime), 
                                         Quaternion.Lerp(transform.rotation, target.rotation, rotationLerpStrenght * Time.deltaTime));
        Vector3 targetLocalPositin = Vector3.Lerp(lowSpeedPosition.localPosition, highSpeedPosition.localPosition,
            cameraReactToSpeedCurve.Evaluate(Mathf.InverseLerp(minMaxSpeed.x, minMaxSpeed.y, movementController.RB.velocity.magnitude)));

        Quaternion targetLocalRotation = Quaternion.Lerp(lowSpeedPosition.localRotation, highSpeedPosition.localRotation,
            cameraReactToSpeedCurve.Evaluate(Mathf.InverseLerp(minMaxSpeed.x, minMaxSpeed.y, movementController.RB.velocity.magnitude)));

        float moveDuraiton = (targetLocalPositin - mainCamera.localPosition).magnitude / localPositionChangeSpeed;
        float rotationDuraiton = Quaternion.Angle(targetLocalRotation, mainCamera.localRotation) / localRotationChangeSpeed;

        float applied = Mathf.InverseLerp(0, moveDuraiton, Time.deltaTime);
        float rotationApplied = Mathf.InverseLerp(0, rotationDuraiton, Time.deltaTime);

        mainCamera.SetLocalPositionAndRotation(Vector3.Lerp(mainCamera.localPosition, targetLocalPositin, applied), 
                                               Quaternion.Lerp(mainCamera.localRotation, targetLocalRotation, applied));
        transform.eulerAngles = transform.eulerAngles.WithZ(0);
    }
}
