using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MinionsFactory : MonoBehaviour {

	public GameObject Minion;
	public AudioClip Explosion;

	private bool _exploded = false;
	private int _count;

	private List<GameObject> _minions;
	private Dictionary<int, Vector3> _destinations;

	// Use this for initialization
	void Start () {
		_minions = new List<GameObject> ();
		_destinations = new Dictionary<int, Vector3> ();

		for (int i=0; i<1000; i++) {
			var monster = (GameObject)GameObject.Instantiate(Minion);
			_destinations.Add(monster.GetInstanceID(), GetNewLocation());
			monster.transform.position = GetNewLocation();
			_minions.Add(monster);
		}
	}
	
	// Update is called once per frame
	void Update () {	

		_minions.ForEach(m => 
		                 {
			var location = m.transform.position;
			var destination = _destinations[m.GetInstanceID()];

			if(Vector3.Distance(location, destination) < 5f){
				_destinations[m.GetInstanceID()] = GetNewLocation();
			}
			else{

				float tX = location.x > destination.x ? -10 : 10;
				float tZ = location.z > destination.z ? -10 : 10;

				Rigidbody rigid = (Rigidbody)m.GetComponent(typeof(Rigidbody));
				rigid.velocity = new Vector3(tX, 0, tZ);
				rigid.AddForce(new Vector3(tX, 0, tZ));

				//m.transform.Translate(tX, 0, tZ, Space.World);
			}

		});

		SearchForPlayer();

		if (Input.GetMouseButton(0) && !_exploded) {
			_exploded = true;
			var player = GameObject.Find ("Player");
			//AudioSource.PlayClipAtPoint(Explosion, player.transform.position);
			player.transform.Translate(0, 4f, 0);
			audio.PlayOneShot(Explosion, 10.0f);

			_count = _minions.FindAll(m => Vector3.Distance(player.transform.position, m.transform.position) < 100f).Count;

			Time.timeScale = 0;
		}

	}

	void OnGUI(){
		if (_exploded) {
			GUI.Label (new Rect (100, 100, Screen.width, Screen.height), "Your explosion destroyed " + _count + " cameras!");
			if(GUI.Button (new Rect(100, 200, 100, 20), "Click to restart")){
				Time.timeScale = 1;
				Application.LoadLevel(0);
			}
		}
	}

	void SearchForPlayer ()
	{
		var player = GameObject.Find ("Player");
		_minions.ForEach(m => {
			if(Vector3.Distance(player.transform.position, m.transform.position) < 100f){
				m.light.color = Color.red;
			}
			else{
				m.light.color = Color.white;
			}
		});
	}

	private Vector3 GetNewLocation(){
		return new Vector3(Random.Range (-5, 2000), 30f, Random.Range (-5, 2000));
	}
}
