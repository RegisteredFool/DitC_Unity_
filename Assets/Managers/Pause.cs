using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject container;
    public bool pause = false;

    private void Start()
    {
        container.SetActive(false);
    }
    public void OnPause(InputValue value)
    {
        //if (value.isPressed)
        //{
        pause = !pause;
        if (pause == true)
        {
            Time.timeScale = 0;
            container.SetActive(true);
        }
        else
        { 
            Time.timeScale = 1;
            container.SetActive(false);
        }
    }
    public void AlternatePause()
    {
        //if (value.isPressed)
        //{
        pause = !pause;
        if (pause == true)
        {
            Time.timeScale = 0;
        }
        else
        { 
            Time.timeScale = 1;
        }
    }
    public void ResumeButton()
    {
        Time.timeScale = 1;
        container.SetActive(false);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
