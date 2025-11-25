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
    public bool isDodging;
    private float dodgeTimer;
    private float cooldownTimer;

    public Animator anim;

    private float angle;

    public GameObject gun;
    private bool isMoving;

    [SerializeField] float knockbackTimer;
    private bool inKnockback;
    public Vector2 knockbackDirection;
    [SerializeField] float knockbackSpeed;

    public bool isTalking;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
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
            gun.GetComponent<SpriteRenderer>().enabled = false;
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

        //Knockback
        if (inKnockback)
        {
            gun.GetComponent<SpriteRenderer>().enabled = false;
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                inKnockback = false;
                anim.SetBool("knockback", false);
                body.linearVelocity = Vector3.zero;
            }
            else
            {
                body.linearVelocity = knockbackDirection * (knockbackSpeed);
            }
        }

        //Talking
        if (isTalking)
        {
            gun.GetComponent<SpriteRenderer>().enabled = false;
            anim.SetBool("talk", true);
        }

        else
        {
            anim.SetBool("talk", false);
            gun.GetComponent<SpriteRenderer>().enabled = true;
            //Body movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
            direction = body.linearVelocity.normalized;

            //Flip Sprite
            if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = Vector3.one;
            }

            //Arm Rotation
            Vector3 mousePos = Input.mousePosition;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(arm.transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            //arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Facing right
            if (IsFacingRight())
            {
                arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                if (angle > -90 && angle < 90)
                {
                    arm.transform.localScale = Vector3.one;
                }
                else if (angle > 90 || angle < -90)
                {
                    arm.transform.localScale = new Vector3(1, -1, 1);
                }
            }
            //Facing left
            else if (!IsFacingRight())
            {
                arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
                if ((angle) > -90 && (angle) < 90)
                {
                    arm.transform.localScale = new Vector3(1, -1, 1);
                }
                else if ((angle) > 90 || (angle) < -90)
                {
                    arm.transform.localScale = Vector3.one;
                }
            }
            isMoving = IsMoving(horizontalInput, verticalInput);

            //Animation
            anim.SetBool("move", horizontalInput != 0 || verticalInput !=0);
        }

        anim.SetBool("knockback", inKnockback);
    }

    //Method that stores all button inputs
    private void ButtonPresses()
    {
        //Shooting Input
        if (Input.GetMouseButtonDown(0) && !isDodging && !isTalking)
        {
            if (isBulletWave == false)
            {
                if (IsFacingRight())
                {
                    Bullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, arm.transform.rotation);
                }
                else if (!IsFacingRight())
                {
                    Bullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
                }
                
            }
            else
            {
                if (IsFacingRight())
                {
                    Bullet newBullet = Instantiate(waveBeam, bulletSpawn.transform.position, arm.transform.rotation);
                }
                else if (!IsFacingRight())
                {
                    Bullet newBullet = Instantiate(waveBeam, bulletSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
                }
            }
        }

        //Dodge Roll Input
        if (Input.GetKeyDown(KeyCode.Q) && !isDodging && cooldownTimer <= 0 && isMoving)
        {
            isDodging = true;
            dodgeTimer = 0.5f;
            cooldownTimer = 0.6f;
            anim.Play("roll");
        }
    }

    public void StartKnockback(Vector2 bulletDirection)
    {
        inKnockback = true;
        //knockbackDirection = transform.position - bulletPosition.normalized;
        knockbackDirection = bulletDirection;
        knockbackTimer = 0.3f;
        anim.Play("hurt");
    }

    //Determines which way the player is facing
    private bool IsFacingRight()
    {
        if (transform.localScale.x == -1)
        {
            return true;
        }
        return false;
    }

    //Determines if the player is moving
    private bool IsMoving(float hInput, float vInput)
    {
        if (hInput != 0 || vInput != 0)
        {
            return true;
        }
        return false;
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

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void GetPowerup()
    {
        isBulletWave = true;
        bulletTimer = 10;
    }


    //All trigger collisions
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //    if (collision.tag == "powerup")
    //    {
    //        isBulletWave = true;
    //        bulletTimer = 10;
    //    }

    //    if (collision.tag == "enemybullet")
    //    {
    //        int damage = collision.gameObject.GetComponent<EnemyBullet>().damage;
    //        Destroy(collision.gameObject);
    //        health -= damage;
    //    }
    //}
}
