using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    public GameObject arm;

    public Bullet bullet;
    public GameObject bulletSpawn;

    public int health;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ButtonPresses();
    }

    private void Movement()
    {
        //Body movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        //Rotation
        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(arm.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (angle > -90 && angle < 90)
        {
            arm.transform.localScale = Vector3.one;
        }
        else if (angle > 90 || angle < -90)
        {
            arm.transform.localScale = new Vector3(1, -1, 1);
            //Debug.Log(angle);
        }
    }

    private void ButtonPresses()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, arm.transform.rotation);
        }
    }
}
