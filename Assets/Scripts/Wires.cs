using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wires : MonoBehaviour
{
    public static Wires instance;

    public InputAction holdAction;

    public InputAction optionsButton;

    public List<GameObject> wirePoints = new List<GameObject>();

    List<bool> wireCompleted = new List<bool>();

    public List<GameObject> wireEndPoints = new List<GameObject>();

    public AnimationCurve pulseCurve;

    public AnimationCurve textCurve;

    public AnimationCurve endCurve;

    [SerializeField] GameObject winText;

    private List<EasyTween> created = new List<EasyTween>();

    [SerializeField] GameObject wirePrefab;

    [SerializeField] Canvas canvas;

    GameObject scene;

    List<GameObject> wires = new List<GameObject>();
    GameObject currentWire;
    Vector3 startPos;

    bool completed;

    bool ended;

    GameObject currentWirePoint;

    bool holding;

    void Awake()
    {
        if (Wires.instance)
            Destroy(this);
        else
            Wires.instance = this;

        holdAction.Enable();
        optionsButton.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < wirePoints.Count; i++)
        {
            wireCompleted.Add(false);
        }

        completed = false;

        ended = false;

        scene = canvas.transform.GetChild(0).gameObject;
    }

    public void Pause()
    {
        holdAction.Disable();
        optionsButton.Disable();
    }

    public void Unpause()
    {
        holdAction.Enable();
        optionsButton.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsButton.ReadValue<float>() > 0)
        {
            MainManager.instance.OpenOptions();
        }

        if (holdAction.ReadValue<float>() > 0)
        {
            if (!holding)
            {
                holding = true;

                Vector3 mousePos = Mouse.current.position.ReadValue();
                mousePos.z = 5;

                foreach (GameObject wirePoint in wirePoints)
                {
                    if (Vector3.Distance(mousePos, wirePoint.transform.position) < 60f && !wireCompleted[wirePoints.IndexOf(wirePoint)])
                    {
                        currentWire = Instantiate(wirePrefab, canvas.transform);
                        currentWire.transform.SetParent(scene.transform);
                        wires.Add(currentWire);
                        currentWire.GetComponent<Image>().color = wirePoint.GetComponent<Image>().color - new Color(0.1f, 0.1f, 0.1f, 0f);
                        currentWirePoint = wirePoint;
                        startPos = currentWirePoint.transform.position;
                        break;
                    }
                }
            }

            if (currentWire != null)
            {
                currentWire.GetComponent<RectTransform>().position = Vector3.Lerp(Mouse.current.position.ReadValue(), startPos, 0.5f);

                currentWire.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(Mouse.current.position.ReadValue(), startPos), 40);

                currentWire.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Mouse.current.position.ReadValue().y - startPos.y, Mouse.current.position.ReadValue().x - startPos.x) * Mathf.Rad2Deg);
            }

        }
        else
        {
            if (holding)
            {
                holding = false;

                Vector3 mousePos = Mouse.current.position.ReadValue();
                mousePos.z = 5;

                bool connected = false;

                foreach (GameObject wirePoint in wireEndPoints)
                {
                    if (Vector3.Distance(mousePos, wirePoint.transform.position) < 60f && wirePoint.GetComponent<Image>().color == currentWirePoint.GetComponent<Image>().color)
                    {
                        currentWire.GetComponent<RectTransform>().position = Vector3.Lerp(wirePoint.transform.position, startPos, 0.5f);

                        currentWire.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(wirePoint.transform.position, startPos), 40);

                        currentWire.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Atan2(wirePoint.transform.position.y - startPos.y, wirePoint.transform.position.x - startPos.x) * Mathf.Rad2Deg);

                        connected = true;

                        wireCompleted[wirePoints.IndexOf(currentWirePoint)] = true;

                        EasyTween tween = currentWire.AddComponent<EasyTween>();

                        currentWire.GetComponent<EasyTween>().rectTransform = currentWire.GetComponent<RectTransform>();

                        created.Add(tween);

                        tween.SetAnimationScale(new Vector3(.98f, .85f, 1f), new Vector3(1f, 1.0f, 1f), pulseCurve, pulseCurve);

                        tween.SetAnimatioDuration(0.25f);

                        tween.OpenCloseObjectAnimation();

                        currentWire = null;

                        currentWirePoint = null;

                        break;
                    }
                }

                if (completed && !ended)
                {
                    EasyTween tween = winText.GetComponent<EasyTween>();

                    tween.SetAnimationPosition(new Vector3(0, -1000, 20), new Vector3(0, 0, 20), endCurve, endCurve);

                    tween.SetAnimatioDuration(0.7f);

                    tween.OpenCloseObjectAnimation();



                    EasyTween tween2 = scene.AddComponent<EasyTween>();

                    scene.GetComponent<EasyTween>().rectTransform = scene.GetComponent<RectTransform>();

                    created.Add(tween2);

                    tween2.SetAnimationPosition(new Vector3(0, 0, 20), new Vector3(0, -1000, 20), endCurve, endCurve);

                    tween2.SetAnimatioDuration(0.7f);

                    tween2.OpenCloseObjectAnimation();

                    ended = true;
                    FuseBox.instance.OnMinigameClosed();
                }

                if (!wireCompleted.Contains(false) && !completed)
                {
                    EasyTween tween = winText.AddComponent<EasyTween>();

                    winText.GetComponent<EasyTween>().rectTransform = winText.GetComponent<RectTransform>();

                    created.Add(tween);

                    tween.SetAnimationPosition(new Vector3(0, -800, 20), new Vector3(0, 0, 20), textCurve, textCurve);

                    tween.SetAnimatioDuration(0.8f);

                    tween.OpenCloseObjectAnimation();

                    completed = true;
                }
                
                // Destroy wire
                if (currentWire != null && connected == false)
                {
                    Destroy(currentWire);
                }
            }
        }
    }
}
