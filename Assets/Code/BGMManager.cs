using UnityEngine;

public class BGMKeep : MonoBehaviour
{
    public static BGMKeep instance;
    public AudioClip bgm;
    private AudioSource aud;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        aud = gameObject.AddComponent<AudioSource>();
        aud.loop = true;
        aud.playOnAwake = false;
    }

    void Start()
    {
        if (bgm != null && !aud.isPlaying)
        {
            aud.clip = bgm;
            aud.Play();
        }
    }
}