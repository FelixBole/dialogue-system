using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Slax.Dialogue
{
    /// <summary>
    /// A single line of dialogue that can be displayed in the UI, with optional audio and effects.
    /// </summary>
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] protected LocalizedString _lineText;
        [SerializeField] protected bool _useLocalizedAudioClip = false;
        [SerializeField] protected LocalizedAudioClip _audioClip;
        [SerializeField] protected AudioClip _fallbackAudioClip;
        [SerializeField] protected ActorExpressionSO _actorExpression;
        [SerializeField] protected List<DialogueEffectSO> _effects = new List<DialogueEffectSO>();
        [SerializeField] protected float _displayDuration = -1f;

        /// <summary>
        /// The text that will be displayed in the UI for this line.
        /// </summary>
        public LocalizedString LineText => _lineText;

        /// <summary>
        /// The audio clip that will be played when this line is displayed.
        /// 
        /// <para>
        /// If this is not set, no audio will be played.
        /// </para>
        /// </summary>
        public AudioClip AudioClip => _useLocalizedAudioClip ? _audioClip.LoadAsset() : _fallbackAudioClip;

        /// <summary>
        /// The expression that the actor will have when this line is displayed.
        /// 
        /// <para>
        /// If this is not set, the actor will keep the expression it had from the
        /// previous line.
        /// </para>
        /// </summary>
        public ActorExpressionSO ActorExpression => _actorExpression;

        /// <summary>
        /// The effects that will be applied when this line is displayed.
        /// 
        /// <para>
        /// If this is not set, no effects will be applied.
        /// </para>
        /// </summary>
        public List<DialogueEffectSO> Effects => _effects;

        /// <summary>
        /// The duration that this line will be displayed for.
        /// 
        /// <para>
        /// If this is set to a negative value, the line will be displayed until the
        /// player presses the continue button.
        /// </para>
        /// </summary>
        public float DisplayDuration => _displayDuration;
    }
}
