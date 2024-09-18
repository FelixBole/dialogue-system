using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Slax.Dialogue
{
    /// <summary>
    /// The data of an actor in the game.
    /// 
    /// <para>
    /// An actor is a character in the game that can be interacted with. The actor
    /// can have a name, description, portrait, dialogues, and interaction conditions.
    /// </para>
    /// 
    /// <para>
    /// All properties are optional and can be used depending on the game's use case.
    /// If there is a need to extend this class for a game's specific use case, it is
    /// recommended to create a new class that inherits from this one.
    /// </para>
    /// </summary>
    [CreateAssetMenu(menuName = "Slax/Dialogue/Actor", fileName = "New Actor Data")]
    public class ActorSO : ScriptableObject
    {
        [SerializeField] protected string _id;
        [SerializeField] protected LocalizedString _name;
        [SerializeField] protected LocalizedString _description;
        [SerializeField] protected bool _useLocalizedPortrait = false;
        [SerializeField] protected LocalizedSprite _localizedPortrait;
        [SerializeField] protected Sprite _fallbackPortrait;
        [SerializeField] protected List<DialogueSO> _dialogues = new List<DialogueSO>();
        [SerializeField] protected List<IActorInteractionCondition> _interactionConditions = new List<IActorInteractionCondition>();
        [SerializeField] protected bool _useInteractionValidationCache = true;

        protected List<IActorInteractionCondition> _cachedValidatedConditions = new List<IActorInteractionCondition>();

        /// <summary>
        /// The unique identifier of the actor.
        /// </summary>
        public string Id => _id;

        /// <summary>
        /// The name of the actor.
        /// 
        /// <para>
        /// This is an optional field that can be used to provide more information
        /// depending on the game's use case.
        /// </para>
        /// </summary>
        public LocalizedString Name => _name;

        /// <summary>
        /// The description of the actor.
        /// 
        /// <para>
        /// This is an optional field that can be used to provide more information
        /// depending on the game's use case.
        /// </para>
        /// </summary>
        public LocalizedString Description => _description;

        /// <summary>
        /// The portrait of the actor.
        /// 
        /// <para>
        /// This is an optional field that can be used to provide more information
        /// depending on the game's use case.
        /// </para>
        /// </summary>
        public Sprite Portrait => _useLocalizedPortrait ? _localizedPortrait.LoadAsset() : _fallbackPortrait;

        /// <summary>
        /// All of the dialogues that the actor can play.
        /// </summary>
        public List<DialogueSO> Dialogues => _dialogues;

        /// <summary>
        /// The conditions that must be met in order for the actor to be interacted with.
        /// The conditions are checked in the order they are in the list.
        /// 
        /// <para>
        /// If empty, the actor can be interacted with immediately.
        /// </para>
        /// </summary>
        public List<IActorInteractionCondition> InteractionConditions => _interactionConditions;

        /// <summary>
        /// If true, the conditions that have been validated will be cached and not
        /// checked again until the cache is cleared.
        /// </summary>
        public bool UseInteractionValidationCache => _useInteractionValidationCache;

        /// <summary>
        /// The conditions that have been validated and cached.
        /// </summary>
        public List<IActorInteractionCondition> CachedValidatedConditions => _cachedValidatedConditions;

        /// <summary>
        /// Clears the cached validated conditions.
        /// </summary>
        public virtual void ClearCachedValidatedConditions()
        {
            _cachedValidatedConditions.Clear();
        }

        /// <summary>
        /// Checks if the actor can be interacted with.
        /// </summary>
        public virtual bool CanInteract()
        {
            foreach (var condition in _interactionConditions)
            {
                if (_useInteractionValidationCache)
                {
                    if (_cachedValidatedConditions.Contains(condition)) continue;

                    bool isConditionValidated = condition.CanInteractWithActor();

                    if (!isConditionValidated) return false;

                    _cachedValidatedConditions.Add(condition);
                }
                else
                {
                    if (!condition.CanInteractWithActor()) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds a dialogue in the actor's dialogues list.
        /// </summary>
        public virtual DialogueSO GetDialogue(string dialogueId) => _dialogues.Find(dialogue => dialogue.Id == dialogueId);
        public virtual DialogueSO GetDialogue(DialogueSO dialogue) => _dialogues.Find(d => d.Id == dialogue.Id);

        public virtual DialogueSO GetLastDialogue() => _dialogues.Count > 0 ? _dialogues[_dialogues.Count - 1] : null;

        public virtual DialogueSO GetFirstDialogue() => _dialogues.Count > 0 ? _dialogues[0] : null;

        public virtual DialogueSO GetNextDialogue(DialogueSO dialogue)
        {
            int index = _dialogues.IndexOf(dialogue);
            if (index == -1) return null;

            if (index + 1 >= _dialogues.Count) return null;

            return _dialogues[index + 1];
        }

        /// <summary>
        /// Gets the default dialogue of the actor, which is the first dialogue in the list.
        /// </summary>
        public virtual DialogueSO GetDefaultDialogue() => _dialogues.Count > 0 ? _dialogues[0] : null;
    }
}
