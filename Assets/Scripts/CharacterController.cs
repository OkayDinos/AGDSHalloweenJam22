using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float minViewDistance, max;

    public float mouseSensitivity = 100f;

    float yRotation = 0f;
    float xRotation = 0f;
    bool moving;

    Rigidbody m_RB;
    float m_Speed = 500f;

    // Start is called before the first frame update
    void Start()
    {
        m_RB = this.gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMove(InputValue a_IV)
    {
            Vector2 InputVector = a_IV.Get<Vector2>();
            Debug.Log("Lol");
            Debug.Log(InputVector);

            m_RB.AddForce(InputVector * m_Speed, ForceMode.Force);

    }

    void OnFire()
    {
        Debug.Log("FML");
    }

    void OnLook(InputValue a_IV)
    {

        Vector2 InputVector = a_IV.Get<Vector2>();

        float mouseX = InputVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = InputVector.y * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);

        this.transform.Rotate(Vector3.up * mouseX);


        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90, max);
        Debug.Log(mouseY + " | " + yRotation);;

        Camera.main.transform.eulerAngles = new Vector3(yRotation, 0, 0);   
    }

}
