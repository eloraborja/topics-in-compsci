using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static List<GameObject> babyList = new List<GameObject>();
    public static List<GameObject> penguinList = new List<GameObject>();
    private static List<float> times = new List<float>();

    public Baby babyPrefab;
    public PenguinAgent penguinPrefab;

    //private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        times.Clear();
        //createBabies(4);
    }

    public void createBabies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            babyList.Add((GameObject)Instantiate<GameObject>(babyPrefab.gameObject));
            babyList[i].tag = "baby" + i;
        }
        return;
    }

    public static List<GameObject> GetList()
    {
        return babyList;
    }

    /*public void createPenguins(int count)
    {
        for (int i = 0; i < count; i++)
        {
            penguinList.Add((GameObject)Instantiate<GameObject>(penguinPrefab.gameObject));
        }
        return;
    }*/

    public static List<GameObject> GetPenguinList()
    {
        return penguinList;
    }

    public static void addTime(float t)
    {
        times.Add(t);
    }

    private float average(List<float> ti)
    {
        float temp = 0.0f;
        for(int i = 0; i < ti.Count; i++)
        {
            temp += ti[i];
        }
        return temp / ti.Count;
    }

    void Update()
    {
        //Debug.Log(average(times));
    }
}
