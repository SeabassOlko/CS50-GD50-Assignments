using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    // Tag
    private string TargetType;

    //Targets Controller
    public TargetSpawner Spawner;

    // Animation
    public Animator animator;

    // Hit tracker
    private bool TargetHit = false;

    // Target Movement
    public bool Moving = false;
    public float Distance;
    // Direction: True == left, false == right
    public bool Direction;
    private float MoveDirection;
    public bool FullMove;
    private bool MoveBack = false;
    public float Speed;
    private Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        TargetType = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
            Move();

        if (gameObject.transform.position.x >= TargetPosition.x && Direction == false && MoveBack == true)
        {
           Reverse(); 
        }    
        else if (gameObject.transform.position.x <= TargetPosition.x && Direction == true && MoveBack == true)
        {
            Reverse();
        }

        if (gameObject.transform.position.x >= TargetPosition.x && Direction == false && MoveBack == false)
        {
            Destroy(); 
        }    
        else if (gameObject.transform.position.x <= TargetPosition.x && Direction == true && MoveBack == false)
        {
            Destroy();
        }
            
    }

    public void Hit()
    {
        if (!TargetHit)
        {
            TargetHit = true;
            animator.SetBool("Hit", true);
        }
    }

    public bool IsHit()
    {
        return TargetHit;
    }

    public void Movement(float Dis, bool Dir, bool FM, float Spd, TargetSpawner Spwn)
    {
        Spawner = Spwn;
        Moving = true;
        Distance = Dis;
        Direction = Dir;
        FullMove = FM;
        if (FullMove == false)
            MoveBack = true;
        Speed = Spd;

        if (Direction)
            MoveDirection = -1;
        else
            MoveDirection = 1;

        Vector3 Temp = new Vector3(Distance * MoveDirection, 0f, 0f);
        TargetPosition = gameObject.transform.position + Temp;
    }

    void Move()
    {
        if (gameObject.transform.position.x <= TargetPosition.x && Direction == false)
            gameObject.transform.position += new Vector3(Time.deltaTime * Speed, 0f, 0f);
        else if (gameObject.transform.position.x >= TargetPosition.x && Direction == true)
            gameObject.transform.position -= new Vector3(Time.deltaTime * Speed, 0f, 0f);;
    }

    void Reverse()
    {
        MoveBack = false;
        Direction = !Direction;
        MoveDirection = -MoveDirection;
        Vector3 Temp = new Vector3(Distance * MoveDirection, 0f, 0f);
        TargetPosition = gameObject.transform.position + Temp;
    }

    void Destroy()
    {
        Spawner.Spawned = false;
        Destroy(gameObject);
    }

}
