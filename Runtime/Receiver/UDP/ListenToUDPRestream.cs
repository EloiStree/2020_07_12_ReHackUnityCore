using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using RestreamChatHacking;

public class ListenToUDPRestream : MonoBehaviour
{
    public int portToListen=2512;
    public System.Threading.ThreadPriority m_threadPriority = System.Threading.ThreadPriority.BelowNormal;
    public UDPReceive m_udpReceiver;
    public string m_lastMessage;
    public RestreamChatMessage m_lastRCM;
    public RestreamMessageUnityEvent m_receivedMessage;
    [TextArea(0,5)]
    public string m_history;
    void Awake()
    {
        m_udpReceiver = new UDPReceive(portToListen, m_threadPriority);

    }
    private void Update()
    {
        m_lastMessage = m_udpReceiver.m_lastReceivedUDPPacket;
        string msg = null;
        do
        {
            if (m_udpReceiver.m_allReceivedUDPPackets.Count > 0)
            {
                msg = m_udpReceiver.m_allReceivedUDPPackets.Dequeue();
                m_history = msg + m_history;
                if (msg != null) { 
                  // Debug.Log(msg);
                    m_lastRCM.SetWithOneLiner(msg.Trim());
                    if (m_lastRCM.IsCorrectlyDefined())
                        m_receivedMessage.Invoke(m_lastRCM.Duplicate()) ;
                    m_lastRCM.Reset();
                }
            }
            else msg = null;
        }
        while (msg != null);
    }
    void OnDisable()
    {
        m_udpReceiver.Kill();
        
    }
}


public class UDPReceive 
{

    Thread m_receiveThread;

    UdpClient m_client;

    public int m_port;
    public System.Threading.ThreadPriority m_threadPriority = System.Threading.ThreadPriority.BelowNormal;
    public string m_lastReceivedUDPPacket = "";
    public Queue<string> m_allReceivedUDPPackets= new Queue<string>();

    public UDPReceive(int port, System.Threading.ThreadPriority threadPriority) {
        m_port = port;
        m_threadPriority = threadPriority;
        init();
    }

    

    // init
    private void init()
    {
        if (m_receiveThread == null) { 
            m_receiveThread = new Thread(
                new ThreadStart(ReceiveData));
            m_receiveThread.Priority = m_threadPriority;
            m_receiveThread.IsBackground = true;
            m_receiveThread.Start();
        }

    }

    // receive thread
    private void ReceiveData()
    {

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, m_port);
        m_client = new UdpClient(anyIP);
       // m_client.Connect(anyIP);
        while (true)
        {

            try
            {
                byte[] data = m_client.Receive(ref anyIP);
                string text = Encoding.Unicode.GetString(data);
                m_lastReceivedUDPPacket = text;
                m_allReceivedUDPPackets.Enqueue(text);

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    public string getLatestUDPPacket()
    {
       
        return m_lastReceivedUDPPacket;
    }

    public void Kill()
    {
        if (m_receiveThread != null)
            m_receiveThread.Abort();

        m_client.Close();
    }
}