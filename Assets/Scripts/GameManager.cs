using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float peaceTime;
    [SerializeField] private float battleTime;
    private float peaceTimeMax;
    private float battleTimeMax;
    public bool inBattle;
    public TMP_Text timerText;
    public MusicManager music;
    public bool isTimerPaused;
    public int rounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        peaceTimeMax = peaceTime;
        battleTimeMax = battleTime;
        rounds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TimerText();
        if (inBattle && !isTimerPaused)
        {
            battleTime -= Time.deltaTime;
            if (battleTime <= 0)
            {
                SwitchModes();
                rounds++;
            }
        }
        else if (!inBattle && !isTimerPaused)
        {
            peaceTime -= Time.deltaTime;
            if (peaceTime <= 0)
            {
                SwitchModes();
            }
        }
    }

    private void SwitchModes()
    {
        inBattle = !inBattle;
        peaceTime = peaceTimeMax;
        battleTime = battleTimeMax;
        music.SwitchSongs(inBattle);
    }

    private void TimerText()
    {
        if (inBattle)
        {
            int minutes = Mathf.FloorToInt(battleTime / 60);
            int seconds = Mathf.FloorToInt(battleTime % 60);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            //timerText.color = new Color(255, 123, 123, 255);
            timerText.color = Color.red;
        }
        else if (!inBattle)
        {
            int minutes = Mathf.FloorToInt(peaceTime / 60);
            int seconds = Mathf.FloorToInt(peaceTime % 60);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            timerText.color = Color.white;
        }
    }
}
