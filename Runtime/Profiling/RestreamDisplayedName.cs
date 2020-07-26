using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestreamDisplayedName : MonoBehaviour
{
    public static RestreamDisplayedName m_instance;
    public string m_startTag= "#Name ";
    public Dictionary<string, string> m_displayNameDB= new Dictionary<string, string>();


    private void Awake()
    {
        m_instance = this;
    }

    public void ResetDatabase() {
        m_displayNameDB.Clear();
    }

    public static void AddDisplayedName(string userId, string displayedName) {
        if (m_instance == null)
            return;
        if (!m_instance.m_displayNameDB.ContainsKey(userId))
        {
            m_instance.m_displayNameDB.Add(userId, displayedName);
        }
        else m_instance.m_displayNameDB[userId] = displayedName;
    }

    public void AddDisplayedNameFromMessage(RestreamChatMessage msg) {
        if ( ! msg.Message.StartsWith(m_startTag) ) 
            return;
        AddDisplayedName(
            msg.UserID.GetID(),
            msg.Message.Substring(m_startTag.Length));

    }

    public static string GetDisplayNameOf(ref RestreamChatMessage msg) {
        if (m_instance == null)
            return msg.Message;
        if (m_instance.m_displayNameDB.ContainsKey(msg.UserID.GetID()))
            return m_instance.m_displayNameDB[msg.UserID.GetID()];
        return msg.Message;
    }
}
