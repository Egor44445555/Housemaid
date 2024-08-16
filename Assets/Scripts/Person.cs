using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.IO;

public class Person : MonoBehaviour
{
    [SerializeField] public float normalSpeed = 3f;

    private Inventory inventory;
    private FrameSwitch frameSwitch;
    private Door door;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private GameObject[] gameObjects;
    public GameObject cartMenu;

    public bool cartMenuIsOpen = false;
    private bool enterCartMenu = false;
    private bool doorActive = false;
    private float speed;
    private string roomName = null;
    private int tasksCount;
    private bool lookUp = false;
    private bool lookDown = false;
    public bool stopRunning = false;
    private bool enterNextFloor = false;
    private Text text;
    private GameObject elevator;
    public GameObject newFrame;
    public GameObject doorEnter;
    public Vector3 doorEnterPoint;
    private bool cloudActive = false;
    string countTrash;
    string countPuddle;
    string countTask;
    bool tasksComplete = true;

    private States State
    {
        get
        {
            return (States)anim.GetInteger("state");
        }
        set
        {
            anim.SetInteger("state", (int)value);
        }
    }
    
    private StatesElevator StateElevator
    {
        get
        {
            return (StatesElevator)elevator.GetComponent<Animator>().GetInteger("state");
        }
        set
        {
            if (elevator)
            {
                elevator.GetComponent<Animator>().SetInteger("state", (int)value);
            }            
        }
    }

    private void ChangeOrderLayerCart()
    {
        if (GameObject.FindGameObjectWithTag("Cart"))
        {
            float playerPositionY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
            float cartPositionY = GameObject.FindGameObjectWithTag("Cart").transform.position.y;
            int playerOrderLayer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder;
            Transform[] objChild = GameObject.FindGameObjectWithTag("Cart").transform.GetComponentsInChildren<Transform>();

            if (playerPositionY > cartPositionY)
            {
                for (var j = 0; j < objChild.Length; j++)
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().sortingOrder = playerOrderLayer + 2;
                }

                GameObject.FindGameObjectWithTag("Cart").GetComponent<SpriteRenderer>().sortingOrder = playerOrderLayer + 1;
            }
            else
            {
                for (var j = 0; j < objChild.Length; j++)
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }

                GameObject.FindGameObjectWithTag("Cart").GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    public void OnTopButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = true;
            lookDown = false;
            rb.velocity = transform.up * normalSpeed;
            State = States.runUp;
        }

