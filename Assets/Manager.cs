using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    void Start()
    {
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();
		


		RubiksCube cube = RubiksCube.GenerateCube(pattern);

		if (cube == null)
        {
			Debug.Log("Cube impossible");
			return;
		}
			

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
		//L cas 3 case 1 il est sur le devant try the other side
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




		//cube.Solve(RubikData.DEFAULT_PATTERN);


		//cube.Randomize();

		//cube.GetJSON((json) => { Debug.Log(json); });
		cube.Solve(new string[] {"WOB", "OY", "WGO", "YB", "R", "WR", "YOG", "GW", "GWR", "BR", "G", "BO", "Y", "none", "W", "WB", "B", "YR", "BYR", "GR", "BRW", "YG", "O", "OW", "YBO", "OG", "GRY"});
	}

}
