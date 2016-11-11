using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridBehavior : MonoBehaviour {

	public static GridBehavior instance;

	private float[] xPositions = { -5f, -2.5f, 0f, 2.5f };
	private float[] yPositions = { 3.75f, 1.25f, -1.25f, -3.75f };
	private Tile[,] tiles = new Tile[4, 4]; 
	private bool[,] alreadyCombined = new bool[4, 4]; //used to prevent multiple combines

	[SerializeField] private Sprite[] images;
	[SerializeField] private Tile originalTile;

	private int posx;
	private int posy;
	private float posz = -2.0f; 
	private int id;
	private int _score = 0;

	public Text scoreText;
	public Text highScoreText;

	void Awake(){
		instance = this;
	}

	// Set up the first tiles
	public void StartGame () {
		highScoreText.text = "" + PlayerPrefs.GetInt ("highscore");
		Reset ();
		SpawnTile ();
		SpawnTile ();
	}

	private IEnumerator Waiting(){
		yield return new WaitForSeconds(3f);
		GameManager.instance.GameOver ();
		Debug.Log(GameManager.instance.currentGameState);
	}
	
	// Update is called once per frame
	void Update () {
		if (AreWeStuck ()) {
			StartCoroutine (Waiting ()); 
		}
		if (GameManager.instance.currentGameState == GameState.inGame) {
			if (Input.GetKeyDown ("down")) {
				Debug.Log ("down pressed");
				GameManager.instance.MovingTiles ();
				MoveDown ();
			} else if (Input.GetKeyDown ("up")) {
				Debug.Log ("up pressed");
				GameManager.instance.MovingTiles ();
				MoveUp ();
			} else if (Input.GetKeyDown ("left")) {
				Debug.Log ("left pressed");
				GameManager.instance.MovingTiles ();
				MoveLeft ();
			} else if (Input.GetKeyDown ("right")) {
				Debug.Log ("right pressed");
				GameManager.instance.MovingTiles ();
				MoveRight ();
			}

		}
	}

	public void MoveDown(){
		float xpos;
		int count = 1; //keep track of how low we can go
		int rowsToClimb;
		bool canSpawn = false;
		int newID = 0;
		bool won = false;

		for (int i = 2; i >= 0; i--) { //rows (y-axis)
			for (int j = 0; j < 4; j++) { //columns (x-axis)
				count=1; //reset
				rowsToClimb = 0; //reset

				if (tiles [j, i] != null){
					while (i + count <= 3) {
						if (tiles [j, i + count] == null) {
							rowsToClimb++;
						}
						count++;
					}

					if (rowsToClimb != 0) { //do we have to move?
						xpos = tiles [j, i].transform.position.x;
						tiles [j, i + rowsToClimb] = tiles [j, i];
						tiles [j, i] = null;
						tiles [j, i + rowsToClimb].transform.position = new Vector3 (xpos, yPositions [i + rowsToClimb], posz);
						canSpawn = true;
					}

					if (i + rowsToClimb + 1 <= 3) {
						if (tiles [j, i + rowsToClimb + 1] != null && alreadyCombined[j, i + rowsToClimb + 1] == false) {
							if (tiles [j, i + rowsToClimb].GetID () == tiles [j, i + rowsToClimb + 1].GetID ()) {
								Debug.Log ("MATCH!!");
								Destroy (tiles [j, i + rowsToClimb].gameObject);
								tiles [j, i + rowsToClimb] = null;
								newID = tiles [j, i + rowsToClimb + 1].GetID () + 1;
								if (newID == 10) {
									won = true;
								}
								_score += CombineValue (newID + 1);
								CheckIfNewHigh (_score);
								scoreText.text = "" + _score;
								tiles [j, i + rowsToClimb + 1].SetTile (newID, images [newID]);
								alreadyCombined [j, i + rowsToClimb + 1] = true;
								canSpawn = true;
							}
						}
					}
				}
			}//end inner loop
		}//end outer loop

		Reset ();

		//spawn new tile
		if (canSpawn) {
			SpawnTile ();
		}

		if (won) {
			GameManager.instance.WonGame ();
		} else {
			//Done moving tiles, so we can wait for input again...
			GameManager.instance.ContinueGame ();
		}
	}

	public void MoveUp(){
		float xpos; 
		int count = 1; //keep track of how high we can go
		int rowsToClimb;
		bool canSpawn = false;
		int newID = 0;
		bool won = false;

		for (int i = 1; i < 4; i++) { //rows (y-axis)
			for (int j = 0; j < 4; j++) { //columns (x-axis)
				count=1; //reset
				rowsToClimb = 0; //reset

				if (tiles [j, i] != null){
					while (i - count >= 0) {
						if (tiles [j, i - count] == null) {
							rowsToClimb++;
						}
						count++;
					}
					if (rowsToClimb != 0) { //do we have to move?
						xpos = tiles [j, i].transform.position.x;
						tiles [j, i - rowsToClimb] = tiles [j, i];
						tiles [j, i] = null;
						tiles [j, i - rowsToClimb].transform.position = new Vector3 (xpos, yPositions [i - rowsToClimb], posz);
						canSpawn = true;
					}
						
					if (i - rowsToClimb - 1 >= 0) {
						if (tiles [j, i - rowsToClimb - 1] != null && alreadyCombined[j, i - rowsToClimb - 1] == false) {
							if (tiles [j, i - rowsToClimb].GetID () == tiles [j, i - rowsToClimb - 1].GetID ()) {
								Debug.Log ("MATCH!!");
								Destroy (tiles [j, i - rowsToClimb].gameObject);
								tiles [j, i - rowsToClimb] = null;
								newID = tiles [j, i - rowsToClimb - 1].GetID () + 1;
								if (newID == 10) {
									won = true;
								}
								_score += CombineValue (newID + 1);
								CheckIfNewHigh (_score);
								scoreText.text = ""+ _score;
								tiles [j, i - rowsToClimb - 1].SetTile (newID, images [newID]);
								alreadyCombined [j, i - rowsToClimb - 1] = true;
								canSpawn = true;
							}
						}
					}
				}
			}//end inner loop
		}//end outer loop

		Reset ();

		//spawn new tile
		if (canSpawn) {
			SpawnTile ();
		}

		if (won) {
			GameManager.instance.WonGame ();
		} else {
			//Done moving tiles, so we can wait for input again...
			GameManager.instance.ContinueGame ();
		}
	}

	public void MoveLeft(){
		float ypos;
		int count = 1; //keep track of how high we can go
		int columnsToClimb;
		bool canSpawn = false;
		int newID = 0;
		bool won = false;

		for (int i = 1; i <= 3; i++) { //columns (x-axis)
			for (int j = 0; j < 4; j++) { //rows (y-axis)
				count=1; //reset
				columnsToClimb = 0; //reset

				if (tiles [i, j] != null){
					while (i - count >= 0) {
						if (tiles [i - count, j] == null) {
							columnsToClimb++;
						}
						count++;
					}
					if (columnsToClimb != 0) { //do we have to move?
						ypos = tiles [i, j].transform.position.y;
						tiles [i - columnsToClimb,j] = tiles [i, j];
						tiles [i,j] = null;
						tiles [i - columnsToClimb,j].transform.position = new Vector3 (xPositions [i - columnsToClimb], ypos, posz);
						canSpawn = true;
					}

					if (i - columnsToClimb - 1 >= 0) {
						if (tiles [i - columnsToClimb - 1,j] != null && alreadyCombined[i - columnsToClimb - 1,j] == false) {
							if (tiles [i - columnsToClimb,j].GetID () == tiles [i - columnsToClimb - 1,j].GetID ()) {
								Debug.Log ("MATCH!!");
								Destroy (tiles [i - columnsToClimb,j].gameObject);
								tiles [i - columnsToClimb,j] = null;
								newID = tiles [i - columnsToClimb - 1,j].GetID () + 1;
								if (newID == 10) {
									won = true;
								}
								_score += CombineValue (newID + 1);
								CheckIfNewHigh (_score);
								scoreText.text = ""+_score;
								tiles [i - columnsToClimb - 1,j].SetTile (newID, images [newID]);
								alreadyCombined [i - columnsToClimb - 1, j] = true;
								canSpawn = true;
							}
						}
					}
				}
			}//end inner loop
		}//end outer loop

		Reset ();

		//spawn new tile
		if (canSpawn) {
			SpawnTile ();
		}
		if (won) {
			GameManager.instance.WonGame ();
		} else {
			//Done moving tiles, so we can wait for input again...
			GameManager.instance.ContinueGame ();
		}
	}

	public void MoveRight(){
		float ypos;
		int count = 1; //keep track of how high we can go
		int columnsToClimb;
		bool canSpawn = false;
		int newID = 0;
		bool won = false;

		for (int i = 2; i >= 0; i--) { //columns (x-axis)
			for (int j = 0; j < 4; j++) { //rows (y-axis)
				count=1; //reset
				columnsToClimb = 0; //reset

				if (tiles [i, j] != null){// && tiles [j, i - 1] == null) {
					while (i + count <= 3) {
						if (tiles [i + count, j] == null) {
							columnsToClimb++;
						}
						count++;
					}
					if (columnsToClimb != 0) { //do we have to move?
						ypos = tiles [i, j].transform.position.y;
						tiles [i + columnsToClimb,j] = tiles [i, j];
						tiles [i,j] = null;
						tiles [i + columnsToClimb,j].transform.position = new Vector3 (xPositions [i + columnsToClimb], ypos, posz);
						canSpawn = true;
					}

					if (i + columnsToClimb + 1 <= 3) {
						if (tiles [i + columnsToClimb + 1,j] != null && alreadyCombined[i + columnsToClimb + 1,j] == false) {
							if (tiles [i + columnsToClimb,j].GetID () == tiles [i + columnsToClimb + 1,j].GetID ()) {
								Debug.Log ("MATCH!!");
								Destroy (tiles [i + columnsToClimb,j].gameObject);
								tiles [i + columnsToClimb,j] = null;
								newID = tiles [i + columnsToClimb + 1,j].GetID () + 1;
								if (newID == 10) {
									won = true;
								}
								_score += CombineValue (newID + 1);
								CheckIfNewHigh (_score);
								scoreText.text = ""+_score;
								tiles [i + columnsToClimb + 1,j].SetTile (newID, images [newID]);
								alreadyCombined [i + columnsToClimb + 1, j] = true;
								canSpawn = true;
							}
						}
					}
				}
			}//end inner loop
		}//end outer loop

		Reset ();

		//spawn new tile
		if (canSpawn) {
			SpawnTile ();
		}

		if (won) {
			GameManager.instance.WonGame ();
		} else {
			//Done moving tiles, so we can wait for input again...
			GameManager.instance.ContinueGame ();
		}
	}

	//spawn either a 2 or a 4 tile
	public void SpawnTile(){
		posx = Random.Range (0, 4);
		posy = Random.Range (0, 4);
		id = Random.Range (8, 9);

		while (tiles [posx, posy] != null) {
			posx = Random.Range (0, 4);
			posy = Random.Range (0, 4);
		}

		tiles [posx,posy] = Instantiate (originalTile) as Tile;
		tiles [posx,posy].SetTile (id, images [id]);
		tiles[posx,posy].transform.position = new Vector3 (xPositions[posx], yPositions[posy], posz);
	}

	public void Reset(){
		for(int i = 0; i<4;i++){
			for(int j = 0; j<4;j++){
				alreadyCombined[i,j] = false;
			}
		}
	}

	public int CombineValue(int power){
		int value = 1;
		for (int i = 1; i <= power; i++) {
			value *= 2;
		}

		return value;
	}

	public void CheckIfNewHigh(int score){
		if (score > PlayerPrefs.GetInt ("highscore")) {
			PlayerPrefs.SetInt ("highscore", score);
			highScoreText.text = "" + score;
		}

	}

	public bool AreWeStuck(){
		//look for a null piece or a match
		//if we have either case, then we 
		//are NOT stuck

		bool stuck = true;
		int id;
		for (int i = 0; i < 4; i++) { 
			for (int j = 0; j < 4; j++) { 
				if (tiles [i, j] == null) {
					stuck = false;
					break;
				} else {
						
					id = tiles [i, j].GetID ();

					//check if any matches above
					if (j - 1 > 0) {
						if (tiles [i, j - 1] != null) {
							if (id == tiles [i, j - 1].GetID ()) {
								stuck = false;
								break;
							}
						}
					}
					//check if any matches below
					if (j + 1 < 4) {
						if(tiles[i,j+1] != null){
							if (id == tiles [i, j + 1].GetID ()) {
								stuck = false;
								break;
							}
						}
					}
					//check if any matches to the left
					if (i - 1 > 0) {
						if (tiles [i - 1, j] != null) {
							if (id == tiles [i - 1, j].GetID ()) {
								stuck = false;
								break;
							}
						}
					}
					//check if any matches to the right
					if (i + 1 < 4) {
						if (tiles [i + 1, j] != null) {
							if (id == tiles [i + 1, j].GetID ()) {
								stuck = false;
								break;
							}
						}
					}
				}
			}//end inner for
		}//end outer for

		return stuck;
	}
}
