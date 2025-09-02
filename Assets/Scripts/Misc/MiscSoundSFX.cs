using UnityEngine;

public class MiscSoundSFX : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip titlemusicClip;
    public AudioClip shipmusicClip;
    public AudioSource beepSource;
    public AudioClip analogbeepClip;
    public AudioClip dingbeepClip;
     public AudioClip shutterbeepClip;
    public AudioSource ambientSource;
    public AudioClip ambientmusicClip;

    public void playAnalogBeep()
    {
        beepSource.clip = analogbeepClip;
        beepSource.time = 0f;
        beepSource.Play();
    }

    public void playDingBeep()
    {
        beepSource.clip = dingbeepClip;
        beepSource.time = 0f;
        beepSource.Play();
    }

    public void playShutterBeep()
    {
        beepSource.clip = shutterbeepClip;
        beepSource.time = 0f;
        beepSource.Play();
    }
    public void playTitleMusic()
    {
        if (musicSource.isPlaying == false)
        {
            if (musicSource.clip != titlemusicClip)
            {
                musicSource.clip = titlemusicClip;
                musicSource.time = 0f;
            }
            musicSource.Play();
        }
    }
    public void playShipMusic()
    {
        if ( musicSource.isPlaying == false)
        {
            if (musicSource.clip != shipmusicClip)
            {
                musicSource.clip = shipmusicClip;
                musicSource.time = 0f;
            }
            musicSource.Play();
        }
    }
    public void playAmbientMusic()
    {
        if (ambientSource.isPlaying == false)
        { 
            ambientSource.clip = ambientmusicClip;
            ambientSource.time = 0f;
            ambientSource.Play();
        }
    }
}
