using System.Collections;
using UnityEngine;
public class AudioLogic : MonoBehaviour
{
    public AudioData audioData;
    public AudioSource cameraAudioSource;
    [SerializeField] private GenericEventBus _BlockDestructionEventBus;

    int chainedSFX = 0;
    bool isPlaying;

    private void Awake()
    {
        cameraAudioSource = gameObject.GetComponent<AudioSource>();

        _BlockDestructionEventBus.Event += OnBlockDestroySFX;
    }
    private void OnDestroy()
    {
        _BlockDestructionEventBus.Event -= OnBlockDestroySFX;
    }

    public void OnBlockDestroySFX()
    {
        if (isPlaying /*|| !Config.sfxOn*/)
            return;

        StartCoroutine(nameof(PlaySFX), chainedSFX);

        if (chainedSFX < audioData.sfxClips.Length - 1)
            chainedSFX++;
        else
            chainedSFX = 0;
    }

    IEnumerator PlaySFX(int sfxIndex)
    {
        isPlaying = true;
        cameraAudioSource.clip = audioData.sfxClips[sfxIndex];
        cameraAudioSource.Play();
        yield return new WaitForSeconds(audioData.sfxClips[sfxIndex].length);
        isPlaying = false;
    }
}
