using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;

public class ConversationTrigger : MonoBehaviour
{
    [SerializeField] private Conversation conversation;

    // Start is called before the first frame update
    void Start()
    {
        ConversationController.Instance.StartConversation(conversation);
    }
}
