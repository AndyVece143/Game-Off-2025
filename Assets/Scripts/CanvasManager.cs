using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Player player;
    public TMP_Text healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = (int)(player.bulletTimer % 60);
        healthText.text = "Health: " + player.health;
    }
}