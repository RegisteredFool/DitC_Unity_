using UnityEditor.Tilemaps;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource sfxObject;
    public static SFXManager instance;
    [SerializeField] private float globalVol;
    void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip sfx, Vector3 pos, float vol)
    {
        AudioSource audioSource = Instantiate(sfxObject, pos, Quaternion.identity);
        audioSource.clip = sfx;
        audioSource.volume = vol * globalVol;
        float clipLength = audioSource.clip.length;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);

        //AudioSource.PlayClipAtPoint(sfx, pos, 1f);
    }
    public void PlaySoundAdjust(AudioClip sfx, Vector3 pos, float vol, float pit)
    {
        AudioSource audioSource = Instantiate(sfxObject, pos, Quaternion.identity);
        audioSource.clip = sfx;
        audioSource.volume = vol * globalVol;
        audioSource.pitch = pit;
        float clipLength = audioSource.clip.length;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);

        //AudioSource.PlayClipAtPoint(sfx, pos, 1f);
    }
}
