﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public bool isFed = false;

    public void feed()
    {
        isFed = true;
    }

    public void unFeed()
    {
        isFed = false;
    }

    public bool checkStatus()
    {
        return isFed;
    }
}
