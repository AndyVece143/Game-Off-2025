using Pathfinding;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    public GameObject arm;

    public EnemyBullet bullet;
    public GameObject bulletSpawn;

    public float shootTimer;

    public Player player;

    private bool engagingPlayer;

    [SerializeField] private TMP_Text healthText;

    private float angle;

    public Animator anim;
    public GameObject gun;

    [SerializeField] float knockbackTimer;
    private bool inKnockback;
    public Vector2 knockbackDirection;
    [SerializeField] float knockbackSpeed;

    public AIPath aiPath;

    public AIDestinationSetter destination;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        destination.target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ArmRotation();
        PlayerDistance();
        ShootTimer();

        healthText.text = "" + health;
    }

    private void Movement()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (inKnockback)
        {
            aiPath.canMove = false;
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

        else
        {
            aiPath.canMove = true;
            gun.GetComponent<SpriteRenderer>().enabled = true;
            
            //Flip Sprite
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (aiPath.desiredVelocity.x <= 0.01f)
            {
                transform.localScale = Vector3.one;
            }
            anim.SetBool("move", aiPath.desiredVelocity.x != 0 || aiPath.desiredVelocity.y != 0);
        }
        anim.SetBool("knockback", inKnockback);
    }

    //Method that rotates the arm in relation to the player
    private void ArmRotation()
    {
        if (engagingPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Facing Right
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
                healthText.transform.localScale = new Vector3(-1, 1, 1);
            }

            //Facing Left
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
                healthText.transform.localScale = Vector3.one;
            }
        }

        else
        {
            arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    //Determines which way the enemy is facing
    private bool IsFacingRight()
    {
        if (transform.localScale.x == -1)
        {
            return true;
        }
        return false;
    }

    //Method that determines the distance between the enemy and player
    private void PlayerDistance()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distance);

        if (distance <= 9)
        {
            engagingPlayer = true;
        }
        else
        {
            engagingPlayer= false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void StartKnockback(Vector2 bulletDirection)
    {
        inKnockback = true;
        knockbackDirection = bulletDirection;
        knockbackTimer = 0.3f;
        anim.Play("hurt");
    }

    //Method that adds a cooldown between shooting
    private void ShootTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0 && engagingPlayer)
        {
            Shoot();
            shootTimer = 3;
        }
    }

    //Method that shoots
    private void Shoot()
    {
        
        if (IsFacingRight())
        {
            EnemyBullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, arm.transform.rotation);
        }

        else if (!IsFacingRight())
        {
            EnemyBullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(new Vector3(0,0,angle)));
        }
    }

    //All trigger collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet" || collision.tag == "wavebeam")
        {
            int damage = collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
            health -= damage;
        }
    }
}
