using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;

    private StatesPuddle StatePuddle
    {
        get
        {
            return (StatesPuddle)anim.GetInteger("state");
        }
        set
        {
            anim.SetInteger("state", (int)value);
        }
    }

    public void OnAction()
    {
        print("OnAction");
        StatePuddle = StatesPuddle.action;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                var p = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                var hit = Physics2D.Raycast(p, Vector2.zero);

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Puddle"))
                {
                    StatePuddle = StatesPuddle.action;
                    StartCoroutine(StartAnimation());
                }
            }
        }
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1);
        StatePuddle = StatesPuddle.idle;
    }
}

public enum StatesPuddle
{
    idle,
    action
}