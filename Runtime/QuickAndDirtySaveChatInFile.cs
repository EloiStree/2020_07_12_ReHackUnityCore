using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class QuickAndDirtySaveChatInFile : MonoBehaviour
{

    public string m_fileName="ChatMessage.txt";
    public string m_textToAppend;
    public float m_appendFrequency=5;

    private void Start()
    {
        InvokeRepeating("AppendText", m_appendFrequency, m_appendFrequency);
    }
    public void MessageToSave(RestreamChatHacking.RestreamChatMessage message) {

        m_textToAppend += message.GetAsOneLiner()+"\n";
      
   
    }

    public  void AppendText()
    {
        //await Task.Run(() =>
        //{
        try
        {
            if (m_textToAppend.Length > 0)
            {
                string folderPath, filepath;
                GetFilePath(out folderPath, out filepath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (!File.Exists(filepath))
                    File.Create(filepath);
                File.AppendAllText(filepath, m_textToAppend);
                m_textToAppend = "";
            }
        }
        catch (Exception e) { Debug.LogWarning("Exception avoid saving chat"); }
        //});
    }

    private void GetFilePath(out string folderPath, out string filepath)
    {
        folderPath = GetFolderPath();
        filepath = folderPath + "/" + m_fileName;
    }

    private static string GetFolderPath()
    {
        return Application.persistentDataPath + "/PixelWars";
    }

    [ContextMenu("Open Location")]
    public void OpenLocation()
    {

         Application.OpenURL(GetFolderPath());
    }


    [ContextMenu("Open File")]
    public void OpenFile()
    {
        GetFilePath(out string d, out string filePath)
          ;Application.OpenURL(filePath);
    }
}
