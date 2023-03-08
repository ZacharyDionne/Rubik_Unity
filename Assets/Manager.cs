using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    	void Start()
    	{
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();		

		pattern[7] = "WB";
		pattern[1] = "YB";
		pattern[15] = "WR";
		pattern[9] = "YR";
		pattern[17] = "WO";
		pattern[11] = "YO";
		pattern[25] = "WG";
		pattern[19] = "YG";


		RubiksCube cube = RubiksCube.GenerateCube(pattern).GetComponent<RubiksCube>();
        	cube.transform.parent = transform.parent;

	
		//cube.Randomize();
		//cube.Solve(RubikData.DEFAULT_PATTERN);
	}

}
