using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.IO;

public class Person : MonoBehaviour
{
    private Inventory inventory;
    private FrameSwitch frameSwitch;

    [SerializeField] public float normalSpeed = 3f;
    [SerializeField] public Count countClass;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private GameObject[] gameObjects;
    public GameObject cartMenu;
    public bool cartMenuIsOpen = false;
    private bool enterCartMenu = false;
    private bool doorActive = false;
    private float speed;
    private string taskTarget = null;
    private string roomName = null;
    private int tasksCount;
    private bool lookUp = false;
    public bool stopRunning = false;
    private bool enterNextFloor = false;
    private Text text;
    public GameObject newFrame;
    public Vector3 doorEnterPoint;
    private string jsonPath = "tasks.json";
    private int currentTaskCount = 0;

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
            rb.velocity = -transform.up * normalSpeed;
            State = States.run;
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
        } else
        {
            State = States.idle;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        taskTarget = collider.CompareTag("Task") ? collider.name : null;
        doorActive = collider.tag == "Door" ? true : false;
        enterNextFloor = collider.CompareTag("Floor");
        roomName = collider.name;

        PlayerPrefs.SetString("taskTarget", collider.CompareTag("Task") ? collider.name : "");
        PlayerPrefs.Save();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        doorActive = false;
        roomName = collider.name;      
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
    }

    public void Action()
    {
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
                        State = States.action;
                        StartCoroutine(destroyTask(i));
                    } else if (gameObjects[i] && LayerMask.NameToLayer("Trash") == gameObjects[i].layer && !bagIsExist)
                    {
                        // No bag in inventory
                        print("No bag");
                        CloudAnimation("bag");
                    }
                    else if (gameObjects[i] && LayerMask.NameToLayer("Puddle") == gameObjects[i].layer && mopIsExist)
                    {
                        // Using a mop
                        stopRunning = true;
                        State = States.action;
                        StartCoroutine(destroyTask(i));
                    } else if (gameObjects[i] && LayerMask.NameToLayer("Puddle") == gameObjects[i].layer && !mopIsExist)
                    {
                        // No mop in inventory
                        print("No mop");
                        CloudAnimation("mop");
                    } else if (gameObjects[i] && LayerMask.NameToLayer("Trash") != gameObjects[i].layer)
                    {
                        // Using other stuff in inventory
                        stopRunning = true;
                        State = States.action;
                        StartCoroutine(destroyTask(i));
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

        bool tasksComplete = true;
        TextAsset txtAsset = (TextAsset)Resources.Load("tasks", typeof(TextAsset));
        string levelJson = JsonHelper.GetJsonObject(txtAsset.ToString(), "level1");
        string roomJson = JsonHelper.GetJsonObject(levelJson, "room" + roomName.Replace("EdgeEnterRoom", ""));

        RoomInfo info = JsonUtility.FromJson<RoomInfo>(roomJson);

        if (roomName.Contains("EdgeEnterRoom") && roomJson != null)
        {
            tasksComplete = countClass.count >= info.tasks ? true : false;
        }

        if (doorActive && tasksComplete)
        {
            FindObjectOfType<FrameSwitch>().OpenDoor();
        }
    }

    IEnumerator destroyTask(int task)
    {
        yield return new WaitForSeconds(1);
        countClass.countChange();
        Destroy(gameObjects[task]);
        print("destroyTask");
        taskTarget = "";

        if (countClass.count < 1)
        {
            Death();
        } else
        {
            State = States.idle;
        }

        stopRunning = false;
    }

    public void Death()
    {
        State = States.death;
        print("Death");
    }

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        speed = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        gameObjects = GameObject.FindGameObjectsWithTag("Task");
    }

    void Update()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject.FindGameObjectWithTag("Cloud").gameObject.transform.position = new Vector2(player.position.x + 2, player.position.y + 3);
    }
}

public enum States {
    idle,
    run,
    runUp,
    swiming,
    death,
    looksUp,
    action
}

[System.Serializable]
public class RoomInfo
{
    public int tasks;
    public bool access;
}