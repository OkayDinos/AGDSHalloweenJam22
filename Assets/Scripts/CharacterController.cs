using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum CurrentCam { MAIN, CUTSCENE, BEHIND }

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

        [SerializeField] Camera mainCam, cutsceneCam, behindCam;

        Interactables m_CurrenInteractable;
        InputActions m_InputActions = null;

        // Start is called before the first frame update
        void Start()
        {
            m_RB = this.gameObject.GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            
        }

        void SetCam(CurrentCam currentCam)
        {
            switch (currentCam)
            {
                case CurrentCam.MAIN:
                    mainCam.enabled = true;
                    cutsceneCam.enabled = false;
                    behindCam.enabled = false;
                    break;
                case CurrentCam.CUTSCENE:
                    mainCam.enabled = false;
                    cutsceneCam.enabled = true;
                    behindCam.enabled = false;
                    break;
                case CurrentCam.BEHIND:
                    mainCam.enabled = false;
                    cutsceneCam.enabled = false;
                    behindCam.enabled = true;
                    break;
            }
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

        async void GoToTheForest()
        {
            m_InputActions.Disable();

            float time = 4f, timer = 0f;

            SetCam(CurrentCam.CUTSCENE);
            
            cutsceneCam.GetComponent<Animator>().Play("GotoForest");

            while (timer < time)
            {
                timer += Time.deltaTime;

                await System.Threading.Tasks.Task.Yield();
            }
            
            StaticManager.instance.Set(true);

            timer = 0f;
            time = 1.5f;

            while (timer < time)
            {
                timer += Time.deltaTime;

                await System.Threading.Tasks.Task.Yield();
            }

            StaticManager.instance.Set(false);

            cutsceneCam.GetComponent<Animator>().Play("WakeUp");

            //Temporary
            m_InputActions.Enable();

            cutsceneCam.enabled = false;
            mainCam.enabled = true;
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
                SetCam(CurrentCam.BEHIND);
            }
            else
            {
                SetCam(CurrentCam.MAIN);
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
            if (mainCam.enabled)
            {
                Vector2 InputVector = a_IV.Get<Vector2>();

                float mouseX = InputVector.x * mouseSensitivity * Time.deltaTime;
                float mouseY = InputVector.y * mouseSensitivity * Time.deltaTime;

                xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);

                this.transform.Rotate(Vector3.up * mouseX);


                m_CameraXRotation -= mouseY;
                m_CameraXRotation = Mathf.Clamp(m_CameraXRotation, -90, max);

                mainCam.transform.eulerAngles = new Vector3(m_CameraXRotation, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
        
            }
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