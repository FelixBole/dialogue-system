using UnityEngine;
using UnityEngine.Events;

namespace Slax.Dialogue
{
    /// <summary>
    /// Represents an actor in the game world.
    /// 
    /// <para>
    /// An actor is an entity in the game world that can be interacted with. This
    /// class provides the basic functionality of an actor, such as starting a dialogue
    /// with the actor.
    /// </para>
    /// </summary>
    public class Actor : MonoBehaviour
    {
        [SerializeField] protected Collider2D _collider;

        /// <summary>
        /// The data of the actor.
        /// </summary>
        [SerializeField] protected ActorSO _actorData;

        /// <summary>
        /// The tag of the object that can trigger the interaction with the actor.
        /// </summary>
        [SerializeField] protected string _triggerTag = "Player";

        /// <summary>
        /// Whether the actor uses a trigger to set itself as ready for interaction.
        /// </summary>
        [SerializeField] protected bool _useTriggerToInteract = true;
        protected bool _isReadyForInteraction = true;
        public bool IsReadyForInteraction => _isReadyForInteraction;

        /// <summary>
        /// Event that is triggered when a dialogue start is requested.
        /// 
        /// <para>
        /// This event is listened to by the dialogue manager to start and
        /// manage the dialogue with this specific actor.
        /// </para>
        /// </summary>
        public static UnityAction<Actor, DialogueSO> OnDialogueStartRequested;
        public ActorSO ActorData => _actorData;


        #region MonoBehaviour Methods

        protected virtual void Awake()
        {
            if (_collider == null) _collider = GetComponent<Collider2D>();
            if (_actorData == null)
            {
                Debug.LogError("Actor data is null");
                return;
            }

            if (_useTriggerToInteract) _isReadyForInteraction = false;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!_useTriggerToInteract) return;
            if (other.CompareTag(_triggerTag))
            {
                _isReadyForInteraction = true;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!_useTriggerToInteract) return;
            if (other.CompareTag(_triggerTag))
            {
                _isReadyForInteraction = false;
            }
        }

        #endregion

        /// <summary>
        /// Tries to start a dialogue with the actor by triggering the dialogue start event.
        /// 
        /// <para>
        /// This method will check if the actor can be interacted with, and if the
        /// dialogue with the given ID exists. If the actor can be interacted with,
        /// the dialogue will be started.
        /// </summary>
        public virtual Actor TryStartDialogue(string dialogueId)
        {
            if (!_isReadyForInteraction) return this;
            if (!_actorData.CanInteract()) return this;

            var dialogue = _actorData.GetDialogue(dialogueId);
            if (dialogue == null)
            {
                Debug.LogError($"Dialogue with ID {dialogueId} not found");
                return this;
            }

            OnDialogueStartRequested?.Invoke(this, dialogue);
            return this;
        }

        public virtual Actor TryStartDialogue(DialogueSO dialogue) => TryStartDialogue(dialogue.Id);

        public virtual DialogueSO PlayDefaultDialogue()
        {
            if (!_isReadyForInteraction) return null;
            if (!_actorData.CanInteract()) return null;

            var dialogue = _actorData.GetDefaultDialogue();
            if (dialogue == null)
            {
                Debug.LogError("No available dialogues in the actor's data.");
                return dialogue;
            }

            OnDialogueStartRequested?.Invoke(this, dialogue);
            return dialogue;
        }

        public virtual Actor SetActorData(ActorSO actorData)
        {
            _actorData = actorData;
            return this;
        }

        public virtual Actor SetTriggerTag(string triggerTag)
        {
            _triggerTag = triggerTag;
            return this;
        }

        public virtual Actor SetUseTriggerToInteract(bool useTriggerToInteract)
        {
            _useTriggerToInteract = useTriggerToInteract;
            return this;
        }

        public virtual Actor SetReadyForInteraction(bool isReadyForInteraction)
        {
            _isReadyForInteraction = isReadyForInteraction;
            return this;
        }
    }
}
