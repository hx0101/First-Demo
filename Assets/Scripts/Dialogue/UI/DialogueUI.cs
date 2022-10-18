using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Options")]
    public GameObject optionPanel;
    public RectTransform option;
    public GameObject optionPrefab;

    [Header("Data")]
    public DialogueData_SO currentData;
    int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }

    void ContinueDialogue()
    {
        if (!currentData.dialoguePieces[currentIndex].end)
        {
            UpdateDialogue(currentData.dialoguePieces[currentIndex + 1]);
        }
        else
        {
            dialoguePanel.SetActive(false);
            UIManager.GetInstance().Pop(false);
        }
    }

    public void UpdateDialogue(DialoguePiece piece)
    {
        currentIndex = currentData.dialoguePieces.IndexOf(piece);
        
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = "";
        mainText.text = piece.text;
        //mainText.DOText(piece.text, 1f);

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.gameObject.SetActive(true);
            
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }

        CreateOptions(piece);

        Debug.Log(currentIndex);
    }

    void CreateOptions(DialoguePiece piece)
    {
        if (option.childCount > 0)
        {
            for (int i = 0; i < option.childCount; i++)
            {
                Destroy(option.GetChild(i).gameObject);
            }
        }
        
        if (piece.options.Count > 0)
        {
            optionPanel.gameObject.SetActive(true);
            for (int i = 0; i < piece.options.Count; i++)
            {
                var option = Instantiate(optionPrefab, this.option);
                OptionUI optionUI = option.GetComponent<OptionUI>();
                optionUI.UpdateOption(piece, piece.options[i]);
            }
        }
        else
        {
            optionPanel.gameObject.SetActive(false);
        }
    }
}
