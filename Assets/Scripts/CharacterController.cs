using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum CurrentCam { MAIN, CUTSCENE, BEHIND, DEAD }

namespace OkayDinos.GrimsNightmare
{
    public class CharacterController : MonoBehaviour
    {
        public static CharacterController instance;

        [SerializeField] float minViewDistance, max;

        public float mouseSensitivity = 100f;

        bool controls = true;

        float m_CameraXRotation = 0f;
        float xRotation = 0f;
        Vector2 m_move;
        public bool sprintPressed = false;
        public bool lookBehind = false;

        float changeSceneTimer = 40f;

        bool sceneLoading = false;

        bool win = false;

        Rigidbody m_RB;
        float m_Speed = 5f;
        float m_SprintSpeed = 11f;
        public bool hasKey = false;
        public bool hasFuse = false;
        bool flipped = false;

        public bool HouseScene = false;

        [SerializeField] Camera mainCam, cutsceneCam, behindCam, deadCam;

        Interactables m_CurrenInteractable;
        InputActions m_InputActions = null;

        public InputAction optionsButton;

        void Awake()
        {
            if (CharacterController.instance)
                Destroy(this);
            else
                CharacterController.instance = this;

            optionsButton.Enable();
        }

        // Start is called before the first frame update
        void Start()
        {
            optionsButton.Enable();

            m_RB = this.gameObject.GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            
            SetCam(CurrentCam.MAIN);

            if (HouseScene)
            {


                WakeUp();
            }
            else
            {
                BeginForest();
            }
        }

        async void WakeUp()
        {
            SetCam(CurrentCam.CUTSCENE);

            m_InputActions.Disable();
            controls = false;

            float time = 0.5f, timer = 0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                
                await System.Threading.Tasks.Task.Yield();
            }

            cutsceneCam.GetComponent<Animator>().Play("WakeUp");

            time = 1f;
            timer = 0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                
                await System.Threading.Tasks.Task.Yield();
            }

            m_InputActions.Enable();
            controls = true;

            SetCam(CurrentCam.MAIN);
        }

        void SetCam(CurrentCam currentCam)
        {
            switch (currentCam)
            {
                case CurrentCam.MAIN:
                    mainCam.enabled = true;
                    cutsceneCam.enabled = false;
                    behindCam.enabled = false;
                    deadCam.enabled = false;    
                    break;
                case CurrentCam.CUTSCENE:
                    mainCam.enabled = false;
                    cutsceneCam.enabled = true;
                    behindCam.enabled = false;
                    deadCam.enabled = false;
                    break;
                case CurrentCam.BEHIND:
                    mainCam.enabled = false;
                    cutsceneCam.enabled = false;
                    behindCam.enabled = true;
                    deadCam.enabled = false;
                    break;
                case CurrentCam.DEAD:
                    mainCam.enabled= false;
                    cutsceneCam.enabled = false;
                    behindCam.enabled = false;
                    deadCam.enabled = true;
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

        public async void BeginForest()
        {
            m_InputActions.Disable();
            controls = false;

            //StaticManager.instance.Set(false);

            SetCam(CurrentCam.CUTSCENE);
            
            cutsceneCam.GetComponent<Animator>().Play("InForestStart");

            float time = 2f, timer = 0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                await System.Threading.Tasks.Task.Yield();
            }

            SetCam(CurrentCam.MAIN);

            cutsceneCam.GetComponent<Animator>().Play("WakeUp");

            m_InputActions.Enable();
            controls = true;
        }

        public async void GoToTheForest()
        {
            m_InputActions.Disable();
            controls = false;

            float time = 4f, timer = 0f;

            SetCam(CurrentCam.CUTSCENE);
            
            cutsceneCam.GetComponent<Animator>().Play("GotoForest");

            while (timer < time)
            {
                timer += Time.deltaTime;

                await System.Threading.Tasks.Task.Yield();
            }
            
            StaticManager.instance.StaticFor(1.6f);

            timer = 0f;
            time = 1.5f;

            while (timer < time)
            {
                timer += Time.deltaTime;

                await System.Threading.Tasks.Task.Yield();
            }

            if (cutsceneCam != null)
            cutsceneCam.GetComponent<Animator>().enabled = false;

            SoundManager.instance.PlayFootstepsSound();

            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneName.THEFOREST);
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

        public void YouWin()
        {
            win = true;
        }

        public void Unpause()
        {
            m_InputActions.Enable();
            controls = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (MainManager.instance.currentGameState == GameState.InOptions)
            {
                Cursor.lockState = CursorLockMode.None;
                m_InputActions.Disable();
                controls = false;
            }

            if (optionsButton.ReadValue<float>() > 0 && !(MainManager.instance.currentGameState == GameState.InOptions))
            {
                Debug.Log("Options");
                MainManager.instance.OpenOptions();
            }

            if (controls)
            Move(m_move);

            if (controls && !win)
            changeSceneTimer -= Time.deltaTime;

            if (changeSceneTimer <= 0)
            {
                if (HouseScene && !sceneLoading)
                {
                    sceneLoading = true;
                    GoToTheForest();
                }
            }
        }

        void OnMove(InputValue context)
        {
            m_move = context.Get<Vector2>();

        }

        void OnFire()
        {
            if (controls)
            Debug.Log("FML");
        }

        void OnLook(InputValue a_IV)
        {
            if (mainCam.enabled && controls)
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
                this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("die");
                other.GetComponent<Animator>().SetTrigger("dead");
                other.GetComponent<demon>().playerDead = true;

                CapsuleCollider[] capsules = other.GetComponents<CapsuleCollider>();
                foreach(CapsuleCollider c in capsules)
                {
                    c.enabled = false;
                }

                other.transform.position = this.transform.position + this.transform.forward * 0.7f;
                other.transform.LookAt(this.transform.position);
                other.transform.Rotate(0, -200, 0);

                GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);

                SetCam(CurrentCam.DEAD);
                this.GetComponent<CharacterController>().enabled = false;
                Debug.Log("Game over");
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
            if(m_CurrenInteractable && controls)
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