using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_LagTimer : MonoBehaviour
{

    public Text m_streamTimer;
    public Text m_user;
    public Text m_userTime;
    public Text m_userLag;

    public DateTime m_date;
    void Update()
    {

        m_date = DateTime.Now;
        m_streamTimer.text = m_date.ToString("HH:mm:ss");
        
    }
    public void DisplayLagOf(RestreamChatMessage message) {

        string msg=message.Message;
        Regex r = new Regex("(?:[01]\\d|2[0123]):(?:[012345]\\d):(?:[012345]\\d)");
            Match m =r.Match(msg);
        m_user.text = message.UserName;
        if (!string.IsNullOrEmpty(m.Value))
        {
            string[] tokens = m.Value.Split(':');
            DateTime d = message.GetWhenAsDateTime();
            DateTime md = new DateTime(d.Year, d.Month, d.Day, int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            m_userLag .text = "" + Mathf.Abs((float)(m_date -md).TotalMilliseconds / 1000f);
            m_userTime.text = "" + md.ToString("HH:mm:ss");

        }
        else {
            m_userLag.text = "" + Mathf.Abs((float)(m_date- message.GetWhenAsDateTime()).TotalMilliseconds/1000f);
         m_userTime.text = "" + m_date.ToString("HH:mm:ss");
        }
    }
}
