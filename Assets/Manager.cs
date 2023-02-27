using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] pattern = (string[]) RubiksCube.DEFAULT_PATTERN.Clone();





		GameObject cube = RubiksCube.GenerateCube(pattern);

        cube.transform.parent = transform.parent;


		//cube.GetComponent<RubiksCube>().Randomize();

		//cube.GetComponent<RubiksCube>().SetRotationMap(pattern);

		//cube.GetComponent<RubiksCube>().Solve(RubiksCube.DEFAULT_PATTERN);


	}
}
