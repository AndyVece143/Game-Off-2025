using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemybullet")
        {
            int damage = collision.gameObject.GetComponent<EnemyBullet>().damage;
            GetComponentInParent<Player>().TakeDamage(damage);
            GetComponentInParent<Player>().StartKnockback(collision.gameObject.transform.right);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "powerup")
        {
            GetComponentInParent<Player>().isBulletWave = true;
            GetComponentInParent<Player>().bulletTimer = 10;
        }
    }
}
