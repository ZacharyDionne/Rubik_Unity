using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{


    void Start()
    {
        string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();


	pattern[0] = "WOB";
	pattern[2] = "WBR";

	RubiksCube cube = RubiksCube.GenerateCube(pattern).GetComponent<RubiksCube>();
	cube.CubeFinishedRandomizing += OnCubeFinishedRandomizing;
        cube.transform.parent = transform.parent;

	cube.Randomize();
	

	}

	void OnCubeFinishedRandomizing(object sender, EventArgs e)
	{
		((RubiksCube) sender).Solve(RubikData.DEFAULT_PATTERN);
	}

}
