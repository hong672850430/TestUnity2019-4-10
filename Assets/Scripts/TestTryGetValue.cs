using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTryGetValue : MonoBehaviour
{
    public class TestData
    {
        public int iTest = 0;
        public string strTest = "name";

        public TestData() { }

        public TestData(int i, string str)
        {
            iTest = i;
            strTest = str;
        }
        public void Print()
        {
            Debug.Log($"iTest {iTest},strTest {strTest}");
        }
    }

    public enum ETest
    {
        Test1,
        Test2,
        Test3,
    }
    public struct IntVector2 : IEquatable<IntVector2>
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(IntVector2 other)
        {
            return other.x == x && other.y == y;
        }
    }

    private Dictionary<ETest, TestData> dictTestData = new Dictionary<ETest, TestData>();
    private Dictionary<IntVector2, TestData> dictTestData1 = new Dictionary<IntVector2, TestData>();

    private IntVector2 iv;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < (int)ETest.Test3; ++i)
        {
            dictTestData.Add((ETest)i, new TestData(i, "name" + i));

            dictTestData1.Add(new IntVector2(i, i), new TestData(i, "name" + i));
        }
        iv = new IntVector2(1, 1);
    }

    private void OnTest1()
    {
        UnityEngine.Profiling.Profiler.BeginSample("Dictionary-TryGetValue1");
        if (dictTestData.TryGetValue(ETest.Test1, out TestData data1))
        {
            UnityEngine.Profiling.Profiler.EndSample();
            OnTest2(data1);
        }

        UnityEngine.Profiling.Profiler.BeginSample("Dictionary-ContainsKey2");
        if (dictTestData.ContainsKey(ETest.Test2))
        {
            UnityEngine.Profiling.Profiler.EndSample();
            OnTest2(dictTestData[ETest.Test2]);
        }

        //UnityEngine.Profiling.Profiler.BeginSample("Dictionary-TryGetValue1");
        //if (dictTestData1.TryGetValue(iv, out TestData data1))
        //{
        //    UnityEngine.Profiling.Profiler.EndSample();
        //    OnTest2(data1);
        //}

        //UnityEngine.Profiling.Profiler.BeginSample("Dictionary-ContainsKey2");
        //if (dictTestData1.ContainsKey(iv))
        //{
        //    UnityEngine.Profiling.Profiler.EndSample();
        //    OnTest2(dictTestData1[iv]);
        //}
    }

    private void OnTest2(TestData data_)
    {
        UnityEngine.Profiling.Profiler.BeginSample("DebugLog");
        Debug.Log(data_.strTest);
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void OnTest3()
    {
        UnityEngine.Profiling.Profiler.BeginSample("Dictionary-TryGetValue1");
        if (dictTestData1.TryGetValue(iv, out TestData data1))
        {
            UnityEngine.Profiling.Profiler.EndSample();
            OnTest2(data1);
        }

        UnityEngine.Profiling.Profiler.BeginSample("Dictionary-ContainsKey2");
        if (dictTestData1.ContainsKey(iv))
        {
            UnityEngine.Profiling.Profiler.EndSample();
            OnTest2(dictTestData1[iv]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            OnTest1();
        }

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Button-A"))
        {
            OnTest1();
        }
        if (GUI.Button(new Rect(0, 200, 100, 100), "Button-B"))
        {
            OnTest3();
        }
    }
}
