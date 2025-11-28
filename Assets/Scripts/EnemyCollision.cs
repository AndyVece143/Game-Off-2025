using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet")
        {
            int damage = collision.gameObject.GetComponent<Bullet>().damage;
            GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponentInParent<Enemy>().StartKnockback(collision.gameObject.transform.right);
            Destroy(collision.gameObject);
        }
    }
}
