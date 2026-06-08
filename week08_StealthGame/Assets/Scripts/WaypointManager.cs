using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private List<WaypointScript> _waypoints = new List<WaypointScript>();

    static WaypointManager instance;

    public static WaypointManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (_waypoints.Count == 0)
            {
                _waypoints = transform.GetComponentsInChildren<WaypointScript>().ToList();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public WaypointScript GetWaypoint(int index)
    {
        if (index < _waypoints.Count)
        {
            return _waypoints[index];
        }
        return null;
    }

    public WaypointScript GetRandomWaypoint()
    {
        if (_waypoints.Count == 0)
        {
            return null;
        }
        int randIndex = Random.Range(0, _waypoints.Count);
        return _waypoints[randIndex];
    }

    public WaypointScript GetClosestWaypoint(Vector3 position)
    {
        if (_waypoints.Count == 0)
        {
            return null;
        }

        WaypointScript closestWaypoint = null;
        float closestDistanceSq = float.MaxValue;
        foreach (WaypointScript waypoint in _waypoints)
        {
            float distSQ = Vector3.SqrMagnitude(waypoint.transform.position - position);
            if (distSQ < closestDistanceSq)
            {
                closestDistanceSq = distSQ;
                closestWaypoint = waypoint;

            }
        }
        return closestWaypoint;
    }

    public WaypointScript GetRandomWaypointInRange(Vector3 position, float range)
    {
        float distSQ = range * range;
        List<WaypointScript> inRangeWaypoints = new List<WaypointScript>();
        foreach (WaypointScript waypoint in _waypoints)
        {
            if (Vector3.SqrMagnitude(waypoint.transform.position - position) <= distSQ)
            {
                inRangeWaypoints.Add(waypoint);
            }
        }

        if (inRangeWaypoints.Count > 0)
        {
            int randIndex = Random.Range(0, inRangeWaypoints.Count);
            return inRangeWaypoints[randIndex];
        }
        return null;

    }


}
