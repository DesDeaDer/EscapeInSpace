using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour 
{
	public const string SCENE_NAME_GAME = "Game";

	public void PlayGame()
	{
		SceneManager.LoadScene(SCENE_NAME_GAME, LoadSceneMode.Single);
	}
}
