using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcMovement : MonoBehaviour {

    public Transform player;
    //number of spaces that can be moved per turn
    public int range = 1;
    private RaycastHit hit;
    //direction to move in 1=east,2=north,3=south,4=west
    public int direction = 1;
    
    //public string dialogue;
    //find player
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    //add action to stack
    void OnEnable()
    {
        EasyMove.OnPlayerMove += MoveNPC;
    }
    //remove action from stack
    void OnDisable()
    {
        EasyMove.OnPlayerMove -= MoveNPC;
    }
    //base called action
    void MoveNPC()
    {
        Wander();
    }
    //particular action to perform
    void Wander()
    {

        //First check if player is in range
        if (PlayerCheck(range, 0, 0, 1))
        {

        }
        else if (PlayerCheck(0, range, 0, 2))
        {

        }
        else if (PlayerCheck(0, -range, 0, 3))
        {

        }
        else if (PlayerCheck(-range, 0, 0, 4))
        {

        }
        //try to move in a direction
        else if (direction == 1)
        {
            MoveDirection(range, 0, 0, 4);
        }
        else if (direction == 2)
        {
            MoveDirection(0, range, 0, 3);
        }

        else if (direction == 3)
        {
            MoveDirection(0, -range, 0, 2);
        }

        else if (direction == 4)
        {
            MoveDirection(-range, 0, 0, 1);
        }
    }

    bool PlayerCheck(int x, int y, int z, int targetDirection)
    {
        return Physics.Linecast(transform.position, transform.position + new Vector3(x, y, z), out hit)
            && hit.collider.gameObject.tag == "Player" && direction == targetDirection;
    }

    void MoveDirection(int x, int y, int z, int newDirection)
    {
        if (Physics.Linecast(transform.position, transform.position + new Vector3(x, y, z), out hit))
        {
            direction = newDirection;
            if(newDirection == 4)
            {
                transform.Translate(new Vector3(hit.transform.position.x + -1 - transform.position.x, 0, 0));
            }
            else if(newDirection == 3)
            {
                transform.Translate(new Vector3(0, hit.transform.position.y + -1 - transform.position.y, 0));
            }
            else if(newDirection == 2)
            {
                transform.Translate(new Vector3(0, hit.transform.position.y + 1 - transform.position.y, 0));
            }
            else if(newDirection == 1)
            {
                transform.Translate(new Vector3(hit.transform.position.x + 1 - transform.position.x, 0, 0));
            }
        }
        else if (!Physics.Linecast(transform.position, transform.position + new Vector3(x,y,z)))
        {
            transform.Translate(x, y, z);
        }
    }
}
