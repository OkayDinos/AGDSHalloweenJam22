using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float minViewDistance = 25f;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    Rigidbody m_RB;
    float m_Speed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        m_RB = this.gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);
    }

    void OnMove(InputValue a_IV)
    {
        Vector2 InputVector = a_IV.Get<Vector2>();
        Debug.Log("Lol");
        Debug.Log(InputVector);



        m_RB.AddForce(InputVector, ForceMode.Force);
        
    }

    void OnFire()
    {
        Debug.Log("FML");
    }



}
