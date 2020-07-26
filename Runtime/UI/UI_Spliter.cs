using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Spliter : MonoBehaviour
{
    public InputField m_userName;
    public InputField m_userId;
    public InputField m_userMessage;
    public RestreamChatMessage m_lastMessageSplited = new RestreamChatMessage();


    public void SetWith(RestreamChatMessage message) {
        m_lastMessageSplited.SetWithOneLiner(message.GetAsOneLiner());
        Refresh();
    }
    public void SetWithOneLiner(string oneLiner) {
       m_lastMessageSplited.SetWithOneLiner(oneLiner);
        Refresh();

    }

    public void Refresh()
    {
        m_userName.text = m_lastMessageSplited.UserName;
        m_userId.text = m_lastMessageSplited.UserID.GetID();
        m_userMessage.text = m_lastMessageSplited.Message;
    }
    
}
