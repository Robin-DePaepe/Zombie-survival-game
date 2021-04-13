using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        //reset the timescale aswell or the next game will be frozen
        Time.timeScale = 1f;
        KamikazeScript.ProgramEnd = true;
        SceneManager.LoadScene(0);
    }
    public void ContinueGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
