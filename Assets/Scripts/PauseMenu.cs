using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_PausePanel;

    public void Pause()
    {
        m_PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        m_PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
