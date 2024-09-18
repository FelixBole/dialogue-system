using UnityEngine;
using UnityEngine.Localization;

namespace Slax.Dialogue
{
    /// <summary>
    /// A choice that can be made by the player in a dialogue.
    /// 
    /// <para>
    /// Each choice has a text that will be displayed to the player, and a dialogue
    /// that will be played if the player chooses this choice.
    /// </para>
    /// </summary>
    [CreateAssetMenu(menuName = "Slax/Dialogue/Dialogue Choice", fileName = "New Dialogue Choice Data")]
    public class DialogueChoiceSO : ScriptableObject
    {
        [SerializeField] protected LocalizedString _choiceText;
        [SerializeField] protected DialogueSO _nextDialogue;

        /// <summary>
        /// The text that will be displayed to the player for this choice.
        /// </summary>
        public LocalizedString ChoiceText => _choiceText;

        /// <summary>
        /// The dialogue that will be played if the player chooses this choice.
        /// 
        /// <para>
        /// If this is not set, and the dialogue containing this choice has a next
        /// dialogue set, the next dialogue from the origin will be played. This can
        /// be useful if both choices lead to the same dialogue, but have different
        /// side effects.
        /// </para>
        /// </summary>
        public DialogueSO NextDialogue => _nextDialogue;
    }
}