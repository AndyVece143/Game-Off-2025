using UnityEngine;

using System.Collections;

public class Girlfriend : MonoBehaviour
{
    public Player player;
    public Animator anim;
    public bool isTalking;
    public SpriteRenderer textIndicator;
    private Vector3 direction;
    private float distance;
    public string[] text1;

    public Dialogue dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        //textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (isTalking == false)
        {
            anim.SetBool("talk", false);
            //Flip Sprite
            direction = player.transform.position - transform.position;
            //Debug.Log(direction);
            if (direction.x < 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= 2)
            {
                textIndicator.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    isTalking = true;
                    player.isTalking = true;
                    textIndicator.enabled = false;
                    Dialogue newDialogue = Instantiate(dialogue);
                    newDialogue.lines = text1;
                }

            }
            else
            {
                textIndicator.enabled = false;
            }
        }
        else if (isTalking)
        {
            anim.SetBool("talk", true);
        }
    }
}
