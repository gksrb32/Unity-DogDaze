﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 15;
	private Vector3 target;

	void Start()
	{
		target = transform.position;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;
		}
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
	}
}
