using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestIEnumerable : MonoBehaviour
{
    public class TestData { }

    private List<TestData> listTestData = new List<TestData>();
    // Start is called before the first frame update
    void Start()
    {
        listTestData.Add(new TestData());
    }

    private void OnTest1()
    {
        UnityEngine.Profiling.Profiler.BeginSample("foreach IEnumerable");
        foreach (TestData td in GetTestData()) { }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void OnTest2()
    {
        UnityEngine.Profiling.Profiler.BeginSample("foreach no IEnumerable");
        foreach (TestData td in listTestData) { }
        UnityEngine.Profiling.Profiler.EndSample();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            OnTest1();

        if (Input.GetKeyUp(KeyCode.B))
            OnTest2();
    }

    private IEnumerable<TestData> GetTestData()
    {
        foreach (TestData td in listTestData)
        {
            yield return td;
        }
    }

}
