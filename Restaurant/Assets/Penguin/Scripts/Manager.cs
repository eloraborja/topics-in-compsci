using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static List<GameObject> babyList;

    public Baby babyPrefab;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        createBabies(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createBabies(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate<GameObject>(babyPrefab.gameObject);
            if(counter == 0)
            {
                temp.tag = "baby";
                counter = 1;
            } else
            {
                temp.tag = "Baby";
                counter = 0;
            }
            babyList.Add(temp);
        }
    }

    public static List<GameObject> listReturn()
    {
        return babyList;
    }
}
