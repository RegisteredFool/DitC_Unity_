using System;
using UnityEngine;

public class PowerupGranter_Basic : MonoBehaviour
{
    public int powerup;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private float volume;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().BeginFire();
            //audioSource.clip = hitSFX;
            //audioSource.Play();
            SFXManager.instance.PlaySound(hitSFX, transform.position, volume);
            Destroy(gameObject);
        }
    }
}
