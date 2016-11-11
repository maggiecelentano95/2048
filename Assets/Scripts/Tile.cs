using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	[SerializeField] private GameObject myTile;
	private int _id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTile(int id, Sprite image) {
		_id = id;
		GetComponent<SpriteRenderer>().sprite = image;
	}

	public int GetID(){
		return _id;
	}
}
