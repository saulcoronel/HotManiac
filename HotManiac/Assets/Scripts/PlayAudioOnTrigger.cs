using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    public AudioSource audioToPlay;  // arrastra aquí el audio que quieres reproducir
    public GameObject player;        // arrastra el objeto del jugador
    private bool hasPlayed = false;  // evita que se repita si no quieres

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.gameObject == player)
        {
            hasPlayed = true;
            if (audioToPlay != null)
            {
                audioToPlay.Play();
            }
        }
    }
}
