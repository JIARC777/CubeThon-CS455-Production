
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	bool gameOver = false;
	public float restartDelay = 1f;
	public GameObject completeLevelUI;
    public void CompleteLevel()
	{
		Debug.Log("LEVEL WON");
		completeLevelUI.SetActive(true);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
	}

    public void EndGame()
	{
		if (gameOver == false)
		{
			gameOver = true;
			Debug.Log("Game Over");
			Invoke("Restart", restartDelay);
		}
		
	}

	void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
