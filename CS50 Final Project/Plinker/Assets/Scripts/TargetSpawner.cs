using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public bool Spawned = false;

    public GameObject TargetPrefab;
    public GameObject HostagePrefab;

    public GameController gameController;

    void Start() 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    void Update() 
    {
        if (gameController.TargetAmount >= 1 && Spawned == false)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        Spawned = true;
        gameController.TargetAmount--;

        bool FullMove;
        float Distance;
        float Speed = Random.Range(0.75f, 2f);
        bool Direction = RdmDirection();

        RdmDistance(out FullMove, out Distance);

        Distance = Distance * gameController.MapWidth;

        if (Direction == false)
        {
            GameObject Target = Instantiate(TargetPrefab, transform.position, transform.rotation);
            Target.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            
            if (RdmHostage())
            {
                GameObject Hostage = Instantiate(HostagePrefab, transform.position - new Vector3(Random.Range(.3f, .5f), 0, 0.1f), transform.rotation);
                Hostage.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            }
        }
        else
        {
            Vector3 position = transform.position + new Vector3(gameController.MapWidth, 0, 0);

            GameObject Target = Instantiate(TargetPrefab, position, transform.rotation);
            Target.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            
            if (RdmHostage())
            {
                GameObject Hostage = Instantiate(HostagePrefab, position + new Vector3(Random.Range(.3f, .5f), 0, -0.1f), transform.rotation);
                Hostage.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            }
        }

    }

    void RdmDistance( out bool FullMove, out float Distance)
    {
        FullMove = false;
        Distance = 2;

        int random = Random.Range(0,4);

        if (random == 0)
        {
            FullMove = false;
            Distance = 0.35f;
        }
        else if (random == 1)
        {
            FullMove = false;
            Distance = 0.5f;
        }
        else if (random == 2)
        {
            FullMove = false;
            Distance = 0.75f;
        }
        else if (random == 3)
        {
            FullMove = true;
            Distance = 1f;
        }
    }

    // Random direcvtion either 0 or 1, left or right
    bool RdmDirection()
    {
        return Random.Range(0,2) == 0;
    }

    // 10% chance of hostage being spawned
    bool RdmHostage()
    {
        return Random.value < .1;
    }
}
