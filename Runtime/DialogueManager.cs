using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Slax.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public UnityAction<Actor, DialogueSO> OnDialogueStart;
        public UnityAction<Actor, DialogueSO, DialogueLine> OnDialogueProgress;
        public UnityAction<Actor, DialogueSO> OnDialogueEnd;
        public UnityAction<Actor, DialogueSO> OnDialogueChoiceSelected;
        public UnityAction<DialogueSO> OnDialogueChoiceSelectionReady;
        public UnityAction<AudioClip> OnDialogueAudioClipPlayed;
        public UnityAction<AudioClip> OnLineSoundEffectPlayed;
        public UnityAction<Actor, GameObject, float> OnLineVisualEffectPlayed;

        public UnityEvent<DialogueManager> OnDialogueManagerReady;

        [SerializeField] protected AudioSource _audioSource;
        [SerializeField] protected bool _playAudioClipsFromManager = true;

        protected static DialogueManager _instance;
        protected Actor _currentActor;
        protected DialogueSO _currentDialogue;
        protected bool _isDialogueActive = false;
        protected int _currentLineIndex = 0;

        public static DialogueManager Instance => _instance;

        /// <summary>
        /// The actor that is currently in dialogue.
        /// </summary>
        public Actor CurrentActor => _currentActor;

        /// <summary>
        /// The dialogue that is currently being played.
        /// </summary>
        public DialogueSO CurrentDialogue => _currentDialogue;

        /// <summary>
        /// Whether a dialogue is currently active.
        /// </summary>
        public bool IsDialogueActive => _isDialogueActive;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;

            if (_audioSource == null && _playAudioClipsFromManager)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        protected virtual void OnEnable()
        {
            Actor.OnDialogueStartRequested += StartDialogue;

            OnDialogueManagerReady?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            Actor.OnDialogueStartRequested -= StartDialogue;
        }

        public virtual void SelectChoice(DialogueChoiceSO choice)
        {
            OnDialogueChoiceSelected?.Invoke(_currentActor, choice.NextDialogue);

            if (choice.NextDialogue != null)
            {
                NextDialogue(choice.NextDialogue);
            }
            else if (_currentDialogue.NextDialogue != null)
            {
                NextDialogue(_currentDialogue.NextDialogue);
            }
            else
            {
                EndDialogue();
            }
        }

        /// <summary>
        /// Starts a dialogue with the given actor.
        /// </summary>
        protected virtual void StartDialogue(Actor actor, DialogueSO dialogue)
        {
            if (_isDialogueActive)
            {
                Debug.LogWarning("A dialogue is already active");
                return;
            }

            _currentActor = actor;
            _currentDialogue = dialogue;
            _currentLineIndex = 0;

            _isDialogueActive = true;

            OnDialogueStart?.Invoke(actor, dialogue); // Dialogue UI and Actor listens to this event
            PlayLine(dialogue.Lines[_currentLineIndex]);
        }

        /// <summary>
        /// Ends the current dialogue.
        /// </summary>
        public virtual void ContinueDialogue()
        {
            if (!_isDialogueActive)
            {
                Debug.LogWarning("No dialogue is active");
                return;
            }

            _currentLineIndex++;

            bool hasNextLine = _currentLineIndex < _currentDialogue.Lines.Count;
            bool hasNextDialogue = _currentDialogue.NextDialogue != null;
            bool hasChoices = _currentDialogue.Choices.Count > 0;

            if (hasNextLine)
            {
                var nextLine = _currentDialogue.Lines[_currentLineIndex];
                OnDialogueProgress?.Invoke(_currentActor, _currentDialogue, nextLine);

                PlayLine(nextLine);
            }
            else if (hasChoices)
            {
                OnDialogueChoiceSelectionReady?.Invoke(_currentDialogue);
            }
            else if (hasNextDialogue)
            {
                NextDialogue(_currentDialogue.NextDialogue);
            }
            else
            {
                EndDialogue();
            }
        }

        IEnumerator PlayNextLineAutomaticallyAfterDelay(DialogueLine nextLine)
        {
            yield return new WaitForSeconds(nextLine.DisplayDuration);
            ContinueDialogue();
        }

        protected virtual void PlayLine(DialogueLine line)
        {
            // Play the audio clip of the line
            AudioClip audioClip = line.AudioClip;
            if (audioClip != null)
            {
                if (_playAudioClipsFromManager)
                {
                    _audioSource.PlayOneShot(audioClip);
                }
                else
                {
                    OnDialogueAudioClipPlayed?.Invoke(audioClip);
                }
            }

            if (line.Effects.Count > 0)
            {
                // Apply the effects of the line
                foreach (var effect in line.Effects)
                {
                    StartCoroutine(PlayLineEffectsAfterDelay(effect, effect.Delay));
                }
            }

            // If the line has a display duration, continue the dialogue after that duration
            // instead of waiting for the player to press the continue button
            if (line.DisplayDuration > 0)
            {
                StartCoroutine(PlayNextLineAutomaticallyAfterDelay(line));
            }
        }

        protected virtual void EndDialogue()
        {
            if (!_isDialogueActive)
            {
                Debug.LogWarning("No dialogue is active");
                return;
            }

            StopAllCoroutines(); // Ensure all ongoing coroutines are stopped when dialogue ends
            OnDialogueEnd?.Invoke(_currentActor, _currentDialogue);

            _currentActor = null;
            _currentDialogue = null;
            _currentLineIndex = 0;
            _isDialogueActive = false;
        }

        protected virtual void NextDialogue(DialogueSO dialogue)
        {
            _isDialogueActive = false;
            StartDialogue(_currentActor, dialogue);
        }

        IEnumerator PlayLineEffectsAfterDelay(DialogueEffectSO effect, float delay)
        {
            if (delay < 0) delay = 0f;

            AudioClip soundEffect = effect.SoundEffect;
            GameObject vfxPrefab = effect.VisualEffectPrefab;

            yield return new WaitForSeconds(delay);

            if (soundEffect != null)
            {

                if (_playAudioClipsFromManager)
                {
                    _audioSource.PlayOneShot(soundEffect);
                }
                else
                {
                    OnLineSoundEffectPlayed?.Invoke(soundEffect);
                }
            }

            if (vfxPrefab != null)
            {
                OnLineVisualEffectPlayed?.Invoke(_currentActor, vfxPrefab, effect.Duration);
            }
        }
    }
}
