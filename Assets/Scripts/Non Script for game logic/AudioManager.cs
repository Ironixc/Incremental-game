using UnityEngine;
using System.Collections; // <--- This is required for Coroutines!

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Background Music Playlist")]
    public AudioClip[] backgroundMusicList;

    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip GoalSound;
    public AudioClip DefeatSound;

    void Start()
    {
        bgmSource.loop = false;
        PlayNextSong();
    }

    void Update()
    {
        if (!bgmSource.isPlaying && backgroundMusicList.Length > 0)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        if (backgroundMusicList.Length == 0) return;

        int randomIndex = Random.Range(0, backgroundMusicList.Length);
        bgmSource.clip = backgroundMusicList[randomIndex];
        bgmSource.Play();
    }

    // --- SFX Trigger Methods ---

    public void PlayClick()
    {
        if (buttonClickSound != null) sfxSource.PlayOneShot(buttonClickSound);
    }

    public void GoalSoundPick()
    {
        if (GoalSound != null)
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = GoalSound;
            tempSource.Play();

            StartCoroutine(DestroySourceRealTime(tempSource, 1.5f));
        }
    }

    private IEnumerator DestroySourceRealTime(AudioSource source, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (source != null)
        {
            Destroy(source);
        }
    }

    public void DefeatSoundPick()
    {
        if (DefeatSound != null) sfxSource.PlayOneShot(DefeatSound);
    }
}