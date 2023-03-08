using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    	void Start()
    	{
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();		

		//pattern[10] = "B";
		//pattern[4] = "W";

		//pattern[11] = "BW";
		//pattern[1] = "WO";
		//pattern[7] = "WB";


		RubiksCube cube = RubiksCube.GenerateCube(pattern).GetComponent<RubiksCube>();
        	cube.transform.parent = transform.parent;

	
		cube.Randomize();
		cube.Solve(RubikData.DEFAULT_PATTERN);
	}

}
