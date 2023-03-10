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

	
			cube.Randomize();
			cube.Solve(RubikData.DEFAULT_PATTERN);
		}

}
