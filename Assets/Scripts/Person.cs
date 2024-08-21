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

    [HideInInspector] public string popupIsOpen = "";
    [HideInInspector] public GameObject popupObject;
    [HideInInspector] public bool stopRunning = false;
    [HideInInspector] public GameObject newFrame;
    [HideInInspector] public GameObject doorEnter;
    [HideInInspector] public Vector3 doorEnterPoint;

    private bool enterPopupObject = false;
    private bool doorActive = false;
    private float speed;
    private string roomName = null;
    private int tasksCount;
    private bool lookUp = false;
    private bool lookDown = false;    
    private bool enterNextFloor = false;
    private Text text;
    private GameObject elevator;    
    
    private bool cloudActive = false;
    string countTrash;
    string countPuddle;
    string countTask;
    string countToiletPaper;
    string countTowels;
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

    public void OnTopButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = true;
            lookDown = false;
            rb.velocity = transform.up * normalSpeed;
            State = States.runUp;
        }
        MovePerson();
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

        MovePerson();
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

        MovePerson();
    }

    public void OnRightTopButtonDown()
    {
        if (speed >= 0f)
        {
            speed = normalSpeed;
        }

        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = new Vector3(transform.right.x * speed, transform.up.y * speed / 2, 0);
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        MovePerson();
    }

    public void OnRightDownButtonDown()
    {
        if (speed >= 0f)
        {
            speed = normalSpeed;
        }

        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = new Vector3(transform.right.x * speed, -transform.up.y * speed / 2, 0);
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        MovePerson();
    }

    public void OnLeftTopButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = new Vector3(-transform.right.x * normalSpeed, transform.up.y * normalSpeed / 2, 0);
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        MovePerson();
    }

    public void OnLeftDownButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = false;
            lookDown = false;
            rb.velocity = new Vector3(-transform.right.x * normalSpeed, -transform.up.y * normalSpeed / 2, 0);
            sprite.flipX = speed <= 0f;
            State = States.run;
        }

        MovePerson();
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

        MovePerson();
    }

    private void MovePerson() {
        FindAnyObjectByType<AudioManager>().InteractionSound("RunCarpet", true);
        FindObjectOfType<ChangeLayerObject>().ChangeOrderLayerObject();
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

        FindAnyObjectByType<AudioManager>().InteractionSound("RunCarpet", false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        doorEnter = collider.CompareTag("Door") ? collider.gameObject : null;
        doorActive = collider.CompareTag("Door") ? true : false;
        enterNextFloor = collider.CompareTag("Floor");
        roomName = collider.gameObject.GetComponent<FrameSwitch>() ? collider.gameObject.GetComponent<FrameSwitch>().activeFrame.name : null;
        PlayerPrefs.SetString("taskTarget", collider.CompareTag("Task") ? collider.name : "");
        PlayerPrefs.Save();

        if (collider.CompareTag("Sound"))
        {
            StartCoroutine(AudioFade.FadeIn(FindAnyObjectByType<AudioManager>().GetSound("Birds"), 0.5f, Mathf.SmoothStep));
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        doorActive = false;
        roomName = collider.gameObject.GetComponent<FrameSwitch>() ? collider.gameObject.GetComponent<FrameSwitch>().activeFrame.name : null;
        PlayerPrefs.SetString("taskTarget", "");
        PlayerPrefs.Save();

        if (collider.CompareTag("Sound"))
        {
            StartCoroutine(AudioFade.FadeOut(FindAnyObjectByType<AudioManager>().GetSound("Birds"), 0.5f, Mathf.SmoothStep));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PopupOpen")
        {
            enterPopupObject = true;
            popupObject = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PopupOpen")
        {
            enterPopupObject = false;
            popupObject = null;
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

        FindAnyObjectByType<AudioManager>().InteractionSound("Snort", true);
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

                        if (gameObjects[i].name.Contains("bottle"))
                        {
                            FindAnyObjectByType<AudioManager>().InteractionSound("BottlePickup", true);
                        }

                        if (gameObjects[i].name.Contains("trashPaper"))
                        {
                            FindAnyObjectByType<AudioManager>().InteractionSound("PaperCrunchy", true);
                        }

                        FindAnyObjectByType<AudioManager>().InteractionSound("BagAction", true);

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
                        FindAnyObjectByType<AudioManager>().InteractionSound("MopAction", true);

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

                    break;
                }
            }
        }

        if (enterPopupObject)
        {
            if (popupObject.GetComponent<Popup>().popup.name.Contains("Closet"))
            {
                FindAnyObjectByType<AudioManager>().InteractionSound("ClosetOpen", true);
            } else
            {
                FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
            }
            
            popupObject.GetComponent<Popup>().popup.SetActive(true);
            stopRunning = true;
            popupIsOpen = popupObject.GetComponent<Popup>().popup.name;
        }

        var taskInfo = JsonHelper.GetJsonValue(roomName);
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countToiletPaper = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("ToiletPaper")) == "" ? "0" : PlayerPrefs.GetString("task" + LayerMask.NameToLayer("ToiletPaper"));
        countTowels = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Towels")) == "" ? "0" : PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Towels"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();

        if (roomName != null && taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash || int.Parse(countPuddle) < taskInfo.removePuddle || int.Parse(countToiletPaper) < taskInfo.toiletPaper || int.Parse(countTowels) < taskInfo.towels ? false : true;
        }

        if (doorActive && tasksComplete)
        {
            if (FindObjectOfType<Door>())
            {
                FindAnyObjectByType<AudioManager>().InteractionSound("DoorOpen", true);
                FindObjectOfType<Door>().OpenDoor(doorEnter);
                StartCoroutine(OpenDoor());
            }
            else
            {
                FindObjectOfType<FrameSwitch>().OpenDoor();
                FindObjectOfType<Door>().CheckDoorAccess();
            }
        } else if (doorActive && !tasksComplete)
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("DoorLock", true);
        }
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<FrameSwitch>().OpenDoor();

        if (FindObjectOfType<Door>())
        {
            FindObjectOfType<Door>().CheckDoorAccess();
        }        
    }

    IEnumerator destroyTask(int task)
    {
        yield return new WaitForSeconds(1);
        FindAnyObjectByType<AudioManager>().InteractionSound("MopAction", false);
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

        FindAnyObjectByType<AudioManager>().InteractionSound("ElevatorArrive", true);
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
