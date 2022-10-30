using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class demon : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    float timer = 0f;
    public bool playerDead = false;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
         agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDead)
        {
            timer += Time.deltaTime;

            if (timer > 0.05f)
            {
                
                agent.destination = Player.transform.position;
                timer = 0f;
            }
        }
        else
        {
            agent.ResetPath();
        }
    }
}
