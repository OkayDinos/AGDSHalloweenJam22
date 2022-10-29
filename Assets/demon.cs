using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class demon : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = Player.transform.position;
    }
}
