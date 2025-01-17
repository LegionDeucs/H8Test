using UnityEngine;

public class JetSkiFeelController : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    [SerializeField] private Transform boatVisual;
    [SerializeField] private Transform engineTransform;

    [SerializeField] private Quaternion rotationToLeft;
    [SerializeField] private Quaternion rotationToRight;

    [SerializeField] float maxReactAngle;
    [SerializeField] float rotationSpeed = 360;

    [SerializeField] private Quaternion lowSpeedRotation;
    [SerializeField] private Quaternion highSpeedRotation;

    [SerializeField] private Transform bodyIKTarget;

    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 turnLeftPosition;
    [SerializeField] private Vector3 turnRightPosition;

    [SerializeField] private float targetIKChangeSpeed = 10;


    [SerializeField] private Vector2 minMaxSpeed;

    [Header("VFX")]
    [SerializeField] private ParticleSystem leftEngineParticle;
    [SerializeField] private ParticleSystem rightEngineParticle;

    [SerializeField] private ParticleSystem backEngineParticle;

    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private Vector2 minMaxScale;
    [SerializeField] private Vector2 minMaxLifetime;

    [SerializeField] private Vector2 moveSpeedAfector;
    [SerializeField] private AnimationCurve moveSpeedAfectorCurve;

    [SerializeField] private Vector2 minMaxAcceleration;
    [SerializeField] private Vector2 minMaxParticleSpeed;
    [SerializeField] private AnimationCurve backEngineSpeedCurve;

    private ParticleSystem.MainModule leftEngineParticleMainModule;
    private ParticleSystem.MainModule rightEngineParticleMainModule;
    private ParticleSystem.MainModule backEngineParticleMainModule;

    private ParticleSystem.MinMaxCurve baseStartSize;

    void Start()
    {
        leftEngineParticleMainModule = leftEngineParticle.main;
        rightEngineParticleMainModule = rightEngineParticle.main;
        backEngineParticleMainModule = backEngineParticle.main;
        baseStartSize = rightEngineParticle.main.startSize;
    }

    private void Update()
    {
        float targetRotationPower = GetTargetRotationPower();

        float speedPower = Mathf.InverseLerp(minMaxSpeed.x, minMaxSpeed.y, movementController.RB.velocity.magnitude);
        float moveSpeed = movementController.RB.velocity.magnitude;
        SetupBoatRotation(targetRotationPower, moveSpeed);
        SetupBodyIKPosition(targetRotationPower, moveSpeed);

        var frameSize = new ParticleSystem.MinMaxCurve(
            baseStartSize.constantMin * scaleCurve.Evaluate(movementController.EngineUnderWaterPower) * Mathf.Lerp(minMaxLifetime.x, minMaxLifetime.y, speedPower),
            baseStartSize.constantMax * scaleCurve.Evaluate(movementController.EngineUnderWaterPower) * Mathf.Lerp(minMaxLifetime.x, minMaxLifetime.y, speedPower));

        leftEngineParticleMainModule.startLifetime = Mathf.Lerp(minMaxLifetime.x, minMaxLifetime.y, speedPower);
        leftEngineParticleMainModule.startSize = frameSize;

        rightEngineParticleMainModule.startLifetime = Mathf.Lerp(minMaxLifetime.x, minMaxLifetime.y, speedPower);
        rightEngineParticleMainModule.startSize = frameSize;
        backEngineParticleMainModule.startSpeed = Mathf.Lerp(minMaxParticleSpeed.x, minMaxParticleSpeed.y,
            backEngineSpeedCurve.Evaluate(Mathf.InverseLerp(minMaxAcceleration.x, minMaxAcceleration.y, movementController.CurrentAcceleration)));
    }

    private void SetupBoatRotation(float targetRotationPower, float moveSpeed)
    {
        Quaternion thisFrameRotation = GetTurningRotation(targetRotationPower) * GetSpeedRotation();

        float rotationDuration = Quaternion.Angle(boatVisual.localRotation, thisFrameRotation) / rotationSpeed;
        boatVisual.localRotation = Quaternion.Lerp(boatVisual.localRotation, thisFrameRotation, Mathf.InverseLerp(0, rotationDuration,
            Time.deltaTime * moveSpeedAfectorCurve.Evaluate(Mathf.InverseLerp(moveSpeedAfector.x, moveSpeedAfector.y, moveSpeed))));
    }


    private Quaternion GetTurningRotation(float targetRotationPower)
    {
        Quaternion targetRotation = Quaternion.Lerp(rotationToLeft, rotationToRight, targetRotationPower);

        return targetRotation;
    }

    private float GetTargetRotationPower()
    {
        float sign = Vector3.Dot(transform.right, movementController.RB.velocity) < 0 ? 1 : -1;
        float reactAngle = sign * Quaternion.Angle(Quaternion.LookRotation(movementController.RB.velocity), Quaternion.LookRotation(transform.forward));

        return Mathf.InverseLerp(-maxReactAngle, maxReactAngle, reactAngle);
    }

    private Quaternion GetSpeedRotation()
    {
        return Quaternion.Lerp(lowSpeedRotation, highSpeedRotation, Mathf.InverseLerp(minMaxSpeed.x, minMaxSpeed.y, movementController.RB.velocity.magnitude));
    }

    private void SetupBodyIKPosition(float targetRotationPower, float moveSpeed)
    {
        Vector3 newTargetPositoin = Extensions.Lerp(turnLeftPosition, turnRightPosition, targetRotationPower, Vector3.Lerp,
            new InterpolationExtremumPoints<Vector3>() { extremum = .5f, point = centerPosition });
        float transitionDuration = (newTargetPositoin - bodyIKTarget.localPosition).magnitude / targetIKChangeSpeed;
        bodyIKTarget.localPosition = Vector3.Lerp(bodyIKTarget.localPosition, newTargetPositoin, 
            Time.deltaTime * moveSpeedAfectorCurve.Evaluate(Mathf.InverseLerp(moveSpeedAfector.x, moveSpeedAfector.y, moveSpeed)));
    }
}
