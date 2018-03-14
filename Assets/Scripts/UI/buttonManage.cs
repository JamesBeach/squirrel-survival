using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class buttonManage : MonoBehaviour {

	private LevelController lvlctrl;

    private void Start()
    {
        if ((lvlctrl = FindObjectOfType<LevelController>()) == null)
        {
            Debug.Log("buttonManager did not find a LevelController in the scene!");
        }
    }

    public void pause()
	{
		lvlctrl.Pause ();
	}

    public void unPause()
    {
        lvlctrl.UnPause();
    }

	public void mainMenu()
	{
        lvlctrl.UnPause(); // game must be unpaused to restore the timescale
		SceneManager.LoadScene ("main menu");
	}

    public void newGamebutton(string NewGameLevel)
    {
        SceneManager.LoadScene(NewGameLevel);
    }
    public void exit()
    {
        Application.Quit();
    }

    public void nextLevel()
	{
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "sf1") 
		{
			SceneManager.LoadScene("Suburb level");
		}
		if (scene.name == "sf2") 
		{
			SceneManager.LoadScene("Highway level");
		}
		if (scene.name == "sf3") 
		{
			SceneManager.LoadScene("Forest level");
		}
		if (scene.name == "sf4") 
		{
			SceneManager.LoadScene("Tree Farm level");
		}
	}
}
