using System.Threading;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
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
    }

    private void ArmRotation()
    {
        if (engagingPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Debug.Log(angle);
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

        else
        {
            arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

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

    private void ShootTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
        {
            Shoot();
            shootTimer = 3;
        }
    }

    private void Shoot()
    {
        EnemyBullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, arm.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet")
        {
            Destroy(collision.gameObject);
            health--;
        }
    }
}
