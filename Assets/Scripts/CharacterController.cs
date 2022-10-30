using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OkayDinos.GrimsNightmare
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] float minViewDistance, max;

        public float mouseSensitivity = 100f;

        float m_CameraXRotation = 0f;
        float xRotation = 0f;
        Vector2 m_move;
        public bool sprintPressed = false;
        public bool lookBehind = false;

        Rigidbody m_RB;
        float m_Speed = 5f;
        float m_SprintSpeed = 11f;
        public bool hasKey = false;
        public bool hasFuse = false;
        bool flipped = false;

        Interactables m_CurrenInteractable;
        InputActions m_InputActions = null;

        public Camera cam1;
        public Camera cam2;

        // Start is called before the first frame update
        void Start()
        {
            m_RB = this.gameObject.GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            cam1.enabled = true;
            cam2.enabled = false;
        }

        void OnEnable()
        {
            m_InputActions = new InputActions();
            m_InputActions.Enable();

            m_InputActions.Player.Sprint.performed += SetSprint;
            m_InputActions.Player.Sprint.canceled += SetSprint;

            m_InputActions.Player.Behind.performed += SetLookBehind;
            m_InputActions.Player.Behind.canceled += SetLookBehind;
        }

        void OnDisable()
        {
            m_InputActions.Player.Sprint.performed -= SetSprint;
            m_InputActions.Player.Sprint.canceled -= SetSprint;

            m_InputActions.Player.Behind.performed -= SetLookBehind;
            m_InputActions.Player.Behind.canceled -= SetLookBehind;

            m_InputActions.Disable();
        }

        void SetSprint(InputAction.CallbackContext ctx)
        {
            sprintPressed = ctx.performed;
        }

        void SetLookBehind(InputAction.CallbackContext context)
        {
            lookBehind = context.performed;
            if (context.performed)
            {
                cam1.enabled = false;
                cam2.enabled = true;
            }
            else
            {
                cam1.enabled = true;
                cam2.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Move(m_move);
        }

        void OnMove(InputValue context)
        {
            m_move = context.Get<Vector2>();

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


            m_CameraXRotation -= mouseY;
            m_CameraXRotation = Mathf.Clamp(m_CameraXRotation, -90, max);

            Camera.main.transform.eulerAngles = new Vector3(m_CameraXRotation, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ruben"))
            {
                this.transform.GetChild(1).GetComponent<Animator>().SetTrigger("die");
                other.GetComponent<Animator>().SetTrigger("dead");
                other.GetComponent<demon>().playerDead = true;
                GameObject.Destroy(m_RB);
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<CharacterController>().enabled = false;
            }

            Debug.Log("Press e to pick up");
            if(other.gameObject.layer == 7)
            {
                m_CurrenInteractable = other.gameObject.GetComponent<Interactables>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(m_CurrenInteractable == other.gameObject.GetComponent<Interactables>())
            {
                m_CurrenInteractable = null;
            }
        }

        void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.01)
                return;
            var scaledMoveSpeed = m_Speed * Time.deltaTime;
            if (sprintPressed)
                scaledMoveSpeed = m_SprintSpeed * Time.deltaTime;
            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
            transform.position += move * scaledMoveSpeed;
        }

        void OnInteract()
        {
            if(m_CurrenInteractable)
            {
                m_CurrenInteractable.SendMessage("DoInteract");
            }
        }

        void PickUp(interactableType a_Current)
        {

            switch (a_Current)
            {
                case interactableType.key:
                    hasKey = true;
                    Debug.Log(hasKey);
                    break;
                case interactableType.fuse:
                    hasFuse = true;
                    Debug.Log(hasFuse);
                    break;
                default:
                    break;
            }

        }
    }
}