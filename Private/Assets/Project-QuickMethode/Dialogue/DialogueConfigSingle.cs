using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "dialogue-config-single", menuName = "Dialogue/Dialogue Config Single", order = 1)]
public class DialogueConfigSingle : ScriptableObject
{
    public List<DialogueDataText> Dialogue = new List<DialogueDataText>();

    public List<DialogueDataChoice> Choice = new List<DialogueDataChoice>();

    public bool ChoiceAvaible => Choice == null ? false : Choice.Count > 0;
}

#if UNITY_EDITOR

[CustomEditor(typeof(DialogueConfigSingle))]
public class DialogueSingleConfigEditor : Editor
{
    private const float POPUP_HEIGHT = 150f * 2;
    private const float LABEL_WIDTH = 65f;

    private DialogueConfigSingle m_target;

    private DialogueConfig m_dialogueConfig;
    private string m_debugError = "";

    private int m_dialogueCount = 0;
    private int m_choiceCount = 0;

    private List<string> m_authorName;
    private List<bool> m_dialogueDelayShow;
    private List<bool> m_dialogueTriggerShow;
    private List<bool> m_choiceAuthorShow;
    private List<bool> m_choiceTriggerShow;

    private Vector2 m_scrollDialogue;
    private Vector2 m_scrollChoice;

    private void OnEnable()
    {
        m_target = target as DialogueConfigSingle;
        //
        m_dialogueCount = m_target.Dialogue.Count;
        m_choiceCount = m_target.Dialogue.Count;
        //
        SetConfigFind();
    }

