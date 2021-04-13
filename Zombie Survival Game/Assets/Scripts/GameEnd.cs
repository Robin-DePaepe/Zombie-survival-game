using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameEnd : MonoBehaviour
{
    [SerializeField] private int m_MapWinAward = 5;
    private GameObject m_PauseMenu;
    private GameObject m_ScoreBoard;

    private void Awake()
    {
        m_PauseMenu = GameObject.Find("PauseMenu");
        m_ScoreBoard = GameObject.Find("ScoreBoard");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Tokens.ChangeTokens(m_MapWinAward);
            EndGame();
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0f;

        //show pause screen without continue
        if (m_PauseMenu != null)
        {
            m_PauseMenu.gameObject.SetActive(true);
            m_PauseMenu.transform.Find("PauseScreen/Continue").gameObject.SetActive(false);
        }
        //show scoreboard
        if (m_ScoreBoard != null)
        {
            m_ScoreBoard.GetComponentInChildren<Canvas>().enabled = true;
            m_ScoreBoard.GetComponent<ScoreBoard>().GameOver();
        }
        //set the cursur free
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
