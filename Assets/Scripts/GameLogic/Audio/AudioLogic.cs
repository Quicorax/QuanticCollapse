using System.Collections;
using UnityEngine;

public class AudioLogic : MonoBehaviour
{
    [SerializeField] private GenericEventBus _BlockDestructionEventBus;
    [SerializeField] private AudioData _AudioData;
    
    private AudioSource cameraAudioSource;
    
    private bool _cancellSFX;
    private bool _cancellMusic;
    private bool _isPlaying;

    private int _chainedSFX = 0;

    private void Awake()
    {
        cameraAudioSource = gameObject.GetComponent<AudioSource>();

        _BlockDestructionEventBus.Event += OnBlockDestroySFX;
    }
    private void OnDisable()
    {
        _BlockDestructionEventBus.Event -= OnBlockDestroySFX;
    }

    void OnBlockDestroySFX()
    {
        if (_isPlaying  || _cancellSFX)
            return;

        StartCoroutine(nameof(PlaySFX), _chainedSFX);

        if (_chainedSFX < _AudioData.sfxClips.Length - 1)
            _chainedSFX++;
        else
            _chainedSFX = 0;
    }

    IEnumerator PlaySFX(int sfxIndex)
    {
        _isPlaying = true;
        cameraAudioSource.clip = _AudioData.sfxClips[sfxIndex];
        cameraAudioSource.Play();
        yield return new WaitForSeconds(_AudioData.sfxClips[sfxIndex].length);
        _isPlaying = false;
    }

    public void CancellSFXCall(bool play) { _cancellSFX = play; }
    public void CancellMusicCall(bool play) { _cancellMusic = play; }
}
