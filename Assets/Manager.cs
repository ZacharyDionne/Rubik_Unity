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





		//cube.Solve(RubikData.DEFAULT_PATTERN);


		//cube.Randomize();

		//cube.GetJSON((json) => { Debug.Log(json); });

		string[] pattern2 = (string[]) RubikData.DEFAULT_PATTERN.Clone();

		pattern[10] = "Y";
		pattern[16] = "W";
		pattern[4] = "R";
		pattern[14] = "B";
		pattern[22] = "O";
		pattern[12] = "G";


		cube.Solve(pattern2);
		//cube.Solve(new string[]{"RWB","RG","RGW","BW","R","YR","YBO","BO","OWG","WO","W","YO","G","none","B","BY","Y","YG","WOB","RB","YOG","RW","O","WG","YRB","OG","GRY"});
		//cube.Solve(new string[]{"GRY","YB","RBY","YO","R","GR","WGO","YG","GWR","OW","W","BO","G","none","B","BW","Y","WG","YBO","RB","BWO","RY","O","WR","GYO","GO","BRW"});
		//cube.Solve(new string[] {"WOB", "OY", "WGO", "YB", "R", "WR", "YOG", "GW", "GWR", "BR", "G", "BO", "Y", "none", "W", "WB", "B", "YR", "BYR", "GR", "BRW", "YG", "O", "OW", "YBO", "OG", "GRY"});
	}

}
