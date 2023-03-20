using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    void Start()
    {
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();


		RubiksCube cube = RubiksCube.GenerateCube(pattern).GetComponent<RubiksCube>();
		cube.transform.parent = transform.parent;

		/*
		//Aucun n'est bien oriente
		cube.Rotate(0, 1.0f);
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, -1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(2, 1.0f);
		*/

		/*
		//L cas 3
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, 1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(3, -1.0f);
		*/

		/*
		//L cas 4
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, -1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(3, -1.0f);
		*/

		/*
		//L cas 2
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, 1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(8, 1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(5, 1.0f);
		cube.Rotate(6, 1.0f);
		cube.Rotate(7, -1.0f);
		*/

		/*
		//ligne cas 1
		cube.Rotate(0, -1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, 1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, -1.0f);
		cube.Rotate(8, 1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(5, 1.0f);
		cube.Rotate(6, 1.0f);
		cube.Rotate(7, 1.0f);
		*/

		/*
		//ligne cas 2
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, 1.0f);
		cube.Rotate(5, 1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(8, 1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(5, 1.0f);
		cube.Rotate(6, 1.0f);
		cube.Rotate(7, -1.0f);
		*/

		/*
		//deja fait
		cube.Rotate(0, 1.0f);
		cube.Rotate(3, 1.0f);
		cube.Rotate(4, 1.0f);
		cube.Rotate(5, -1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(2, 1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(3, -1.0f);
		cube.Rotate(5, 1.0f);
		cube.Rotate(6, 1.0f);
		cube.Rotate(7, -1.0f);
		*/




		cube.Solve(RubikData.DEFAULT_PATTERN);


		//cube.Randomize();
		//cube.Solve(RubikData.DEFAULT_PATTERN);
	}

}
