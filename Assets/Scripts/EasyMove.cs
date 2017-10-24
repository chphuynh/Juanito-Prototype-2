using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EasyMove : MonoBehaviour
{
    //alternative movement
    public bool smoothMode = false;
    public float walkSpeed = 3;
	public int boxesOnTarget = 0;

    //base Movement Related
    public int moveRange;
    private RaycastHit hit;
   
    //Delegate manages a stack of all non player actions that occur
    public delegate void MoveAction();
    public static event MoveAction OnPlayerMove;
    
    //Dialogue
    public GameObject textField;

    //Handles npc interaction
    void Talk(GameObject npc)
    {
        if(npc.GetComponent<DialogueTrigger>()!= null) npc.GetComponent<DialogueTrigger>().TriggerDialogue();
    }
	
    //Handles box interaction
    void MoveBox(GameObject box, int x, int y, int z)
    {
		bool wasOnTarget = isOnBlockTarget(box);
		bool moved = false;
		while (true)
		{
			moved = true;
			bool blocked = Physics.Linecast(box.transform.position, box.transform.position + new Vector3(x, y, z), out hit);
			string hitTag = (blocked)? hit.collider.gameObject.tag: "";
			if (!blocked || hitTag == "EventTrigger" || hitTag == "BoxTarget")
			{
				box.transform.Translate(x, y, z);
			} else {
				break;
			}
        }
		
		if (moved)
		{
			if (wasOnTarget)
			{
				--boxesOnTarget;
			}
			
			if (isOnBlockTarget(box))
			{
				++boxesOnTarget;
			}
		}
    }
	public bool isOnBlockTarget(GameObject obj) {
		bool coll = Physics.Linecast(obj.transform.position, obj.transform.position + new Vector3(0f,0f,0.5f), out hit);
		return coll && hit.collider.gameObject.tag == "BoxTarget";
	}
	public void HandleEventTrigger(string trigger) 
    {
		switch(trigger) {
			case "leaveHouse":
				transform.Translate(new Vector3(0, -27, 0));
				break;
			case "enterHouse":
				transform.Translate(new Vector3(0, 27, 0));
                break;
            case "spawnBoxes":
                GameObject boxes = GameObject.FindWithTag("Boxes");
                foreach(Transform box in boxes.transform)
                {
                    box.transform.GetChild(0).gameObject.SetActive(true);
                }

                DialogueTrigger pharmDialogue = GameObject.Find("Pharmacist").GetComponent<DialogueTrigger>();
                Dialogue newDialogue = new Dialogue();
                newDialogue.name = pharmDialogue.dialogue.name;
                string[] sentence = new string[1];
                sentence[0] = "Let me see where the packages are...";
                newDialogue.sentences = sentence;
                pharmDialogue.dialogue = newDialogue;
                pharmDialogue.eventType = "checkBoxes";

                break;
            case "wrongBoxes":
                DialogueTrigger pharmDialogue2 = GameObject.Find("Pharmacist").GetComponent<DialogueTrigger>();
                Dialogue newDialogue2 = new Dialogue();
                newDialogue2.name = pharmDialogue2.dialogue.name;
                string[] sentence2 = new string[3];
                sentence2[0] = "I don't think you delivered the packages right, ya see?";
                sentence2[1] = "Don't worry, I have two more packages for you outside.";
                sentence2[2] = "All you need to do is move the boxes to buildings of the same color.";
                newDialogue2.sentences = sentence2;
                pharmDialogue2.dialogue = newDialogue2;
                pharmDialogue2.eventType = "";
                FindObjectOfType<DialogueManager>().StartDialogue(newDialogue2, "");
                sentence2 = new string[1];
                sentence2[0] = "Let me see where the packages are...";
                newDialogue2.sentences = sentence2;
                pharmDialogue2.dialogue = newDialogue2;
                pharmDialogue2.eventType = "checkBoxes";
                break;
            case "rightBoxes":
                GameObject boxes3 = GameObject.FindWithTag("Boxes");
                
                foreach(Transform box in boxes3.transform)
                {
                    box.transform.GetChild(0).gameObject.SetActive(false);
                }

                DialogueTrigger pharmDialogue3 = GameObject.Find("Pharmacist").GetComponent<DialogueTrigger>();
                Dialogue newDialogue3 = new Dialogue();
                newDialogue3.name = pharmDialogue3.dialogue.name;
                string[] sentence3 = new string[3];
                sentence3[0] = "It seems you have correctly delivered my packages.";
                sentence3[1] = "As a reward, here is your la flor de mazapan. It is blue by the counter.";
                sentence3[2] = "This is not from the forest, so beware it may not be as effective.";
                newDialogue3.sentences = sentence3;
                pharmDialogue3.dialogue = newDialogue3;
                pharmDialogue3.eventType = "";
                FindObjectOfType<DialogueManager>().StartDialogue(newDialogue3, "");
                sentence3 = new string[1];
                sentence3[0] = "Need more medicine, you say?";
                newDialogue3.sentences = sentence3;
                pharmDialogue3.dialogue = newDialogue3;
                pharmDialogue3.eventType = "";
                GameObject medicine = GameObject.Find("Medicine").transform.GetChild(0).gameObject;
                medicine.SetActive(true);
                break;
            case "checkBoxes":
                GameObject boxes2 = GameObject.FindWithTag("Boxes");
                GameObject firstBox = new GameObject();
                GameObject secondBox = new GameObject();

                bool boxLocate1 = false;
                bool boxLocate2 = false;
                foreach(Transform box in boxes2.transform)
                {
                    GameObject box2 = box.transform.GetChild(0).gameObject;
                    box2.SetActive(true);
                    switch(box2.GetComponent<Box>().boxNumber)
                    {
                        case "1":
                            firstBox = box2;
                            if(box2.transform.position == new Vector3(6.5f, 2.5f, -1f))
                            {
                                boxLocate1 = true;
                            }
                            break;
                        case "2":
                            secondBox = box2;
                            if(box2.transform.position == new Vector3(-6.5f, -0.5f, -1f))
                            {
                                boxLocate2 = true;
                            }
                            break;
                        default:
                            break;
                    }
                    //Debug.Log(box2.transform.position);
                }

                if(!boxLocate1 || !boxLocate2)
                {
                    firstBox.transform.position = new Vector3(-5.5f, 1.5f, -1f);
                    secondBox.transform.position = new Vector3(-5.5f, 3.5f, -1f);
                    HandleEventTrigger("wrongBoxes");
                }
                else
                {
                    HandleEventTrigger("rightBoxes");
                }

                break;
            case "enterPharm":
                transform.Translate(new Vector3(-25,0,0));
                break;
            case "leavePharm":
                //transform.Translate(new Vector3(25,0,0));
                transform.position = new Vector3(-7.5f, 1.5f, -1f);
                break;
            case "talkNeighbor":
                GameObject.Find("Guards").SetActive(false);
                break;
            case "leaveCity":
                Application.Quit();
                Debug.Log("Quit Game");
                break;
			default:
				//Debug.Log("Not defined.");
				break;
		}
	}
    //regulated call if smooth movement enabled
    void FixedUpdate()
    {
        if (smoothMode)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Lerp(0, Input.GetAxis("Horizontal") * walkSpeed, 1.2f),
                                                 Mathf.Lerp(0, Input.GetAxis("Vertical") * walkSpeed, 1.2f), 0);
        }
    }

    //Input and call to delegate handled in Update
    void Update()
    {
        if (!smoothMode)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveDirection(0, moveRange, 0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDirection(0, -moveRange, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveDirection(moveRange, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveDirection(-moveRange, 0, 0);
            }
        }
    }
    
    //collision based interaction if smooth movement enabled
    void OnCollisionEnter(Collision col)
    {
        if (smoothMode)
        {
			if (col.gameObject.tag == "NPC")
			{
				Talk(col.gameObject);
			}
			else if (col.gameObject.tag == "Box")
			{
				
			}
        }
    }

    void MoveDirection(int x, int y, int z)
    {
		bool continueMove = true;
		
        // Continue Conversation       
        if(FindObjectOfType<DialogueManager>().textField.activeSelf)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            return;
        }
        if (Physics.Linecast(transform.position, transform.position + new Vector3(x, y, z), out hit))
        {
			continueMove = false;
			string hitTag = hit.collider.gameObject.tag;
            if (hitTag == "NPC")
            {
                Talk(hit.collider.gameObject);
            }
			else if (hitTag == "Box")
			{
				MoveBox(hit.collider.gameObject, x, y, z);
			}
            else if (hitTag == "Medicine")
            {
                continueMove = true;
                hit.collider.gameObject.SetActive(false);
                Dialogue medDialogue = new Dialogue();
                medDialogue.name = "";
                string[] medSentence = new string[1];
                medSentence[0] = "La flor de mazapan obtained.";
                medDialogue.sentences = medSentence;
                FindObjectOfType<DialogueManager>().StartDialogue(medDialogue, "");
                GameObject.Find("GrandFather").transform.Translate(-2f, 0f, 0f);
                GameObject.Find("Neighbor").transform.GetChild(0).gameObject.SetActive(true);
            }
			else if (hitTag == "EventTrigger") {
				HandleEventTrigger(hit.collider.gameObject.GetComponent<EventTrigger>().eventType);
				continueMove = true;
			} else if (hitTag == "BoxTarget") {
				continueMove = true;
			}
        }
        
		if (continueMove)
        {
            transform.Translate(x, y, z);
            if (OnPlayerMove != null) OnPlayerMove();
        }
    }
}