using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : Interactables
{
    float minimum = 0f;
    float maximum = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoInteract()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<OkayDinos.GrimsNightmare.CharacterController>().hasKey == true)
        {
            Debug.Log("open door");
            OpenDoor();
        }
    }

    async void OpenDoor()
    {
        float time = 5f;
        float timer = 0f;
        while(timer < time)
        {
            timer += Time.deltaTime;
            transform.rotation = new Quaternion(0, Mathf.Lerp(minimum, maximum, timer / time), 0, 1);
            await System.Threading.Tasks.Task.Yield();
        }
    }
}
