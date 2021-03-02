using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestImage : MonoBehaviour
{
    public Image imgLeft;
    public Image imgRight;
    public Image imgMiddle;

    private bool isRotation = default;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            isRotation = !isRotation;
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (isRotation)
        {
            Vector3 euler = imgMiddle.transform.rotation.eulerAngles;
            imgMiddle.transform.rotation = Quaternion.Euler(euler.x, euler.y + Time.deltaTime * 20, euler.z);
        }
    }


}
