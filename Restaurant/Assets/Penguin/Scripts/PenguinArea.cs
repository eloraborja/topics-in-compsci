﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class PenguinArea : MonoBehaviour
{
    [Tooltip("The agent inside the area")]
    public PenguinAgent penguinAgent;

    public PenguinAgent penguinAgent2;

    //public static List<GameObject> babies = Manager.babyList;

    public List<GameObject> babies;

    public static List<GameObject> penguins = Manager.penguinList;

    [Tooltip("The TextMeshPro text that shows the cumulative reward of the agent")]
    public TextMeshPro cumulativeRewardText;

    [Tooltip("Prefab of a live fish")]
    public Fish fishPrefab;

    private List<GameObject> fishList;

    /// <summary>
    /// Reset the area, including fish and penguin placement
    /// </summary>
    public void ResetArea()
    {
        RemoveAllFish();
        //PlacePenguins();
        //PlaceBabies();
        SpawnFish(12, .5f);

        penguinAgent.transform.SetParent(transform);
        penguinAgent.transform.localPosition = new Vector3(-1.3f, 0.05f, -1.1f);

        penguinAgent2.transform.SetParent(transform);
        penguinAgent2.transform.localPosition = new Vector3(-2.65f, 0.05f, -1.1f);

    }

    /// <summary>
    /// Remove a specific fish from the area when it is eaten
    /// </summary>
    /// <param name="fishObject">The fish to remove</param>
    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    /// <summary>
    /// The number of fish remaining
    /// </summary>
    public int FishRemaining
    {
        get { return fishList.Count; }
    }

    /// <summary>
    /// Choose a random position on the X-Z plane within a partial donut shape
    /// </summary>
    /// <param name="center">The center of the donut</param>
    /// <param name="minAngle">Minimum angle of the wedge</param>
    /// <param name="maxAngle">Maximum angle of the wedge</param>
    /// <param name="minRadius">Minimum distance from the center</param>
    /// <param name="maxRadius">Maximum distance from the center</param>
    /// <returns>A position falling within the specified region</returns>
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // Pick a random radius
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // Pick a random angle
            angle = UnityEngine.Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    /// <summary>
    /// Remove all fish from the area
    /// </summary>
    private void RemoveAllFish()
    {
        if (fishList != null)
        {
            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishList[i] != null)
                {
                    Destroy(fishList[i]);
                }
            }
        }

        fishList = new List<GameObject>();
    }

    /// <summary>
    /// Place the penguin in the area
    /// </summary>
    private void PlacePenguin()
    {
        Rigidbody rigidbody = penguinAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        penguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void PlacePenguins()
    {
        for (int i = 0; i < penguins.Capacity; i++)
        {
            Rigidbody rigidbody = penguins[i].GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            penguins[i].transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
            penguins[i].transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        }
    }

    /// <summary>
    /// Place the baby in the area
    /// </summary>
    /*private void PlaceBaby()
    {
        Rigidbody rigidbody = penguinBaby.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }*/

    private void PlaceBabies()
    {
        for (int i = 0; i < babies.Capacity; i++)
        {
            Rigidbody rigidbody = babies[i].GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            babies[i].transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
            babies[i].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    /// <summary>
    /// Spawn some number of fish in the area and set their swim speed
    /// </summary>
    /// <param name="count">The number to spawn</param>
    /// <param name="fishSpeed">The swim speed</param>
    private void SpawnFish(int count, float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the fish
            GameObject fishObject = Instantiate<GameObject>(fishPrefab.gameObject);
            //fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            //fishObject.transform.position = new Vector3(-59f, 0.09f, -(i*1.5f));
            //fishObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            // Set the fish's parent to this area's transform
            fishObject.transform.SetParent(transform);
            //fishObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            fishObject.transform.localPosition = new Vector3(i * 0.08f + (-2f), 0.05f, 1f);

            // Keep track of the fish
            fishList.Add(fishObject);

            // Set the fish speed
            fishObject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        ResetArea();
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        if (penguinAgent.transform.position.y <= -5)
        {
            penguinAgent.transform.SetParent(transform);
            penguinAgent.transform.localPosition = new Vector3(-1.3f, 0.05f, -1.1f);
        }
        if (penguinAgent2.transform.position.y <= -5)
        {
            penguinAgent2.transform.SetParent(transform);
            penguinAgent2.transform.localPosition = new Vector3(-2.65f, 0.05f, -1.1f);
        }
    }
}
