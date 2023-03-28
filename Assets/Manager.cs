using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

	RubiksCube cube;



    void Start()
    {
		string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();



		cube = RubiksCube.GenerateCube(pattern);
		//cube = RubiksCube.GenerateCube(new string[] { "WBR", "WB", "BOY", "BY", "R", "BR", "YRB", "GY", "RYG", "WR", "Y", "GR", "B", "none", "G", "OG", "W", "RY", "RGW", "GW", "WOB", "OB", "O", "WO", "OWG", "YO", "YOG" });


		if (cube == null)
        {
			Debug.Log("Cube impossible");
			return;
		}
			

		cube.transform.parent = transform.parent;





        //cube.Solve(RubikData.DEFAULT_PATTERN);



        cube.Randomize();

        cube.GetJSON((json) => { Debug.Log(json); });

        //string[] pattern2 = (string[]) RubikData.DEFAULT_PATTERN.Clone();

        //pattern2[10] = "Y";
        //pattern2[16] = "W";





        cube.Solve(RubikData.DEFAULT_PATTERN);
		//cube.Solve(new string[]{"RWB","RG","RGW","BW","R","YR","YBO","BO","OWG","WO","W","YO","G","none","B","BY","Y","YG","WOB","RB","YOG","RW","O","WG","YRB","OG","GRY"});
		//cube.Solve(new string[]{"GRY","YB","RBY","YO","R","GR","WGO","YG","GWR","OW","W","BO","G","none","B","BW","Y","WG","YBO","RB","BWO","RY","O","WR","GYO","GO","BRW"});
		//cube.Solve(new string[] {"WOB", "OY", "WGO", "YB", "R", "WR", "YOG", "GW", "GWR", "BR", "G", "BO", "Y", "none", "W", "WB", "B", "YR", "BYR", "GR", "BRW", "YG", "O", "OW", "YBO", "OG", "GRY"});
	}









	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			cube.OrderList.Add(new RotateOrder(cube.Index, 1.0f));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			cube.OrderList.Add(new RotateOrder(cube.Index, -1.0f));
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			cube.Index--;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			cube.Index++;
		}


	}





}
