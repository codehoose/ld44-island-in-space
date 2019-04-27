using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicBehaviour : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Awake()
    {
        var musicOn = PlayerPrefs.GetInt("music", 1) == 1;
        if (!musicOn)
        {
            var silence = audioMixer.FindSnapshot("Silence");
            audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[] { silence }, new float[] { 1 }, 0);
        }
        else
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }

    }
}
