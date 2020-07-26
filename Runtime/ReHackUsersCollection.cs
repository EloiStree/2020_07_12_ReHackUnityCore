using RestreamChatHacking;
using System.Collections.Generic;

public class ReHackUsersCollection<T> where T : class
{
    Dictionary<string, T> m_register = new Dictionary<string, T>();
    T m_defaultValue;

    public ReHackUsersCollection()
    {
        m_defaultValue = null;
    }
    public ReHackUsersCollection(T defaultValue)
    {
        m_defaultValue = defaultValue;
    }
  
    public T GetFromId(string userId)
    {
        if (!m_register.ContainsKey(userId))
            m_register.Add(userId, m_defaultValue);
        return m_register[userId];
    }
    public void SetDefaultIfNotFound(T defaultValue)
    {
        m_defaultValue = defaultValue;
    }
    public T GetFromMessage(ref RestreamChatMessage userTarget)
    {

        return GetFromId(userTarget.UserID.GetID());
    }

    public bool IsRegistered(string userId)
    {
        return m_register.ContainsKey(userId);
    }
    public bool IsRegistered(ref RestreamChatMessage userTarget)
    {

        return IsRegistered(userTarget.UserID.GetID());
    }
    public void Store(string userId, T toStore)
    {
        if (!m_register.ContainsKey(userId))
            m_register.Add(userId, m_defaultValue);
        m_register[userId] = toStore;
    }
    public void Store(ref RestreamChatMessage userTarget, T toStore)
    {
        Store(userTarget.UserID.GetID(), toStore);

    }

}
