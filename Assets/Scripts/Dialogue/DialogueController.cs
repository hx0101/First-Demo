using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    bool canTalk = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.F))
        {
            UIManager.GetInstance().Push(new DialoguePanel());
            OpenDialogue();
            canTalk = false;
        }
    }

    void OpenDialogue()
    {
        DialogueUI.Instance.UpdateDialogueData(currentData);
        DialogueUI.Instance.UpdateDialogue(currentData.dialoguePieces[0]);
    }
}
