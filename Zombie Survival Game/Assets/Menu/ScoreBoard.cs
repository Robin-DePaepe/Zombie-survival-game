using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private Text m_KillsText;
    [SerializeField] private Text m_TimeText;

    private int m_Kills;
    private float m_Time;
    private bool m_GameOver;
    void Start()
    {
        m_GameOver = false;
        m_Kills = 0;
        m_Time = 0f;
        m_KillsText.text = m_Kills.ToString();
    }

    void Update()
    {
        if (m_GameOver == false)
        {
            m_Time += Time.deltaTime;

            int minutes = (int)(m_Time / 60f);
            int seconds = (int)(m_Time - (minutes * 60));

            m_TimeText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
    }

    public void AddKill()
    {
        ++m_Kills;
        m_KillsText.text = m_Kills.ToString();
    }
}