        ChangeOrderLayerCart();
    }

    public void OnDownButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = false;
            lookDown = true;
            rb.velocity = -transform.up * normalSpeed;
            State = States.runDown;
        }

        ChangeOrderLayerCart();
    }

    public void OnRightButtonDown()
    {
        if (speed >= 0f)
        {
            speed = normalSpeed;
        }

        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = transform.right * speed;
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        ChangeOrderLayerCart();
    }

    public void OnLeftButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = -transform.right * normalSpeed;
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        ChangeOrderLayerCart();
    }

    public void OnButtonUp()
    {        
        speed = 0f;
        rb.velocity = transform.right * speed;
        
        if (lookUp)
        {
            State = States.looksUp;
        } else if (lookDown)
        {
            State = States.looksDown;
        }
        else
        {
            State = States.idle;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        doorEnter = collider.CompareTag("Door") ? collider.gameObject : null;
        doorActive = collider.CompareTag("Door") ? true : false;
        enterNextFloor = collider.CompareTag("Floor");
        roomName = collider.gameObject.GetComponent<FrameSwitch>() ? collider.gameObject.GetComponent<FrameSwitch>().activeFrame.name : null;
        PlayerPrefs.SetString("taskTarget", collider.CompareTag("Task") ? collider.name : "");
        PlayerPrefs.Save();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        doorActive = false;
        roomName = collider.gameObject.GetComponent<FrameSwitch>() ? collider.gameObject.GetComponent<FrameSwitch>().activeFrame.name : null;
        PlayerPrefs.SetString("taskTarget", "");
        PlayerPrefs.Save();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Cart")
        {
            enterCartMenu = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Cart")
        {
            enterCartMenu = false;
        }
    }

    void CloudAnimation(string inventory)
    {
        Transform[] objChild = GameObject.FindGameObjectWithTag("Cloud").transform.GetComponentsInChildren<Transform>();
        GameObject.FindGameObjectWithTag("Cloud").GetComponent<Animator>().SetBool("show", true);

        foreach (Transform child in objChild)
        {
            if (child.tag != "Cloud" && child.gameObject.GetComponent<SpriteRenderer>())
            {
                if (child.name.Replace("Cloud", "") == inventory)
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
                }
                else
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
            }
        }

        StartCoroutine(hideCloud());
    }

    IEnumerator hideCloud()
    {
        yield return new WaitForSeconds(2);
        GameObject.FindGameObjectWithTag("Cloud").GetComponent<Animator>().SetBool("show", false);
        GameObject.FindGameObjectWithTag("Cloud").GetComponent<Animator>().SetBool("hide", true);
        yield return new WaitForSeconds(0.50f);
        GameObject.FindGameObjectWithTag("Cloud").GetComponent<Animator>().SetBool("hide", false);
    }

    public void Action()
    {
        cloudActive = true;

        if (!stopRunning)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] && PlayerPrefs.GetString("taskTarget") == gameObjects[i].name)
                {
                    bool bagIsExist = false;
                    bool mopIsExist = false;

                    for (int j = 0; j < inventory.stuff.Length; j++)
                    {
                        if (inventory.stuff[j] && inventory.stuff[j].name == "bag")
                        {
                            bagIsExist = true;
                        }

                        if (inventory.stuff[j] && inventory.stuff[j].name == "mop")
                        {
                            mopIsExist = true;
                        }
                    }

                    if (gameObjects[i] && LayerMask.NameToLayer("Trash") == gameObjects[i].layer && bagIsExist)
                    {
                        // Using a bag
                        stopRunning = true;
                        State = States.actionBag;
                        StartCoroutine(destroyTask(i));
                        PlayerPrefs.SetString("task" + gameObjects[i].layer, (int.Parse(PlayerPrefs.GetString("task" + gameObjects[i].layer)) + 1).ToString());
                        PlayerPrefs.Save();
                    }
                    else if (gameObjects[i] && LayerMask.NameToLayer("Trash") == gameObjects[i].layer && !bagIsExist)
                    {
                        // No bag in inventory
                        CloudAnimation("bag");
                    }
                    else if (gameObjects[i] && LayerMask.NameToLayer("Puddle") == gameObjects[i].layer && mopIsExist)
                    {
                        // Using a mop
                        stopRunning = true;
                        State = States.actionMop;
                        StartCoroutine(destroyTask(i));
                        PlayerPrefs.SetString("task" + gameObjects[i].layer, (int.Parse(PlayerPrefs.GetString("task" + gameObjects[i].layer)) + 1).ToString());
                        PlayerPrefs.Save();
                    }
                    else if (gameObjects[i] && LayerMask.NameToLayer("Puddle") == gameObjects[i].layer && !mopIsExist)
                    {
                        // No mop in inventory
                        CloudAnimation("mop");
                    }
                    else if (gameObjects[i] && LayerMask.NameToLayer("Trash") != gameObjects[i].layer)
                    {
                        // Using other stuff in inventory
                        stopRunning = true;
                        //State = States.action;
                        StartCoroutine(destroyTask(i));
                        PlayerPrefs.SetString("task" + gameObjects[i].layer, (int.Parse(PlayerPrefs.GetString("task" + gameObjects[i].layer)) + 1).ToString());
                        PlayerPrefs.Save();
                    }

                    break;
                }
            }
        }

        if (enterCartMenu)
        {
            cartMenu.SetActive(true);
            stopRunning = true;
            cartMenuIsOpen = true;
        }

        var taskInfo = JsonHelper.GetJsonValue(roomName);
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();


        if (roomName != null && taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash || int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
        }

        if (doorActive && tasksComplete)
        {
            if (FindObjectOfType<Door>())
            {
                FindObjectOfType<Door>().OpenDoor(doorEnter);
                StartCoroutine(OpenDoor());
            }
            else
            {
                FindObjectOfType<FrameSwitch>().OpenDoor();
                FindObjectOfType<Door>().CheckDoorAccess();
            }
        }
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(0.30f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        yield return new WaitForSeconds(0.30f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        FindObjectOfType<FrameSwitch>().OpenDoor();

        if (FindObjectOfType<Door>())
        {
            FindObjectOfType<Door>().CheckDoorAccess();
        }        
    }

    IEnumerator destroyTask(int task)
    {
        yield return new WaitForSeconds(1);
        gameObjects[task].transform.position = new Vector2(0, 0);
        stopRunning = false;
        State = States.idle;
        FindObjectOfType<Door>().CheckDoorAccess();
        PlayerPrefs.SetString("taskTarget", "");
        PlayerPrefs.Save();
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        elevator = GameObject.FindGameObjectWithTag("Elevator");
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        speed = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        gameObjects = GameObject.FindGameObjectsWithTag("Task");
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();

        foreach (GameObject task in gameObjects)
        {
            PlayerPrefs.SetString("task" + task.layer, "0");
            PlayerPrefs.Save();
        }

        StateElevator = StatesElevator.close;
        StartCoroutine(closeElevator());
    }

    IEnumerator closeElevator()
    {
        yield return new WaitForSeconds(2);
        StateElevator = StatesElevator.idle;
    }

    void Update()
    {
        if (cloudActive == true)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject.FindGameObjectWithTag("Cloud").gameObject.transform.position = new Vector2(player.position.x + 2, player.position.y + 3);
        }
    }

    IEnumerator closeCloud()
    {
        yield return new WaitForSeconds(2);
        cloudActive = false;
    }
}

public enum States {
    idle,
    run,
    runUp,
    looksUp,
    runDown,
    looksDown,
    actionMop,
    actionBag
}

public enum StatesElevator
{
    idle,
    open,
    close
}

[System.Serializable]
public class RoomInfo
{
    public int collectTrash;
    public int removePuddle;
    public string mainTask;
}