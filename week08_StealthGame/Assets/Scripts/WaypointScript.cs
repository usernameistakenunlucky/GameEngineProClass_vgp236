using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    [SerializeField] private float gizmoRadius = 1.0f;
    [SerializeField] private Color gizmoColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);                                                                                                                                                                                                                                 
    }



}
