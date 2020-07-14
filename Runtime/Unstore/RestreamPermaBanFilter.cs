using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class RestreamPermaBanFilter : MonoBehaviour
{
    public RestreamMessageUnityEvent m_fromAuthorizedUser;
    public RestreamMessageUnityEvent m_fromBanUser;
    [Header("Debug")]
    public Dictionary<string,string> m_listOfBanned = new Dictionary<string,string>();
    [Header("Add at compilation")]
    public List<string> m_userIdBans;
    public List<string> m_messageLeadingToBans;
    public TextAsset m_inProjectBanUsers;
    public TextAsset m_inProjectBanMessages;
    public string m_urlOfBanListFileUsers;
    public string m_urlOfBanListFileMessages;
    [Header("Add at compilation")]
    public List<string> m_currentlyBan;



    IEnumerator Start()
    {
        for (int i = 0; i < m_userIdBans.Count; i++)
        {
            BanThisUserId(m_userIdBans[i]);
        }
        for (int i = 0; i < m_messageLeadingToBans.Count; i++)
        {
            BanThisUserFromOneLine(m_messageLeadingToBans[i]);
        }

        if (m_inProjectBanUsers != null)
        {
            LoadBanUsersFromTextReturnLine(m_inProjectBanUsers.text);
        }
        if (m_inProjectBanMessages != null)
        {
            LoadBanMessagesFromTextReturnLine(m_inProjectBanMessages.text);
        }

        if (!string.IsNullOrEmpty(m_urlOfBanListFileUsers))
        {
            WWW www = new WWW(m_urlOfBanListFileUsers);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                LoadBanUsersFromTextReturnLine(www.text);
            }
        }
        if (!string.IsNullOrEmpty(m_urlOfBanListFileMessages))
        {
            WWW www = new WWW(m_urlOfBanListFileMessages);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                LoadBanMessagesFromTextReturnLine(www.text);
            }
        }
        InvokeRepeating("RefreshCurrentlyBan", 0, 1);
    }

    public void RefreshCurrentlyBan() {
        m_currentlyBan = m_listOfBanned.Keys.ToList();
    }

    private void LoadBanUsersFromTextReturnLine(string text)
    {
        string[] tokens = text.Split('\n');
        for (int i = 0; i < tokens.Length; i++)
        {
            BanThisUserId(tokens[i]);
        }
    }
    private void LoadBanMessagesFromTextReturnLine(string text)
    {
        string[] tokens = text.Split('\n');
        for (int i = 0; i < tokens.Length; i++)
        {
            BanThisUserFromOneLine(tokens[i]);
        }
    }

    public void BanThisUserFromOneLine(string oneliner) {
        bool isValide;
        RestreamChatMessage msg;
        RestreamChatMessage.CreateFromOneLiner(oneliner, out isValide, out msg);
        if (isValide)
            BanThisUser(msg);
    }
    public void BanThisUser(RestreamChatMessage message)
    {
        BanThisUserId(message.UserID.GetID());

    }
    public void BanThisUserId(string id) {
        if (!m_listOfBanned.ContainsKey(id)) {
            m_listOfBanned.Add(id, "");
        }
    }

    public void PushIfNotBan(RestreamChatMessage message) {
        bool isBanUser = IsUserBan(message);
        if (isBanUser)
        {
            m_fromBanUser.Invoke(message);
        }
        else
        {
            m_fromAuthorizedUser.Invoke(message);
        }

    }


    private bool IsUserBan(RestreamChatMessage message)
    {

        string id = message.UserID.GetID().Trim();
        return m_listOfBanned.ContainsKey(id);
    }
 
}
