using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {

	public AudioClip cannon;

	public GameObject Explosion;
	public GameObject Gun;
	public GameObject roundHE;

	public float Power;
	public float Radius;

	private int force;

	AudioSource gunFireSound;

	void Start () {
		gunFireSound = GetComponent<AudioSource>();

		force = 0;
	}

	void Update() {
		if (Input.GetKeyDown("e"))
		{
			Time.timeScale = 0.5f/ Time.timeScale;
		}

		if (Input.GetKeyDown("left ctrl"))
		{
			Destroy(Instantiate(Explosion, transform.position, transform.lossyScale.x > 0 ? (transform.rotation) : (Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 180))), 3f);


			//Explosion.GetComponent<ParticleSystem>().Play();
			gunFireSound.pitch = 0.8f * Time.timeScale;
			gunFireSound.PlayOneShot(cannon, 0.5f);

			force = 100;


			Collider2D[] hits = Physics2D.OverlapCircleAll(GetComponent<Transform>().position, Radius);
			int i = 0;
			while (i < hits.Length)
			{
				AddExplosionForce(hits[i].GetComponent<Rigidbody2D>(), Power, transform.position, Radius);
				hits[i].GetComponent<Rigidbody2D>();
				i++;
			}

			GameObject round = Instantiate(roundHE, transform.position, transform.rotation);
			round.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-transform.lossyScale.x*30 + Gun.GetComponent<Rigidbody2D>().velocity.x, 0 + Gun.GetComponent<Rigidbody2D>().velocity.y), ForceMode2D.Impulse);
		}

		if (force > 0)
		{
			force -= 10;
			Gun.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(transform.lossyScale.x*force * 10,0));
		}
	}

	public static void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius)
	{
		var dir = (body.transform.position - expPosition);
		float calc = 1 - (dir.magnitude / expRadius);
		if (calc <= 0)
		{
			calc = 0;
		}

		body.AddForce(dir.normalized * expForce * calc);
	}
}
