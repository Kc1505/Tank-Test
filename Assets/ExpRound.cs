using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpRound : MonoBehaviour {

	public float Power;
	public float Radius;

	private int collided;

	public GameObject explosion;

	private void Start()
	{
		collided = 0;
	}

	private void Update()
	{
		Vector2 v = GetComponent<Rigidbody2D>().velocity;
		float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (collided == 0)
		{
			collided = 1;

			Destroy(Instantiate(explosion, transform.position, transform.rotation), 2f);
			

			Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.GetComponent<Transform>().position, Radius);
			int i = 0;
			while (i < hits.Length)
			{
				AddExplosionForce(hits[i].GetComponent<Rigidbody2D>(), Power, transform.position, Radius);
				hits[i].GetComponent<Rigidbody2D>();
				i++;
			}

			Destroy(gameObject);

			collided = 0;
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
