﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Diagnostics;

public class PenguinAgent : Agent
{
    [Tooltip("How fast the agent moves forward")]
    public float moveSpeed = 5f;

    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;

    [Tooltip("Prefab of the heart that appears when the baby is fed")]
    public GameObject heartPrefab;

    [Tooltip("Prefab of the regurgitated fish that appears when the baby is fed")]
    public GameObject regurgitatedFishPrefab;

    public PenguinAgent p1;

    public GameObject kitchen;

    public Stopwatch timer;

    public List<string> babyTags = new List<string>();
    private PenguinArea penguinArea;
    new private Rigidbody rigidbody;
    private GameObject baby;
    public List<GameObject> babies = new List<GameObject>();
    private bool isFull; // If true, penguin has a full stomach
    private int index = 0;

    /// <summary>
    /// Initial setup, called when the agent is enabled
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        penguinArea = GetComponentInParent<PenguinArea>();
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Perform actions based on a vector of numbers
    /// </summary>
    /// <param name="actionBuffers">The struct of actions to take</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Convert the first action to forward movement
        float forwardAmount = actionBuffers.DiscreteActions[0];

        // Convert the second action to turning left or right
        float turnAmount = 0f;
        if (actionBuffers.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actionBuffers.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        // Apply movement
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        // Apply a tiny negative reward every step to encourage action
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    /// <summary>
    /// Read inputs from the keyboard and convert them to a list of actions.
    /// This is called only when the player wants to control the agent and has set
    /// Behavior Type to "Heuristic Only" in the Behavior Parameters inspector.
    /// </summary>
    /// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAction = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAction = 2;
        }

        // Put the actions into the array
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }

    /// <summary>
    /// When a new episode begins, reset the agent and area
    /// </summary>
    public override void OnEpisodeBegin()
    {
        timer = new Stopwatch();
        timer.Start();
        moveSpeed = 15f;
        turnSpeed = 180f;
        isFull = false;
        penguinArea.ResetArea();
        index = changeBaby(-1);
        //babies = Manager.GetList();
        baby = babies[index];

    }

    /// <summary>
    /// Collect all non-Raycast observations
    /// </summary>
    /// <param name="sensor">The vector sensor to add observations to</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        // Whether the penguin has eaten a fish (1 float = 1 value)
        sensor.AddObservation(isFull);

        // Distance to the baby (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(baby.transform.position, transform.position));

        // Direction to baby (1 Vector3 = 3 values)
        sensor.AddObservation((baby.transform.position - transform.position).normalized);

        // Direction penguin is facing (1 Vector3 = 3 values)
        sensor.AddObservation(transform.forward);

        // Distance to the other penguin (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(kitchen.transform.position, transform.position));

        // Direction to other penguin (1 Vector3 = 3 values)
        sensor.AddObservation((kitchen.transform.position - transform.position).normalized);

        // Distance to the other penguin (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(p1.transform.position, transform.position));

        // Direction to other penguin (1 Vector3 = 3 values)
        sensor.AddObservation((p1.transform.position - transform.position).normalized);
    }

    /// <summary>
    /// When the agent collides with something, take action
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
        {
            // Try to eat the fish
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag(baby.tag))
        {
            // Try to feed the baby
            RegurgitateFish();
        }
        else if (collision.transform.CompareTag("penguin"))
        {
            AddReward(-0.5f);
        }
    }

    /// <summary>
    /// Check if agent is full, if not, eat the fish and get a reward
    /// </summary>
    /// <param name="fishObject">The fish to eat</param>
    private void EatFish(GameObject fishObject)
    {
        if (isFull) return; // Can't eat another fish while full
        isFull = true;

        penguinArea.RemoveSpecificFish(fishObject);

        AddReward(1f);
    }

    /// <summary>
    /// Check if agent is full, if yes, feed the baby
    /// </summary>
    private void RegurgitateFish()
    {
        if (!isFull) return; // Nothing to regurgitate
        isFull = false;

        // Spawn regurgitated fish
        //GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
        //regurgitatedFish.transform.parent = transform.parent;
        //regurgitatedFish.transform.position = baby.transform.position;
        //Destroy(regurgitatedFish, 4f);

        // Spawn heart
        //GameObject heart = Instantiate<GameObject>(heartPrefab);
        //heart.transform.parent = transform.parent;
        //heart.transform.position = baby.transform.position + Vector3.up;
        //Destroy(heart, 4f);

        AddReward(1f);

        baby.GetComponent<Baby>().feed();

        index = changeBaby(index);

        if (index == 99)
        {
            moveSpeed = 0f;
            turnSpeed = 0f;
        }

        /*if (penguinArea.FishRemaining <= 0)
        {
            index = 0;
            //EndEpisode();
        }*/

        if (index != 99)
        {
            baby = babies[index];
        }

    }

    private int changeBaby(int index)
    {
        for (int i = index + 1; i < babies.Capacity; i++)
        {
            if (babyTags.Contains(babies[i].tag))
            {
                return i;
            }
        }
        return 99;
    }

    public void setTags(string t)
    {
        babyTags.Add(t);
    }

    void Update()
    {
        int babiesFed = 0;
        for (int i = 0; i < babies.Capacity; i++)
        {
            if (babies[i].GetComponent<Baby>().checkStatus() == true)
            {
                babiesFed++;
            }
        }
        if (babiesFed == babies.Capacity)
        {
            for (int i = 0; i < babies.Capacity; i++)
            {
                babies[i].GetComponent<Baby>().unFeed();
            }
            timer.Stop();
            Manager.addTime(timer.ElapsedMilliseconds);
            p1.EndEpisode();
            EndEpisode();
        }
    }

}
