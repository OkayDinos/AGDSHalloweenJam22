using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wires : MonoBehaviour
{
    public InputAction holdAction;

    public List<GameObject> wirePoints = new List<GameObject>();

    [SerializeField] GameObject wirePrefab;

    [SerializeField] Canvas canvas;

    List<GameObject> wires = new List<GameObject>();
    GameObject currentWire;
    Vector3 startPoint;

    bool holding;

    // Start is called before the first frame update
    void Start()
    {
        holdAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (holdAction.ReadValue<float>() > 0)
        {
            if (!holding)
            {
                holding = true;

                Vector3 mousePos = Mouse.current.position.ReadValue();
                mousePos.z = 5;

                foreach (GameObject wirePoint in wirePoints)
                {
                    if (Vector3.Distance(mousePos, wirePoint.transform.position) < 25f)
                    {
                        startPoint = mousePos;
                        currentWire = Instantiate(wirePrefab, canvas.transform);
                        currentWire.transform.SetParent(canvas.transform);
                        wires.Add(currentWire);
                        break;
                    }
                }
                
                // GameObject wire = Instantiate(wirePrefab, transform.position, Quaternion.identity);
                // wires.Add(wire);

                // wire.transform.SetParent(canvas.transform);

                // currentWire = wire;

                // startPoint = Mouse.current.position.ReadValue();
            }

            if (currentWire != null)
            {
                currentWire.GetComponent<RectTransform>().position = Vector3.Lerp(Mouse.current.position.ReadValue(), startPoint, 0.5f);

                currentWire.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(Mouse.current.position.ReadValue(), startPoint), 10);

                currentWire.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Mouse.current.position.ReadValue().y - startPoint.y, Mouse.current.position.ReadValue().x - startPoint.x) * Mathf.Rad2Deg);
            }

        }
        else
        {
            if (holding)
            {
                holding = false;

                // Destroy wire
                if (currentWire != null)
                {
                    Destroy(currentWire);
                }
            }
        }
    }
}
