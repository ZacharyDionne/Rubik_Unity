using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    	void Start()
    	{
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();

		pattern[4] = "G";
		pattern[22] = "B";

		pattern[12] = "W";
		pattern[14] = "Y";
	
	
		pattern[10] = "O";
		pattern[16] = "R";


		RubiksCube cube = RubiksCube.GenerateCube(pattern).GetComponent<RubiksCube>();
        	cube.transform.parent = transform.parent;

	
		//cube.Randomize();
		cube.Solve(RubikData.DEFAULT_PATTERN);
	}

}
