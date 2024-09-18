using UnityEngine;
using UnityEngine.Localization;

namespace Slax.Dialogue
{
    /// <summary>
    /// Represents an expression that an actor can have during a dialogue line.
    /// </summary>
    [CreateAssetMenu(fileName = "ActorExpression", menuName = "Slax/Dialogue/Actor Expression")]
    public class ActorExpressionSO : ScriptableObject
    {
        [SerializeField, TextArea] protected string _note;
        [SerializeField] protected int _actorId;
        [SerializeField] protected bool _useLocalizedExpressionSprite = false;
        [SerializeField] protected LocalizedSprite _localizedExpressionSprite;
        [SerializeField] protected Sprite _fallbackExpressionSprite;
        [SerializeField] protected AnimationClip _expressionAnimation;
        [SerializeField] protected float _expressionDuration;
        [SerializeField] protected float _transitionDuration = 0f;

        /// <summary>
        /// The actor identifier that the expression belongs to.
        /// 
        /// <para>
        /// This should be a one-to-one relationship with the actor's int identifier
        /// in order to be able to find the actor that the expression belongs to.
        /// </para>
        /// </summary>
        public int ActorId => _actorId;

        /// <summary>
        /// The sprite that represents the expression.
        /// </summary>
        public Sprite ExpressionSprite => _useLocalizedExpressionSprite ? _localizedExpressionSprite.LoadAsset() : _fallbackExpressionSprite;

        /// <summary>
        /// The animation that should be played when the expression is shown.
        /// </summary>
        public AnimationClip ExpressionAnimation => _expressionAnimation;

        /// <summary>
        /// The duration of the expression.
        /// </summary>
        public float ExpressionDuration => _expressionDuration;

        /// <summary>
        /// The duration of the transition to the expression.
        /// </summary>
        public float TransitionDuration => _transitionDuration;
    }
}
