using UnityEngine;

namespace Slax.Dialogue
{
    public abstract class DialogueStartConditionSO : ScriptableObject, IDialogueStartCondition
    {
        public virtual bool CanStartDialogue()
        {
            return true;
        }
    }
}
