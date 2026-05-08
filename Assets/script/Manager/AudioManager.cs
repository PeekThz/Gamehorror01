using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource loopSource;
    public AudioSource uiSource;

    [Header("BGM")]
    public AudioClip bgmClip;

    [Header("SFX")]
    public AudioClip footstepClip;
    public AudioClip runClip;
    public AudioClip pickupClip;
    public AudioClip hideClip;
    public AudioClip monsterClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (bgmClip != null)
        {
            PlayBGM(bgmClip, true);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (loopSource == null || clip == null) return;

        if (loopSource.clip == clip && loopSource.isPlaying) return;

        loopSource.Stop();
        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        if (loopSource == null) return;

        loopSource.Stop();
        loopSource.loop = false;
        loopSource.clip = null;
    }

    public void PlayFootstep()
    {
        PlayLoop(footstepClip);
    }

    public void PlayRunLoop()
    {
        PlayLoop(runClip);
    }

    public void PlayPickup()
    {
        PlaySFX(pickupClip);
    }

    public void PlayHide()
    {
        PlaySFX(hideClip);
    }

    public void PlayMonster()
    {
        PlaySFX(monsterClip);
    }
}