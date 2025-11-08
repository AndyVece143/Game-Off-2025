using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    public GameObject arm;

    public Bullet bullet;
    public Bullet waveBeam;
    public GameObject bulletSpawn;
    public float bulletTimer;

    public bool isBulletWave;

    public int health;

    private Vector2 direction;
    private bool isDodging;
    private float dodgeTimer;
    private float cooldownTimer;


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
        BulletTimer();
    }

    //Method for all movement related code
    private void Movement()
    {
        //Dodge Roll cooldown
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        //Dodge Roll
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0)
            {
                isDodging = false;
                body.linearVelocity = Vector3.zero;
            }
            else
            {
                body.linearVelocity = direction * (speed * 2);
            }
        }

        else
        {
            //Body movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
            direction = body.linearVelocity.normalized;

            //Arm Rotation
            Vector3 mousePos = Input.mousePosition;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(arm.transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Body flipping
            if (angle > -90 && angle < 90)
            {
                arm.transform.localScale = Vector3.one;
            }
            else if (angle > 90 || angle < -90)
            {
                arm.transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }

    //Method that stores all button inputs
    private void ButtonPresses()
    {
        //Shooting Input
        if (Input.GetMouseButtonDown(0) && !isDodging)
        {
            if (isBulletWave == false)
            {
                Bullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, arm.transform.rotation);
            }
            else
            {
                Bullet newBullet = Instantiate(waveBeam, bulletSpawn.transform.position, arm.transform.rotation);
            }
        }

        //Dodge Roll Input
        if (Input.GetKeyDown(KeyCode.Q) && !isDodging && cooldownTimer <= 0)
        {
            isDodging = true;
            dodgeTimer = 0.5f;
            cooldownTimer = 0.6f;
        }
    }

    //Timer for how long the wave beam lasts
    private void BulletTimer()
    {
        if (isBulletWave)
        {
            bulletTimer -= Time.deltaTime;
            if (bulletTimer <= 0)
            {
                isBulletWave = false;
            }
        }
        else
        {
            bulletTimer = 0;
        }
    }

    //All trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Powerup
        if (collision.tag == "powerup")
        {
            isBulletWave = true;
            bulletTimer = 10;
        }

        //Enemy bullet
        if (collision.tag == "enemybullet")
        {
            int damage = collision.gameObject.GetComponent<EnemyBullet>().damage;
            Destroy(collision.gameObject);
            health -= damage;
        }
    }
}
