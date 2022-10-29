using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactableType
{ key, fuse }

public class Pickup : Interactables
{
    public interactableType currentType;

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
        GameObject.FindGameObjectWithTag("Player").SendMessage("PickUp", currentType);
        GameObject.Destroy(this.gameObject);
    }
}
