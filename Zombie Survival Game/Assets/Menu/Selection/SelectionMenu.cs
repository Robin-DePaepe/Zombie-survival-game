using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectionMenu : MonoBehaviour
{
    private int m_levelIndex;

    public void LoadScene()
    {
        SceneManager.LoadScene(m_levelIndex);
    }

    public void SelectLevel1()
    {
        m_levelIndex = 1;
    }
    public void SelectLevel2()
    {
        m_levelIndex = 2;
    }

    public void SelectLevel3()
    {
        m_levelIndex = 3;
    }
}
