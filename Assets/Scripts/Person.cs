using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Person : MonoBehaviour
{
    [SerializeField] public float normalSpeed = 3f;
    [SerializeField] public Count countClass;

    public GameObject cartMenu;
    public bool cartMenuIsOpen = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private GameObject[] gameObjects;
    private float speed;
    private string taskTarget;
    private int tasksCount;
    private bool lookUp = false;
    public bool stopRunning = false;
    private bool enterNextFloor = false;
    private bool enterCartMenu = false;
    Text text;

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
    }

    public void OnTopButtonDown()
    {

        if (!stopRunning)
        {
            lookUp = true;
            rb.velocity = transform.up * normalSpeed;
            State = States.runUp;
        }            
    }

    public void OnDownButtonDown()
    {
        if (!stopRunning)
        {
            lookUp = false;
            rb.velocity = -transform.up * normalSpeed;
            State = States.run;
        }            
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

        if (collider.CompareTag("Task"))
        {
            taskTarget = collider.name;
        }

        if (collider.CompareTag("Floor"))
        {
            enterNextFloor = true;
        }
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
        if (!stopRunning)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] && taskTarget == gameObjects[i].name)
                {
                    stopRunning = true;
                    State = States.action;

                    StartCoroutine(destroyTask(i));

                    break;
                }
            }
        }

        if (enterCartMenu)
        {
            cartMenu.SetActive(true);
            enterCartMenu = false;
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