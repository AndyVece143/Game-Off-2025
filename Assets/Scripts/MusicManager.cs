using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip peaceMusic;
    public AudioClip battleMusic;
    public AudioSource source;
    public GameManager manager;


    private void Start()
    {
        source.resource = peaceMusic;
        source.Play();
    }

    public void SwitchSongs(bool inBattle)
    {
        if (inBattle)
        {
            source.resource = battleMusic;
            source.Play();
            source.loop = true;
        }

        else if (!inBattle)
        {
            source.resource = peaceMusic;
            source.Play();
            source.loop = true;
        }
    }
}
