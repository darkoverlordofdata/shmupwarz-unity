using UnityEngine;
using System.Collections;

public class Shrapnel : MonoBehaviour {

	public static Shrapnel Instance;
	public ParticleSystem particles;
	
	void Awake() {
		if (Instance != null) {
			Debug.LogError("Multiple instances of Shrapnel Provider!");
		}
	
		Instance = this;	
	}
	
	public void Hit(float x, float y) {
		Vector3 position;
		position = new Vector3(x, y, 0);
		instantiate(particles, position);
		
	}
	
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position) {
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;
	
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
	
		return newParticleSystem;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
