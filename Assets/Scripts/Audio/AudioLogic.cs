using System.Collections;
using UnityEngine;
public class AudioLogic : MonoBehaviour
{
    bool _cancellSFX;
    bool _cancellMusic;

    private AudioSource cameraAudioSource;

    [SerializeField] private AudioData audioData;

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

    void OnBlockDestroySFX()
    {
        if (isPlaying  || _cancellSFX)
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

    public void CancellSFXCall(bool play) { _cancellSFX = play; }
    public void CancellMusicCall(bool play) { _cancellMusic = play; }
}
