using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ManagerAudio : MonoBehaviour
{
    #region Fields

    public static ManagerAudio Instance;

    public AudioSource AudioSource;

    [Space(20)]
    public AudioClip SoundAmbient;

    public AudioClip SoundVictory;

    #endregion Fields

    #region Unity

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayAmbient();
    }

    #endregion Unity

    #region Methods

    public void PlayAmbient() => ChangeCurrentSound(SoundAmbient);

    public void PlayVictory()
    {
        AudioSource.loop = false;
        ChangeCurrentSound(SoundVictory);
    }

    private void ChangeCurrentSound(AudioClip newSound)
    {
        AudioSource.Stop();
        AudioSource.clip = newSound;
        AudioSource.Play();
    }

    #endregion Methods
}