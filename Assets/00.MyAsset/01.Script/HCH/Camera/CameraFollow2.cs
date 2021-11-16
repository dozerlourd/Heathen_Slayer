using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform target; // Variable target will follow the target which is the Player. 
    public Vector3 cameraOffset; // cameraOffset variable is used to keep the camera in postion where it dosent leave the Player.
    public float followSpeed = 10f; // This followSpeed variable will determine how fast the camera will follow the target which is the Player.
    public float xMin = 0f; // This xMin variable which is initialized to 0f just sets the camera to postion 0.
    Vector3 velocity = Vector3.zero; // This piece of code just sits as a refrence when we use Vector3.SmoothDamp.

    void FixedUpdate()
    {
        Vector3 targetPos = target.position + cameraOffset; // This line of code just makes the camera follow the Player.
        Vector3 clampPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, float.MaxValue), targetPos.y, targetPos.z); // This line of code justs controlles how the camera will follow the target.
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampPos, ref velocity, followSpeed * Time.deltaTime); // This line of code just takes the current postion of the target and sets how fast the camera will follow behind the target.

        transform.position = new Vector3(transform.position.x, smoothPos.y,transform.position.z);
    }

    private void Update()
    {
        Vector3 targetPos = target.position + cameraOffset; // This line of code just makes the camera follow the Player.
        Vector3 clampPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, float.MaxValue), targetPos.y, targetPos.z); // This line of code justs controlles how the camera will follow the target.
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampPos, ref velocity, followSpeed * Time.deltaTime); // This line of code just takes the current postion of the target and sets how fast the camera will follow behind the target.

        transform.position = new Vector3(smoothPos.x, transform.position.y, transform.position.z);
    }
}