using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menu, credits, story;

    
    
    public void StartGame()
    {
        SceneManager.LoadScene("Aybars Level Generator");
    }

    public void BackToMenu()
    {
        credits.SetActive(false);
        menu.SetActive(true);
    }
    
    public void BackFromStory()
    {
        story.SetActive(false);
        menu.SetActive(true);
    }

    public void ToStory()
    {
        story.SetActive(true);
        menu.SetActive(false);
    }
    public void ToCredits()
    {
        credits.SetActive(true);
        menu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetSave()
    {
        LevelGeneration level1 = new LevelGeneration();
        level1.level = 1;
        SaveSystem.SaveLevel(level1);
    }
    
    
}
