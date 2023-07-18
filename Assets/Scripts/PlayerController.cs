/*
 * Author: Adrian
 * Date: 18/7/2023
 * Description: Basic Player movement
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The Vector3 used to store the WASD input of the user.
    /// </summary>
    Vector3 movementInput = Vector3.zero;

    /// <summary>
    /// The Vector3 used to store the left/right mouse input of the user.
    /// </summary>
    Vector3 rotationInput = Vector3.zero;

    /// <summary>
    /// The Vector3 used to store the up/down mouse input of the user.
    /// </summary>
    Vector3 headRotationInput;

    /// <summary>
    /// The Vector3 used to be able to clamp the camera.
    /// </summary>
    Vector3 localCameraRot;

    /// <summary>
    /// The movement speed of the player per second.
    /// </summary>
    public float baseMoveSpeed = 5f;

    /// <summary>
    /// The speed at which the player rotates
    /// </summary>
    public float rotationSpeed = 60f;       

    /// <summary>
    /// The camera attached to the player object
    /// </summary>
    public Transform playerCamera;

    /// <summary>
    /// The Body attached to the player object
    /// </summary>
    public GameObject playerBody;


    /// <summary>
    /// Called when the Move action is detected.
    /// </summary>
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();                     
        
    }
    
    //Player Movement
    private void Movement()
    {
        //Moving forward
        Vector3 forwardDirection = transform.forward;       
        
        //Moving right + left
        Vector3 rightDirection = transform.right; 

        //Player Movement
        transform.position += ((forwardDirection * movementInput.y + rightDirection * movementInput.x) * baseMoveSpeed * Time.deltaTime );
    }


    /// <summary>
    /// Called when the Look action is detected.
    /// </summary>
    /// <param name="value"></param>
     void OnLook(InputValue value){
        
        //rotation value for camera to look up and down
        rotationInput.x = -value.Get<Vector2>().y;                      
        
        //rotation value for camera to look left and right
        rotationInput.y = value.Get<Vector2>().x;                        
        
    }

    /// Player rotation
    private void Rotation()
    {
        //up and down
        localCameraRot.x += rotationInput.x * rotationSpeed * Time.deltaTime;
        
        //left and right
        localCameraRot.y += rotationInput.y * rotationSpeed * Time.deltaTime;
        
        //Camera will not go upsidedown
        localCameraRot.x = Mathf.Clamp(localCameraRot.x, -90, 90);
        
        //Player Camera movement
        playerCamera.localRotation = Quaternion.Euler(localCameraRot);

        //Player will move along with the camera direction
        transform.rotation = Quaternion.Euler(0,localCameraRot.y,0);
        
        

        
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Hi");
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
    }
}
