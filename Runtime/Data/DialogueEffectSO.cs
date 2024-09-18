using UnityEngine;
using UnityEngine.Localization;

namespace Slax.Dialogue
{
    /// <summary>
    /// A scriptable object that represents a dialogue effect.
    ///
    /// <para>
    /// A dialogue effect is something that happens when a dialogue line is displayed.
    /// This can be a sound effect, a visual effect, or anything else that you want to
    /// happen when a line is displayed.
    /// </para>
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueEffect", menuName = "Slax/Dialogue/Dialogue Effect")]
    public class DialogueEffectSO : ScriptableObject
    {
        [SerializeField] protected bool _useLocalizedSoundEffect = false;
        [SerializeField] protected LocalizedAudioClip _soundEffect;
        [SerializeField] protected AudioClip _fallbackSoundEffect;
        [SerializeField] protected GameObject _visualEffectPrefab;
        [SerializeField] protected float _duration = -1f;
        [SerializeField] protected float _delay = 0f;

        /// <summary>
        /// The sound effect that will be played when this effect is applied.
        /// 
        /// <para>
        /// If this is not set, no sound effect will be played.
        /// </para>
        /// </summary>
        public AudioClip SoundEffect => _useLocalizedSoundEffect ? _soundEffect.LoadAsset() : _fallbackSoundEffect;

        /// <summary>
        /// The visual effect that will be spawned when this effect is applied.
        /// 
        /// <para>
        /// If this is not set, no visual effect will be spawned.
        /// </para>
        /// </summary>
        public GameObject VisualEffectPrefab => _visualEffectPrefab;

        /// <summary>
        /// The duration of the effect.
        /// 
        /// <para>
        /// If this is set to a negative value, the effect will be applied until the
        /// player presses the continue button.
        /// </para>
        public float Duration => _duration;

        /// <summary>
        /// The delay before the effect is applied.
        /// </summary>
        public float Delay => _delay;
    }
}
