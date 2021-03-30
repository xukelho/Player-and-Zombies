using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Fields

    public event EventHandler<Projectile> Exploded;

    public int DamageToDeal = 20;

    public float FlightTime = 5f;

    public Rigidbody Rigidbody;
    public GameObject Barrel;

    private Vector3 _startingPosition;

    private float _parabolaMaxHeight = 3;

    private bool _hasExploded = false;

    #endregion Fields

    #region Unity

    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        LiveBeing being = collision.gameObject.GetComponent<LiveBeing>();

        if (being != null)
        {
            being.TakeDamage(DamageToDeal);
            _hasExploded = true;
        }
    }

    #endregion Unity

    #region Methods

    public void Fire(Vector3 targetLocation)
    {
        _hasExploded = false;
        StartCoroutine(MoveProjectileInAnArc(targetLocation));
    }

    /// <summary>
    /// Get position from a parabola defined by start and end, height, and time
    /// </summary>
    /// <param name='start'>
    /// The start point of the parabola
    /// </param>
    /// <param name='end'>
    /// The end point of the parabola
    /// </param>
    /// <param name='height'>
    /// The height of the parabola at its maximum
    /// </param>
    /// <param name='t'>
    /// Normalized time (0->1)
    /// </param>S
    private Vector3 CalculateParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }

    private IEnumerator MoveProjectileInAnArc(Vector3 targetLocation)
    {
        float startingTime = Time.realtimeSinceStartup;
        Vector3 margin = new Vector3(.05f, .05f, .05f);
        float currentStepInParabola = 0;

        while (currentStepInParabola < 1 && !_hasExploded)
        {
            yield return 0;

            float flightCurrentDuration = Time.realtimeSinceStartup - startingTime;

            currentStepInParabola = flightCurrentDuration / FlightTime;

            Vector3 currentPosition = CalculateParabola(_startingPosition, targetLocation, _parabolaMaxHeight, currentStepInParabola);
            transform.position = currentPosition;
        }

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        Exploded(this, this);
    }

    #endregion Methods
}