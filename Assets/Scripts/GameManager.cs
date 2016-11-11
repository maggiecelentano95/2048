using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameState{
	inGame,
	gameOver,
	movingTiles,
	wonGame
}
public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public Canvas WonGameCanvas;
	public Canvas inGameCanvas;
	public Canvas gameOverCanvas;

	public GameState currentGameState;

	void Awake(){
		instance = this;
	}

	void Start(){
		Debug.Log ("Game State = inGame");
		currentGameState = GameState.inGame;
		GridBehavior.instance.StartGame ();
	}

	public void StartGame(){
		Debug.Log ("Game State = InGame");
		SetGameState (GameState.inGame);
	}

	public void ContinueGame(){
		Debug.Log ("ContinueGame");
		SetGameState (GameState.inGame);
	}

	public void GameOver(){
		Debug.Log ("Game State = GameOver");
		SetGameState (GameState.gameOver);
	}

	public void RestartGame(){
		SceneManager.LoadScene ("Main");
	}

	public void MovingTiles(){
		Debug.Log ("Game State = MovingTiles");
		SetGameState (GameState.movingTiles);
	}

	public void WonGame(){
		SetGameState (GameState.wonGame);
	}

	void SetGameState(GameState newGameState){
		if (newGameState == GameState.inGame) {
			//setup Unity scene for menu state
			WonGameCanvas.enabled = false;
			inGameCanvas.enabled = true;
			gameOverCanvas.enabled = false;
		} else if (newGameState == GameState.wonGame) {
			WonGameCanvas.enabled = true;
			inGameCanvas.enabled = false;
			gameOverCanvas.enabled = false;
		} 
		else if (newGameState == GameState.gameOver) {
			WonGameCanvas.enabled = false;
			inGameCanvas.enabled = false;
			gameOverCanvas.enabled = true;
		} 
		currentGameState = newGameState; 
	}
}
