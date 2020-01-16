using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekAndFlee : MonoBehaviour
{
    //Define max speed for the AI to move
    public float maxSpeed = 10f;
    // float that slowly increments LERP value over frames
    float lerpCount = 0f;
    // Distance over which to accelerate when starting
    public float accelerationDistance = 5f;
    // Define radius at which an arriving AI will stop
    public float slowRadius = 4f;
    public float stopRadius = 1f;
    // Define Time at which an arriving AI will attempt to arrive at the target
    public float arriveTimeToTarget = 0.25f;
    // Define the maximum rotation per frame allowed for a wandering AI
    public float maxWanderRotation = 5f;
    // Defines the radius around the player in which the AI will become active
    public float ActivationRange = 20f;
    // Whenever a Lerp is called, this float is given a timestamp;
    // Determine whether the object seeks or flees from the player
    public bool seek = true;
    public bool arrive = false;
    public bool wander = false;
    //This bool will keep track of when  the object is accelerating
    private bool isAccelerating = true;
    
    // Transform of player and rigidbody of AI
    public Transform playerTrans;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // grab the rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Solve for the magnitude of distance between the player and AI
        float dist = Vector3.Distance(transform.position, playerTrans.position);
        Vector3 aiVelocity = new Vector3();
        // thing to note. In order for th AI to work, everything except the algorithm desired must be deselected. If nothing is selected, Flee will be used
        // if its in range and seek is activated, have the AI seek the player
        if (seek && !arrive && !wander && (dist < ActivationRange))
        {
            // Determine based on algorithm type what the aiVelocity will be
            // seek uses player pos - ai pos
            aiVelocity = playerTrans.position - transform.position;
            AIKinematicMove(aiVelocity);
        }
        else if (arrive && !seek && !wander && (dist < ActivationRange))
        {
            aiVelocity = playerTrans.position - transform.position;
            AIKinematicArrive(aiVelocity, dist);
        }
        else if (wander && !arrive && !seek)
        {
            AIKinematicWander();
        }
        // otherwise if its in range flee from the player
        else if (!seek && !arrive && !wander && (dist < ActivationRange))
        {
            // Determine based on algorithm type what the aiVelocity will be
            // seek uses ai pos - player pos
            aiVelocity = transform.position - playerTrans.position;
            AIKinematicMove(aiVelocity);
        }
        else
        {
          //  Debug.Log("AI inactive");
        }

    }

    // recalculate the velocity vector before reassigning
    // Determine rotation based on vector
    void AIKinematicMove(Vector3 aiVel)
    {  /* Old Factors for non-smoothed motion
        // Normalize and apply magnitude of Max Speed
        aiVel.Normalize();
        
        // Multiply by Time.deltaTime to allow for consistency in variable frame rates
        aiVel *= (maxSpeed * Time.deltaTime);

        // assign the new velocity vector to the rigidbody velocity component
        rb.velocity = aiVel;
        */

        // Changed factor to allow for smoothing
        // Note due to velocity magnitude, effects of Lerp were either very slow or very quick in relative smaller environments
        
        aiVel = Vector3.Lerp(aiVel.normalized, aiVel.normalized * maxSpeed * Time.deltaTime, lerpCount/accelerationDistance);
        if (lerpCount < accelerationDistance && isAccelerating)
        {
            lerpCount += 0.1f;
            
        } else
        {
            isAccelerating = false;
        }
        Debug.Log(lerpCount);
        rb.velocity = aiVel;
        // transform.LookAt(aiVel);
        // Create a new orientation vector based off the AI velocity (arguments for step and max rotation left at 2pi rad because no specific step is needed currently)
        Vector3 orientation = Vector3.RotateTowards(transform.forward, aiVel, 2 * Mathf.PI, 2 * Mathf.PI);
        // assign that orientation value to the transform
        transform.rotation = Quaternion.LookRotation(orientation);
    }

    void AIKinematicArrive(Vector3 aiVel, float dist)
    {
        
        
        // Alot of drifting towards target after stop occurs. This function acts as a hard break on the AI
        if (dist < slowRadius)
        {
            if (!isAccelerating)
            {
                lerpCount = 0f;
                isAccelerating = true;
            }
            //Note Slow radius Cannot be the same as stop radius (divide by zero error)
            aiVel = Vector3.Lerp(aiVel, Vector3.zero, lerpCount/(slowRadius-stopRadius));
            if (lerpCount < (slowRadius-stopRadius) && isAccelerating)
            {
                lerpCount += 0.1f;
                Debug.Log(lerpCount);
            }
            else
            {
            //  isAccelerating = false;
            }
            rb.velocity = aiVel;
            return;

        }
        // assign the new velocity vector to the rigidbody velocity component (with smoothing)
        aiVel = Vector3.Lerp(aiVel.normalized, aiVel.normalized * maxSpeed * Time.deltaTime, lerpCount / accelerationDistance);
        // Check if the object is accelerating, if it is, then increase LERP Counter
        if (lerpCount < accelerationDistance && isAccelerating)
        {
            lerpCount += 0.1f;
        }
        // when its reached its max velocity turn off isAccelerating so it doesnt increase the lerpCounter
        else
        {

            isAccelerating = false;
        }
        rb.velocity = aiVel;
     
        // Create a new orientation vector based off the AI velocity (arguments for step and max rotation left at 2pi rad because no specific step is needed currently)
        Vector3 orientation = Vector3.RotateTowards(transform.forward, aiVel, 2 * Mathf.PI, 2 * Mathf.PI);
        // assign that orientation value to the transform
        transform.rotation = Quaternion.LookRotation(orientation);
    }

    void AIKinematicWander()
    {
        float orientX = Mathf.Sin(transform.rotation.y * Mathf.Deg2Rad);
        float orientZ = Mathf.Cos(transform.rotation.y * Mathf.Deg2Rad);
        Vector3 aiVel = new Vector3(orientX, 0, orientZ);
        aiVel *= maxSpeed * Time.deltaTime;
        rb.velocity = aiVel;

        transform.Rotate(transform.rotation.x, RandomBinomial() * maxWanderRotation, transform.rotation.z);
    }

    float RandomBinomial()
    {
        return (Random.value - Random.value);
    }
}