    public override void OnInspectorGUI()
    {
        if (m_debugError != "")
        {
            QUnityEditor.SetLabel(m_debugError, QUnityEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
            return;
        }
        //
        SetGUIGroupDialogue();
        //
        QUnityEditor.SetSpace(10f);
        //
        SetGUIGroupChoice();
        //
        QUnityEditor.SetDirty(m_target);
    }

    //

    private void SetConfigFind()
    {
        if (m_dialogueConfig != null)
            return;
        //
        var AuthorConfigFound = QUnityAssets.GetScriptableObject<DialogueConfig>("");
        //
        if (AuthorConfigFound == null)
        {
            m_debugError = "Config not found, please create one";
            Debug.Log("[Dialogue] " + m_debugError);
            return;
        }
        //
        if (AuthorConfigFound.Count == 0)
        {
            m_debugError = "Config not found, please create one";
            Debug.Log("[Dialogue] " + m_debugError);
            return;
        }
        //
        if (AuthorConfigFound.Count > 1)
            Debug.Log("[Dialogue] Config found more than one, get the first one found");
        //
        m_dialogueConfig = AuthorConfigFound[0];
        //
        if (m_dialogueConfig.Author.Count == 0)
        {
            m_debugError = "Author Config not have any data, please add one";
            Debug.Log("[Dialogue] " + m_debugError);
            return;
        }
        //
        //CONTINUE:
        //
        m_authorName = m_dialogueConfig.AuthorName;
        //
        m_dialogueDelayShow = new List<bool>();
        while (m_dialogueDelayShow.Count < m_target.Dialogue.Count) m_dialogueDelayShow.Add(false);
        //
        m_dialogueTriggerShow = new List<bool>();
        while (m_dialogueTriggerShow.Count < m_target.Dialogue.Count) m_dialogueTriggerShow.Add(false);
        //
        m_choiceAuthorShow = new List<bool>();
        while (m_choiceAuthorShow.Count < m_target.Choice.Count) m_choiceAuthorShow.Add(false);
        //
        m_choiceTriggerShow = new List<bool>();
        while (m_choiceTriggerShow.Count < m_target.Choice.Count) m_choiceTriggerShow.Add(false);
        //
        m_debugError = "";
    }

    private void SetGUIGroupDialogue()
    {
        QUnityEditor.SetLabel("DIALOGUE", QUnityEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        //COUNT:
        QUnityEditor.SetHorizontalBegin();
        QUnityEditor.SetLabel("Count", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
        //
        m_dialogueCount = QUnityEditor.SetField(m_dialogueCount);
        //
        if (QUnityEditor.SetButton("+"))
            m_dialogueCount++;
        //
        if (QUnityEditor.SetButton("-"))
            if (m_dialogueCount > 0)
                m_dialogueCount--;
        //
        QUnityEditor.SetHorizontalEnd();
        //COUNT:
        //
        while (m_dialogueCount > m_target.Dialogue.Count)
        {
            m_target.Dialogue.Add(new DialogueDataText(m_dialogueConfig.DelayDefault));
            m_dialogueDelayShow.Add(false);
            m_dialogueTriggerShow.Add(false);
            m_choiceAuthorShow.Add(false);
            m_choiceTriggerShow.Add(false);
        }
        while (m_dialogueCount < m_target.Dialogue.Count)
        {
            m_target.Dialogue.RemoveAt(m_target.Dialogue.Count - 1);
            m_dialogueDelayShow.RemoveAt(m_dialogueDelayShow.Count - 1);
            m_dialogueTriggerShow.RemoveAt(m_dialogueTriggerShow.Count - 1);
            m_choiceAuthorShow.RemoveAt(m_choiceAuthorShow.Count - 1);
            m_choiceTriggerShow.RemoveAt(m_choiceTriggerShow.Count - 1);
        }
        //
        QUnityEditor.SetSpace(10);
        //
        m_scrollDialogue = QUnityEditor.SetScrollViewBegin(m_scrollDialogue, QUnityEditor.GetGUIHeight(POPUP_HEIGHT));
        for (int i = 0; i < m_target.Dialogue.Count; i++)
        {
            //ITEM:
            //
            //ITEM - NUM:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel(i.ToString(), QUnityEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QUnityEditor.GetGUIWidth(25));
            //ITEM - NUM:
            //
            //ITEM - MAIN:
            QUnityEditor.SetVerticalBegin();
            //
            //ITEM - AUTHOR
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel("Author", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
            m_target.Dialogue[i].AuthorIndex = QUnityEditor.SetPopup(m_target.Dialogue[i].AuthorIndex, m_authorName);
            QUnityEditor.SetHorizontalEnd();
            //ITEM - AUTHOR
            //
            //ITEM - Dialogue:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel("Dialogue", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
            m_target.Dialogue[i].Dialogue = QUnityEditor.SetField(m_target.Dialogue[i].Dialogue);
            QUnityEditor.SetHorizontalEnd();
            //ITEM - Dialogue:
            //
            //ITEM - DELAY
            QUnityEditor.SetHorizontalBegin();
            if (QUnityEditor.SetButton("Delay", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH)))
                m_dialogueDelayShow[i] = !m_dialogueDelayShow[i];
            if (m_dialogueDelayShow[i])
            {
                QUnityEditor.SetVerticalBegin();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Alpha", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Dialogue[i].Delay.Alpha = QUnityEditor.SetField(m_target.Dialogue[i].Delay.Alpha);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Space", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Dialogue[i].Delay.Space = QUnityEditor.SetField(m_target.Dialogue[i].Delay.Space);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Mark", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Dialogue[i].Delay.Mark = QUnityEditor.SetField(m_target.Dialogue[i].Delay.Mark);
                QUnityEditor.SetHorizontalEnd();
                QUnityEditor.SetVerticalEnd();
            }
            else
            {
                QUnityEditor.SetLabel("Alpha: " + m_target.Dialogue[i].Delay.Alpha, null, QUnityEditor.GetGUIWidth(LABEL_WIDTH * 1.25f));
                QUnityEditor.SetLabel("Space: " + m_target.Dialogue[i].Delay.Space, null, QUnityEditor.GetGUIWidth(LABEL_WIDTH * 1.25f));
                QUnityEditor.SetLabel("Mark: " + m_target.Dialogue[i].Delay.Mark, null, QUnityEditor.GetGUIWidth(LABEL_WIDTH * 1.25f));
            }
            QUnityEditor.SetHorizontalEnd();
            //ITEM - DELAY
            //
            //ITEM - TRIGGER:
            QUnityEditor.SetHorizontalBegin();
            if (QUnityEditor.SetButton("Trigger", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH)))
                m_dialogueTriggerShow[i] = !m_dialogueTriggerShow[i];
            //
            if (m_dialogueTriggerShow[i])
            {
                QUnityEditor.SetVerticalBegin();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Code", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Dialogue[i].TriggerCode = QUnityEditor.SetField(m_target.Dialogue[i].TriggerCode);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Object", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Dialogue[i].TriggerObject = QUnityEditor.SetField(m_target.Dialogue[i].TriggerObject);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetVerticalEnd();
            }
            else
            {
                QUnityEditor.SetLabel("Code: " + m_target.Dialogue[i].TriggerCode, null, QUnityEditor.GetGUIWidth(LABEL_WIDTH * 2 + 4));
                QUnityEditor.SetLabel("" + (m_target.Dialogue[i].TriggerObject != null ? m_target.Dialogue[i].TriggerObject.name : ""));
            }
            QUnityEditor.SetHorizontalEnd();
            //ITEM - TRIGGER:
            //
            QUnityEditor.SetHorizontalEnd();
            //ITEM - MAIN:
            //
            QUnityEditor.SetVerticalEnd();
            //ITEM:
            //
            //NEXT:
            QUnityEditor.SetSpace(10);
        }
        QUnityEditor.SetScrollViewEnd();
    }

    private void SetGUIGroupChoice()
    {
        QUnityEditor.SetLabel("CHOICE", QUnityEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        //COUNT:
        QUnityEditor.SetHorizontalBegin();
        QUnityEditor.SetLabel("Count", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
        //
        m_choiceCount = QUnityEditor.SetField(m_choiceCount);
        //
        if (QUnityEditor.SetButton("+"))
            m_choiceCount++;
        //
        if (QUnityEditor.SetButton("-"))
            if (m_choiceCount > 0)
                m_choiceCount--;
        //
        QUnityEditor.SetHorizontalEnd();
        //COUNT:
        //
        while (m_choiceCount > m_target.Choice.Count)
            m_target.Choice.Add(new DialogueDataChoice());
        while (m_choiceCount < m_target.Choice.Count)
            m_target.Choice.RemoveAt(m_target.Choice.Count - 1);
        //
        QUnityEditor.SetSpace(10);
        //
        m_scrollChoice = QUnityEditor.SetScrollViewBegin(m_scrollChoice, QUnityEditor.GetGUIHeight(POPUP_HEIGHT));
        for (int i = 0; i < m_target.Choice.Count; i++)
        {
            //ITEM:
            //
            //ITEM - NUM:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel(i.ToString(), QUnityEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QUnityEditor.GetGUIWidth(25));
            //ITEM - NUM:
            //
            //ITEM - MAIN:
            QUnityEditor.SetVerticalBegin();
            //ITEM - Dialogue:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel("Text", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
            m_target.Choice[i].Text = QUnityEditor.SetField(m_target.Choice[i].Text);
            QUnityEditor.SetHorizontalEnd();
            //ITEM - Dialogue:
            //
            //ITEM - NEXT:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel("Next", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
            m_target.Choice[i].Next = QUnityEditor.SetField<DialogueConfigSingle>(m_target.Choice[i].Next);
            QUnityEditor.SetHorizontalEnd();
            //ITEM - NEXT:
            //
            //ITEM - AUTHOR
            QUnityEditor.SetHorizontalBegin();
            if (QUnityEditor.SetButton("Extra", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH)))
                m_choiceAuthorShow[i] = !m_choiceAuthorShow[i];
            //
            if (m_choiceAuthorShow[i])
            {
                QUnityEditor.SetVerticalBegin();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Author", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Choice[i].AuthorIndex = QUnityEditor.SetPopup(m_target.Choice[i].AuthorIndex, m_authorName);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Dialogue", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Choice[i].Dialogue = QUnityEditor.SetField(m_target.Choice[i].Dialogue);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetVerticalEnd();
            }
            else
            {
                QUnityEditor.SetLabel(string.Format("{0} : {1}", m_authorName[m_target.Choice[i].AuthorIndex], m_target.Choice[i].Dialogue), null);
            }
            //
            QUnityEditor.SetHorizontalEnd();
            //ITEM - AUTHOR
            //
            //ITEM - TRIGGER:
            QUnityEditor.SetHorizontalBegin();
            if (QUnityEditor.SetButton("Trigger", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH)))
                m_choiceTriggerShow[i] = !m_choiceTriggerShow[i];
            //
            if (m_choiceTriggerShow[i])
            {
                QUnityEditor.SetVerticalBegin();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Code", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Choice[i].TriggerCode = QUnityEditor.SetField(m_target.Choice[i].TriggerCode);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetHorizontalBegin();
                QUnityEditor.SetLabel("Object", null, QUnityEditor.GetGUIWidth(LABEL_WIDTH));
                m_target.Choice[i].TriggerObject = QUnityEditor.SetField(m_target.Choice[i].TriggerObject);
                QUnityEditor.SetHorizontalEnd();
                //
                QUnityEditor.SetVerticalEnd();
            }
            else
            {
                QUnityEditor.SetLabel("Code: " + m_target.Choice[i].TriggerCode, null, QUnityEditor.GetGUIWidth(LABEL_WIDTH * 2 + 4));
                QUnityEditor.SetLabel("" + (m_target.Choice[i].TriggerObject != null ? m_target.Choice[i].TriggerObject.name : ""));
            }
            QUnityEditor.SetHorizontalEnd();
            //ITEM - TRIGGER:
            //
            QUnityEditor.SetHorizontalEnd();
            //ITEM - MAIN:
            //
            QUnityEditor.SetVerticalEnd();
            //ITEM:
            //
            //NEXT:
            QUnityEditor.SetSpace(10);
        }
        QUnityEditor.SetScrollViewEnd();
    }
}

#endif