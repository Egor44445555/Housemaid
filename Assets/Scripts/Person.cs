using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

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
    private string taskTarget;
    private int tasksCount;
    private bool lookUp = false;
    public bool stopRunning = false;
    private bool enterNextFloor = false;
    private Text text;
    public GameObject newFrame;
    public Vector3 doorEnterPoint;

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
        float playerPositionY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        float cartPositionY = GameObject.FindGameObjectWithTag("Cart").transform.position.y;

        if (playerPositionY > cartPositionY)
        {
            GameObject.FindGameObjectWithTag("Cart").GetComponent<SpriteRenderer>().sortingOrder = 11;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Cart").GetComponent<SpriteRenderer>().sortingOrder = 0;
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
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        doorActive = false;
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

    public void Action()
    {
        if (doorActive)
        {
            FindObjectOfType<FrameSwitch>().OpenDoor();
        }

        if (!stopRunning)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] && taskTarget == gameObjects[i].name)
                {
                    bool bagIsExist = false;

                    for (int j = 0; j < inventory.stuff.Length; j++)
                    {
                        if (inventory.stuff[j] && inventory.stuff[j].name == "bag")
                        {
                            bagIsExist = true;
                        }
                    }

                    if (gameObjects[i] && LayerMask.NameToLayer("Trash") == gameObjects[i].layer && bagIsExist)
                    {
                        // Using a broom
                        stopRunning = true;
                        State = States.action;
                        StartCoroutine(destroyTask(i));
                    } else if (gameObjects[i] && LayerMask.NameToLayer("Trash") == gameObjects[i].layer && !bagIsExist)
                    {
                        // No bag in inventory
                        State = States.run;
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

        if (enterNextFloor)
        {
            //SceneManager.LoadScene(2);
        }
    }

    IEnumerator destroyTask(int task)
    {
        yield return new WaitForSeconds(1);
        countClass.countChange();
        Destroy(gameObjects[task]);
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
        bool isWater = Physics2D.OverlapCircle(GameObject.Find("Foothold").GetComponent<Transform>().position, 0.2f, LayerMask.GetMask("Water"));

        if (isWater)
        {
            State = States.swiming;
        }
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