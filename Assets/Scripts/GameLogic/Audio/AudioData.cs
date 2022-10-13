using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ChainedSFXClips")]
    public class AudioData : ScriptableObject
    {
        public AudioClip[] sfxClips;
    }
}
