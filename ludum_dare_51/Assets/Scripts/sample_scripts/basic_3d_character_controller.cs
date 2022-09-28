using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic_3d_character_controller : MonoBehaviour
{
    
    public float speed = 10.0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialCamOffset;
    
    
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += speed * move * Time.deltaTime;
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialCamOffset = Camera.main.transform.position - initialPosition;
    }
    
    void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
    
    void Die()
    {
        Debug.Log("You died!");
        Reset();
    }
    
    // Update is called once per frame
    void Update()
    {
        CameraLerpFollow();
        Move();
        ProcessJump();
        if (transform.position.y < -10)
        {
            Die();
        }
    }


    void ProcessJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }



    void CameraLerpFollow()
    {
        Vector3 targetPosition = transform.position + initialCamOffset;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 0.1f);
    }
}
