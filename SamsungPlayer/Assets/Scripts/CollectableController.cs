using UnityEngine;
using System.Collections;

public class CollectableController : MonoBehaviour {

    private GameObject gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Collect()
    {

    }
}
