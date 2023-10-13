using System.Collections;
using UnityEngine;

namespace QuanticCollapse
{
    public class AudioLogic : MonoBehaviour
    {
        [SerializeField] private GenericEventBus _blockDestructionEventBus;
        [SerializeField] private GenericEventBus _audioSettingsChanged;

        [SerializeField] private AudioData _audioData;
        private AudioSource _cameraAudioSource;
        private GameProgressionService _gameProgression;

        private bool _cancelSfx;
        private bool _cancelMusic;
        private bool _isPlaying;

        private int _chainedSfx = 0;

        private void Awake()
        {
            _cameraAudioSource = gameObject.GetComponent<AudioSource>();
            _gameProgression = ServiceLocator.GetService<GameProgressionService>();

            _blockDestructionEventBus.Event += OnBlockDestroySFX;
            _audioSettingsChanged.Event += CheckAudioSettings;
        }

        private void OnDisable()
        {
            _blockDestructionEventBus.Event -= OnBlockDestroySFX;
            _audioSettingsChanged.Event -= CheckAudioSettings;
        }

        void CheckAudioSettings()
        {
            _cancelSfx = _gameProgression.CheckSFXOff();
            _cancelMusic = _gameProgression.CheckMusicOff();
        }

        void OnBlockDestroySFX()
        {
            if (_isPlaying || _cancelSfx)
                return;

            StartCoroutine(nameof(PlaySFX), _chainedSfx);

            if (_chainedSfx < _audioData.SfxClips.Length - 1)
                _chainedSfx++;
            else
                _chainedSfx = 0;
        }

        IEnumerator PlaySFX(int sfxIndex)
        {
            _isPlaying = true;
            _cameraAudioSource.clip = _audioData.SfxClips[sfxIndex];
            _cameraAudioSource.Play();
            yield return new WaitForSeconds(_audioData.SfxClips[sfxIndex].length);
            _isPlaying = false;
        }
    }
}