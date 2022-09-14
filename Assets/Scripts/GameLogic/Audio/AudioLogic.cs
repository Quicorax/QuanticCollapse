using System.Collections;
using UnityEngine;

public class AudioLogic : MonoBehaviour
{
    [SerializeField] private GenericEventBus _BlockDestructionEventBus;
    [SerializeField] private GenericEventBus _AudioSettingsChanged;
    
    [SerializeField] private AudioData _audioData;
    private AudioSource _cameraAudioSource;
    private GameProgressionService _gameProgression;
    
    private bool _cancellSFX;
    private bool _cancellMusic;
    private bool _isPlaying;

    private int _chainedSFX = 0;

    private void Awake()
    {
        _cameraAudioSource = gameObject.GetComponent<AudioSource>();
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        
        _BlockDestructionEventBus.Event += OnBlockDestroySFX;
        _AudioSettingsChanged.Event += CheckAudioSettings;
    }
    private void OnDisable()
    {
        _BlockDestructionEventBus.Event -= OnBlockDestroySFX;
        _AudioSettingsChanged.Event -= CheckAudioSettings;
    }

    void CheckAudioSettings()
    {
        _cancellSFX = _gameProgression.CheckSFXOff();
        _cancellMusic = _gameProgression.CheckMusicOff();
    }
    void OnBlockDestroySFX()
    {
        if (_isPlaying  || _cancellSFX)
            return;

        StartCoroutine(nameof(PlaySFX), _chainedSFX);

        if (_chainedSFX < _audioData.sfxClips.Length - 1)
            _chainedSFX++;
        else
            _chainedSFX = 0;
    }

    IEnumerator PlaySFX(int sfxIndex)
    {
        _isPlaying = true;
        _cameraAudioSource.clip = _audioData.sfxClips[sfxIndex];
        _cameraAudioSource.Play();
        yield return new WaitForSeconds(_audioData.sfxClips[sfxIndex].length);
        _isPlaying = false;
    }
}
