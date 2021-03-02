using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DateTime dt = new DateTime(2020, 10, 1);
        TimeSpan ts = DateTime.UtcNow - dt;
        Debug.Log(ts.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
