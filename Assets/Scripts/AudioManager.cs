using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    [Header("环境声音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("Player音效")]
    public AudioClip[] walkStepClips;
    public AudioClip jumpClip;
    public AudioClip fallClip;
    public AudioClip dashClip;
    public AudioClip[] attackClip;

    [Header("Voice")]
    public AudioClip jumpVoice;
    public AudioClip[] dashVoice;
    public AudioClip[] attackVoice;

    AudioSource ambientSource;
    public AudioSource musicSource;
    AudioSource fxSorce;
    AudioSource playerSource;
    AudioSource voiceSource;
    private void Awake()
    {
        current = this;

        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSorce = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        current.StartMusic();
    }

    void Start()
    {
        voiceSource.volume = 0.5f;
    }

    public static void PlayerFootstepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);

        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayerJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.jumpVoice;
        current.voiceSource.Play();
    }

    public static void PlayerFallAudio()
    {
        current.playerSource.clip = current.fallClip;
        current.playerSource.Play();
    }

    public static void PlayerDashAudio()
    {
        current.playerSource.clip = current.dashClip;
        current.playerSource.Play();

        int index = Random.Range(0, current.dashVoice.Length);
        current.voiceSource.clip = current.dashVoice[index];
        current.voiceSource.Play();
    }

    public static void PlayerAttackAudio(int index)
    {
        current.playerSource.clip = current.attackClip[index];
        current.playerSource.Play();

        current.voiceSource.clip = current.attackVoice[index];
        current.voiceSource.Play();
    }

    void StartMusic()
    {
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.volume = 0.1f;
        current.musicSource.Play();
    }
}
