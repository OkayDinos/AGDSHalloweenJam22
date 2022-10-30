using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class demon : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    float timer = 0f;

    bool delay = true;
    public bool playerDead = false;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
         agent = GetComponent<NavMeshAgent>();

         SpawnDelay();
    }

    async void SpawnDelay()
    {
        float time = 3f, timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            await System.Threading.Tasks.Task.Yield();
        }
        delay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!delay)
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
}
