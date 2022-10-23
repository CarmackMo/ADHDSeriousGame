using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float speed;


    private Rigidbody rigidBody;
    private float directionX;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();    
    }


    void Start()
    {
        
    }

    void Update()
    {
        PlayerMovement();
    }


    private void PlayerMovement()
    {
# if UNITY_ANDROID
        directionX = Input.acceleration.x;
#else
        directionX = Input.GetAxis("Horizontal");
#endif

            
        rigidBody.velocity = new Vector3(directionX * speed, 0, 0);


    }
}
