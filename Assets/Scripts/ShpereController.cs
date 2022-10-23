using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpereController : MonoBehaviour
{
    bool isSetUp = false;
    bool gyInfo;
    Gyroscope go;
    Rigidbody rb;

    Vector3 angle;
    float dirX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gyInfo = SystemInfo.supportsGyroscope;
        go = Input.gyro;
        go.enabled = true;
    }

    private void Update()
    {

        dirX = Input.acceleration.x * 5;
        rb.velocity = new Vector3 (dirX, 0, 0);



    }

    private void OnGUI()
    {
        if (!isSetUp)
            GUI.Label(new Rect(100, 100, 100, 30), "Set up fail!!!!!");
        else
            GUI.Label(new Rect(100, 200, 200, 100), angle.ToString());

        
    }



}
