using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class NPCController : MonoBehaviour
{
    [SerializeField] private EnenemyMovementController enenemyMovementController;
    [SerializeField] private SplineContainer splineContainer;

    [SerializeField] private float displaceDistance;

    private float percentDisplacement;
    private void Start()
    {
        percentDisplacement = displaceDistance / splineContainer.CalculateLength();
    }

    private void Update()
    {
        SplineUtility.GetNearestPoint(splineContainer.Spline, enenemyMovementController.transform.position, out float3 nearest, out float t);
        enenemyMovementController.SetAimPoint(splineContainer.EvaluatePosition(Mathf.Repeat(t + percentDisplacement, 1)));
    }
}
