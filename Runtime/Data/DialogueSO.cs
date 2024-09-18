using System.Collections.Generic;
using UnityEngine;

namespace Slax.Dialogue
{
    /// <summary>
    /// A ScriptableObject that represents a dialogue that can be played in the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Slax/Dialogue/Dialogue", fileName = "New Dialogue Data")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField] protected string _id;
        [SerializeField] protected List<DialogueLine> _lines = new List<DialogueLine>();
        [SerializeField] protected List<DialogueChoiceSO> _choices = new List<DialogueChoiceSO>();
        [SerializeField] protected DialogueSO _nextDialogue;
        [SerializeField] protected List<DialogueStartConditionSO> _startConditionSOs = new List<DialogueStartConditionSO>();
        [SerializeField] protected List<IDialogueStartCondition> _startConditions = new List<IDialogueStartCondition>();

        /// <summary>
        /// The unique identifier of the dialogue.
        /// </summary>
        public string Id => _id;

        /// <summary>
        /// The lines that will be played during this dialogue.
        /// 
        /// <para>
        /// If choices are set, since the choices lead to new dialogues, all the
        /// available lines will be played before the choices are presented to the
        /// player.
        /// </para>
        /// </summary>
        public List<DialogueLine> Lines => _lines;

        /// <summary>
        /// The choices that the player can make during this dialogue, each choice 
        /// has its own dialogue that will be played if the player chooses it.
        /// 
        /// <para>
        /// This means that if choices are set AND a next dialogue is set, the next
        /// dialogue will be ignored.
        /// </para>
        /// </summary>
        public List<DialogueChoiceSO> Choices => _choices;

        /// <summary>
        /// If set, this represents the dialogue that will be played after this one.
        /// 
        /// <para>
        /// If choices are set, this will be ignored as the choices will lead to new
        /// dialogues.
        /// </para>
        /// </summary>
        public DialogueSO NextDialogue => _nextDialogue;

        /// <summary>
        /// The conditions that must be met in order for this dialogue to start. The
        /// conditions are checked in the order they are in the list. This is an added
        /// layer of conditions that can be used in association with scriptable
        /// objects that run condition logic for easy drag and drop in the inspector.
        /// </summary>
        public List<DialogueStartConditionSO> StartConditionSOs => _startConditionSOs;

        /// <summary>
        /// The conditions that must be met in order for this dialogue to start. The
        /// conditions are checked in the order they are in the list.
        /// 
        /// <para>
        /// If empty, the dialogue will start immediately.
        /// </para>
        /// </summary>
        public List<IDialogueStartCondition> StartConditions => _startConditions;
    }
}
