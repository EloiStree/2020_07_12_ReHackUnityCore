
using System;
using UnityEngine;

namespace RestreamChatHacking { 
[System.Serializable]
public class RestreamChatUser
{

    public RestreamChatUser(string userName)
    {
        UserName = userName;
        Platform = ChatPlatform.Unknow;
    }
    public RestreamChatUser(string userName, ChatPlatform platform) : this(userName)
    {
        Platform = platform;
        UserName = userName;
    }
        public static  void GetUrlProfileOf(string userName, ChatPlatform platform, out bool found, out string url) {
            url = "";
            found = false;
            switch (platform)
            {
               

                case ChatPlatform.Twitch:
                    url ="https://www.twitch.tv/"+userName;


                    found = true; break;
                case ChatPlatform.Youtube:
                    url = "https://www.youtube.com/results?search_query=" + userName.Replace("+", " ");

                    found = true; break;
                case ChatPlatform.Facebook:
                    url = "https://www.facebook.com/" + userName;

                    found = true; break;
                case ChatPlatform.LinkedIn:
                    url = "https://www.linkedin.com/search/results/all/?keywords=" + userName.Replace("+", " ") + "&origin=GLOBAL_SEARCH_HEADER";

                    found = true; break;
                case ChatPlatform.Periscope:
                    url = "https://www.periscope.tv/" + userName;
                    found = true;
                    break;
                case ChatPlatform.DLive:
                    url = "https://dlive.tv/" + userName;
                    found = true;

                    break; 
                case ChatPlatform.RestreamInitMessage:
                case ChatPlatform.Restream:
                case ChatPlatform.Mockup:
                case ChatPlatform.Unknow:
                case ChatPlatform.Discord:
                default:
                    found = false;
                    break;
            }

        }

    public static void CreateFromOneLiner(string oneliner, out bool succed, out RestreamChatMessage message)
    {

        message = new RestreamChatMessage();
        message.SetWithOneLiner(oneliner);
        succed = message.IsCorrectlyDefined();
    }


    [SerializeField] string m_userName;

    public string UserName
    {
        get { return m_userName; }
        set { m_userName = value; }
    }


    public RestreamChatUser Duplicate()
    {
        RestreamChatUser duplicate = new RestreamChatUser("");
        duplicate.SetWithOneLiner(GetAsOneLiner());

        return duplicate;
    }

    public void Reset()
    {

        UserName = "";
        SetPlatform(ChatPlatform.Unknow);
    }

    public bool IsCorrectlyDefined()
    {
        return !string.IsNullOrEmpty(UserName);
    }



    public string GetAsOneLiner()
    {

        return string.Format("RCU¦{0}¦{1}¶",
                  UserName, Platform);
    }
    public void SetWithOneLiner(string text)
    {
        // Debug.Log(">" + text+ "<"   );
        text = text.Trim();
        if (!(text.IndexOf("RCU¦") == 0 &&
            text[text.Length - 1] == '¶'))
            return;
        text = text.Substring(4);
        text = text.Substring(0, text.Length - 1);
        string[] token = text.Split('¦');
        if (token.Length != 5)
            return;
        UserName = token[0];
        Platform = GetEnumOf(token[1]);
    }

    private ChatPlatform GetEnumOf(string text)
    {
        try
        {
            ChatPlatform platform;
            if (Enum.TryParse(text, out platform))
                return platform;
            else return ChatPlatform.Unknow;
        }
        catch (Exception) { return ChatPlatform.Unknow; }
    }

    
        private ChatPlatform m_platformId;

    public ChatPlatform Platform
    {
        get { return m_platformId; }
        set { m_platformId = value; }
    }



    public UserIdentifier UserID { get { return new UserIdentifier(m_platformId, m_userName); } }

    public void SetPlatform(ChatPlatform platform)
    {
        m_platformId = platform;
    }


    public override string ToString()
    {
        return string.Format("User({0},{1})", UserName, Platform);
    }

    public static bool operator ==(RestreamChatUser obj1, RestreamChatUser obj2)
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        if (ReferenceEquals(obj1, null))
        {
            return false;
        }
        if (ReferenceEquals(obj2, null))
        {
            return false;
        }

        return obj1.UserID == obj2.UserID;
    }

    public static bool operator !=(RestreamChatUser obj1, RestreamChatUser obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(RestreamChatUser other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UserID.Equals(other.UserID);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((RestreamChatUser)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = UserID.GetHashCode();
            return hashCode;
        }
    }

}

}