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

cube.Rotate(0, 1.0f);
cube.Rotate(0, 1.0f);
cube.Rotate(3, 1.0f);
cube.Rotate(6, 1.0f);
cube.Rotate(4, -1.0f);
cube.Rotate(5, -1.0f);
//cube.Rotate(5, -1.0f);
cube.Solve(RubikData.DEFAULT_PATTERN);

			//cube.Randomize();
			//cube.Solve(RubikData.DEFAULT_PATTERN);
	}

}
