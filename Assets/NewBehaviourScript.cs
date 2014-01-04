using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour {

	public GameObject brick;

	private IList<GameObject> walls;


	// Use this for initialization
	void Start () {
		walls = new List<GameObject> ();



		for (int y = 0; y < 5; y++) {
			for (int x = 0; x < 5; x++) {
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				cube.AddComponent<Rigidbody>();
				cube.transform.position = new Vector3(x, y, 0);
				walls.Add(cube);
			}
		}


		walls.Add ((GameObject)GameObject.Instantiate (brick));
	}
	
	// Update is called once per frame
	void Update () {
		((List<GameObject>)walls).ForEach (w => w.transform.Rotate (new Vector3 (0f, 1f, 0f)));
	}
}
