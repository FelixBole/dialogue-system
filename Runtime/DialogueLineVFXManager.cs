using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slax.Dialogue
{
    public class DialogueLineVFXManager : MonoBehaviour
    {
        bool _playOnActorPosition = true;

        protected virtual void OnEnable()
        {
            DialogueManager.Instance.OnLineVisualEffectPlayed += OnLineVisualEffectPlayed;
        }

        protected virtual void OnDisable()
        {
            DialogueManager.Instance.OnLineVisualEffectPlayed -= OnLineVisualEffectPlayed;
        }

        protected virtual void OnLineVisualEffectPlayed(Actor actor, GameObject visualEffect, float duration)
        {
            if (_playOnActorPosition)
            {
                visualEffect.transform.position = actor.transform.position;
                Instantiate(visualEffect, actor.transform.position, Quaternion.identity);
            }
            else
            {
                // Instantiate with the rotation that the prefab has
                Instantiate(visualEffect, actor.transform.position, visualEffect.transform.rotation);
            }
        }
    }
}
