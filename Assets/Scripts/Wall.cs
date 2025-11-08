using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet" || collision.tag == "enemybullet")
        {
            Destroy(collision.gameObject);
        }
    }
}