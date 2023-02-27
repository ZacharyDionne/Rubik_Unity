using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RubiksCube : MonoBehaviour
{
	public static string[] DEFAULT_PATTERN = {
		"WBR",	//0 UP
		"WB",	//1 UP
		"WOB",	//2 UP
		"RB",	//3 LEFT
		"B",	//4 FRONT DECLARATION
		"OB",	//5 RIGHT
		"YRB",	//6 DOWN
		"YB",	//7 DOWN
		"YBO",	//8 DOWN
		"WR",	//9 UP
		"W",	//10 UP DECLARATION
		"WO",	//11 UP
		"R",	//12 LEFT DECLARATION
		"none", //13
		"O",	//14 RIGHT DECLARATION
		"YR",	//15 DOWN
		"Y",	//16 DOWN DECLARATION
		"YO",	//17 DOWN
		"WRG",	//18 UP
		"WG",	//19 UP
		"WGO",	//20 UP
		"RG",	//21 LEFT
		"G",	//22 BACK DECLARATION
		"OG",	//23 RIGHT
		"YGR",	//24 DOWN
		"YG",	//25 DOWN
		"YOG"	//26 DOWN
	};


	protected static Dictionary<string, Dictionary<string, Vector3>> orientationMap = new()
	{
		{
			"WRB",
			new Dictionary<string, Vector3>
			{
				{"W", new Vector3(0.0f, 0.0f, 0.0f)},
				{"R", new Vector3(0.0f, 0.0f, 0.0f)},
				{"B", new Vector3(0.0f, 0.0f, 0.0f)}
			}
		}
		
	};


	protected static readonly int[] UP = { 0, 1, 2, 9, 10, 11, 18, 19, 20};
	protected static readonly int[] DOWN = { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
	protected static readonly int[] FRONT = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
	protected static readonly int[] BACK = { 18, 19, 20, 21, 22, 23, 24, 25, 26 };
	protected static readonly int[] LEFT = { 0, 9, 18, 3, 12, 21, 6, 15, 24};
	protected static readonly int[] RIGHT = { 2, 11, 20, 5, 13, 23, 8, 17, 26 };

	protected static readonly int[] CORNERS = { 0, 2, 6, 8, 18, 20, 24, 26 };
	protected static readonly int[] EDGES = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
	protected static readonly int[] MIDDLES = { 4, 10, 12, 14, 16, 22 };

	protected static Vector3[] worldPositions = {
		new Vector3(2.0f, 2.0f, 2.0f),
		new Vector3(0.0f, 2.0f, 2.0f),
		new Vector3(-2.0f, 2.0f, 2.0f),
		new Vector3(2.0f, 0.0f, 2.0f),
		new Vector3(0.0f, 0.0f, 2.0f),
		new Vector3(-2.0f, 0.0f, 2.0f),
		new Vector3(2.0f, -2.0f, 2.0f),
		new Vector3(0.0f, -2.0f, 2.0f),
		new Vector3(-2.0f, -2.0f, 2.0f),
		new Vector3(2.0f, 2.0f, 0.0f),
		new Vector3(0.0f, 2.0f, 0.0f),
		new Vector3(-2.0f, 2.0f, 0.0f),
		new Vector3(2.0f, 0.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(-2.0f, 0.0f, 0.0f),
		new Vector3(2.0f, -2.0f, 0.0f),
		new Vector3(0.0f, -2.0f, 0.0f),
		new Vector3(-2.0f, -2.0f, 0.0f),
		new Vector3(2.0f, 2.0f, -2.0f),
		new Vector3(0.0f, 2.0f, -2.0f),
		new Vector3(-2.0f, 2.0f, -2.0f),
		new Vector3(2.0f, 0.0f, -2.0f),
		new Vector3(0.0f, 0.0f, -2.0f),
		new Vector3(-2.0f, 0.0f, -2.0f),
		new Vector3(2.0f, -2.0f, -2.0f),
		new Vector3(0.0f, -2.0f, -2.0f),
		new Vector3(-2.0f, -2.0f, -2.0f)
	};
	

	protected static Dictionary<string, int[]> directions = new()
	{
		{ "W", UP},
		{ "Y", DOWN},
		{ "B", FRONT},
		{ "G", BACK},
		{ "R", LEFT },
		{ "O", RIGHT }
	};




	public static readonly byte CLOCKWISE = 1;
	public static readonly byte COUNTERCLOCKWISE = 2;


	public static GameObject GenerateCube(string[] pattern)
	{
		GameObject gameObject = Instantiate(Resources.Load<GameObject>("Rubik's Cube"));
		gameObject.GetComponent<RubiksCube>().currentPattern = pattern;
		//gameObject.GetComponent<RubiksCube>().SetRotationMap(pattern);


		Dictionary<int, Vector3> middlesFrame = new Dictionary<int, Vector3>();
		middlesFrame.Add(4, new Vector3(0.0f, 0.0f, 0.0f));
		middlesFrame.Add(10, new Vector3(90.0f, 0.0f, 0.0f));
		middlesFrame.Add(12, new Vector3(0.0f, 0.0f, -90.0f));
		middlesFrame.Add(14, new Vector3(0.0f, 0.0f, 90.0f));
		middlesFrame.Add(16, new Vector3(-90.0f, 0.0f, 0.0f));
		middlesFrame.Add(22, new Vector3(180.0f, 0.0f, 0.0f));



		Dictionary<int, Vector3> edgesFrame = new Dictionary<int, Vector3>();
		edgesFrame.Add(1, new Vector3(0.0f, 0.0f, 0.0f));
		edgesFrame.Add(3, new Vector3(0.0f, -90.0f, 0.0f));
		edgesFrame.Add(5, new Vector3(0.0f, 90.0f, 0.0f));
		edgesFrame.Add(7, new Vector3(0.0f, 180.0f, 0.0f));
		edgesFrame.Add(9, new Vector3(0.0f, 0.0f, -90.0f));
		edgesFrame.Add(11, new Vector3(0.0f, 0.0f, 90.0f));
		edgesFrame.Add(15, new Vector3(0.0f, 180.0f, -90.0f));
		edgesFrame.Add(17, new Vector3(0.0f, 180.0f, 90.0f));
		edgesFrame.Add(19, new Vector3(0.0f, 0.0f, 180.0f));
		edgesFrame.Add(21, new Vector3(180.0f, -90.0f, 0.0f));
		edgesFrame.Add(23, new Vector3(180.0f, 90.0f, 0.0f));
		edgesFrame.Add(25, new Vector3(0.0f, 180.0f, 180.0f));





		for (int i = 0; i < DEFAULT_PATTERN.Length; i++)
        {
		
			
			Transform cuby = gameObject.transform.Find(DEFAULT_PATTERN[FindCuby(DEFAULT_PATTERN, pattern[i])]);
			gameObject.GetComponent<RubiksCube>().cubyPositions[i] = cuby;
			

			cuby.position = worldPositions[i];
			
			int defaultPosition = FindCuby(DEFAULT_PATTERN, pattern[i]);

			switch (pattern[i].Length)
            {
				case 1:
					cuby.Rotate(middlesFrame[defaultPosition] - middlesFrame[i]);

					break;

				case 2:
					cuby.Rotate(edgesFrame[defaultPosition] - edgesFrame[i]);
					gameObject.GetComponent<RubiksCube>().PhysicRotateEdge(pattern[i], cuby);

					break;

				case 3:
					gameObject.GetComponent<RubiksCube>().CorrectCornerOrientation(cuby);
					gameObject.GetComponent<RubiksCube>().PhysicRotateCorner(pattern[i], cuby);

					break;
            }
			


        }


		return gameObject;
    }





	public static int FindCuby(string[] pattern, string name)
	{
		string cuby = Array.Find<string>(pattern, (string currentName) => {


			if (name.Length != currentName.Length)
				return false;

			for (int i = 0; i < name.Length; i++)
			{
				if (!currentName.Contains(name[i]))
					return false;
			}

			for (int i = 0; i < currentName.Length; i++)
			{
				if (!name.Contains(currentName[i]))
					return false;
			}


			return true;
		});

		if (cuby == null)
			return -1;

		return Array.IndexOf(pattern, cuby);
	}


	protected int FindCuby(Transform cuby)
    {
		string[] cubies = new string[27];


		for (int i = 0; i < 27; i++)
        {
			cubies[i] = transform.GetChild(i).name;
		}


		return Array.IndexOf(cubies, Array.Find<string>(cubies, (tmpCuby) => {
			
			if (cuby.name.Length != tmpCuby.Length)
				return false;

			for (int i = 0; i < cuby.name.Length; i++)
			{
				if (!tmpCuby.Contains(cuby.name[i]))
					return false;
			}

			for (int i = 0; i < tmpCuby.Length; i++)
			{
				if (!cuby.name.Contains(tmpCuby[i]))
					return false;
			}


			return true;
		}));

    }



	protected bool IsCubyOn(string[] pattern, int[] face, Transform cuby)
    {
		int position = FindCuby(cuby);

		for (int i = 0; i < face.Length; i++)
        {
			if (position == face[i])
				return true;
        }

		return false;
    }




	protected static string[] FindCubiesOn(string[] pattern, int[] face)
    {
		string[] cubies = new string[9];

		for (int i = 0; i < face.Length; i++)
        {
			cubies[i] = pattern[i];
        }

		return cubies;
    }























	//used to rotate cubies from the central axis of the cube
	[SerializeField]
	protected GameObject rotationAnchor;


	//variable of control
	protected bool isRotating;

	//variables for the rotate animation
	protected int angleCounter;
	protected const float ROTATION_SLOWNESS = 30.0f;
	protected const float ROTATION = 180.0f / ROTATION_SLOWNESS;
	protected const float ROTATION_STEP = ROTATION_SLOWNESS / 2.0f;




	// -1 or 1, defines wich direction to rotate
	protected float direction;



	//Variable used to randomize the cube
	protected int randomCounter;
	//Store the informations before randomizing the cube
	protected int lastIndex;
	protected float lastDirection;





	protected Vector3 currentAxis;


	//variables for keeping track of cubies' position
	protected Transform[] cubyPositions;
	protected Transform[] bufferPositions;



	protected string[] currentPattern;
	









	//Array to tell wich cubie positions are affected by a rotation
	protected int index;
    protected int[,] rotationPositions = {
		{  0,  1,  2,  3,  4,  5,  6,  7,  8 },      // rotation 0, cuby at 0, 1, 2, ... affected 
		{  9, 10, 11, 12, 13, 14, 15, 16, 17 },
		{ 18, 19, 20, 21, 22, 23, 24, 25, 26 },
		{  0,  9, 18,  1, 10, 19,  2, 11, 20 },
		{  3, 12, 21,  4, 13, 22,  5, 14, 23 },
		{  6, 15, 24,  7, 16, 25,  8, 17, 26 },
		{  0,  9, 18,  3, 12, 21,  6, 15, 24 },
		{  1, 10, 19,  4, 13, 22,  7, 16, 25 },
		{  2, 11, 20,  5, 14, 23,  8, 17, 26 }
	};

	System.Random random;








	void Start()
    {
		isRotating = false;
		index = 0;
		randomCounter = -1;

		bufferPositions = new Transform[9];


	}

	// Update is called once per frame
	void Update()
	{
		//A and D to rotate, LEFT and RIGHT to change where to rotate
		if (!isRotating)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				direction = 1.0f;
				RotateBegin();
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				direction = -1.0f;
				RotateBegin();
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (index != 0)
				{
					index -= 1;
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (index != 8)
				{
					index += 1;
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
            {

            }
			
		}
	}




	void FixedUpdate()
    {
		if (isRotating)
			RotateAnimation();
		else if (randomCounter > 0)
			NextRandom();
		else if (randomCounter == 0)
        {
			direction = lastDirection;
			index = lastIndex;
			randomCounter = -1;
		}
	}


    private void Awake()
    {
		random = new System.Random();
		cubyPositions = new Transform[27];
	}

















    public void Solve(string[] obectivePattern)
    {
		Dictionary<string, int[]> currentDirections = new Dictionary<string, int[]> {
			{ "UP", directions[obectivePattern[10]]},
			{ "DOWN", directions[obectivePattern[16]] },
			{ "FRONT", directions[obectivePattern[4]] },
			{ "BACK", directions[obectivePattern[22]] },
			{ "LEFT", directions[obectivePattern[12]] },
			{ "RIGHT", directions[obectivePattern[14]] }
		};

		string top = obectivePattern[10];
		string down = obectivePattern[16];
		string front = obectivePattern[4];
		string back = obectivePattern[22];
		string left = obectivePattern[12];
		string right = obectivePattern[14];


		//make the top cross
		//find cross edges

		string[] topCross = FindCubyWithColor(currentPattern, top);

		foreach (string str in topCross)
			Debug.Log(str);

		

	}






	protected static string[] FindCubyWithColor(string[] pattern, string color)
    {
		return Array.FindAll(pattern, (string str) => { return str.Contains(color[0]) && str.Length == 2; });
	}








	public bool IsCorner(int position)
	{
		return Array.IndexOf(CORNERS, position) != -1;
	}

	public bool IsCorner(string name)
	{
		return name.Length == 3;
	}

	public bool IsMiddle(int position)
	{
		return Array.IndexOf(MIDDLES, position) != -1;
	}

	public bool IsMiddle(string name)
	{
		return name.Length == 1;
	}

	public bool IsEdge(int position)
	{
		return Array.IndexOf(EDGES, position) != -1;
	}

	public bool IsEdge(string name)
	{
		return name.Length == 2;
	}




	public int FindCuby(string name)
    {
		string cuby = Array.Find<string>(currentPattern, (string currentName) => {


			if (name.Length != currentName.Length)
				return false;

			for (int i = 0; i < name.Length; i++)
            {
				if (!currentName.Contains(name[i]))
					return false;
            }

			for (int i = 0; i < currentName.Length; i++)
			{
				if (!name.Contains(currentName[i]))
					return false;
			}


			return true;
		});

		if (cuby == null)
			return -1;

		return Array.IndexOf(currentPattern, cuby);
    }









	protected void CorrectCornerOrientation(Transform cuby)
    {
		int i = FindCuby(cuby.name);
		int defaultPosition = FindCuby(DEFAULT_PATTERN, cuby.name);

		if (i == 0)
		{
			if (defaultPosition == 2)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
		}
		else if (i == 2)
		{
			if (defaultPosition == 0)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
		}
		else if (i == 6)
		{
			if (defaultPosition == 0)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
		}
		else if (i == 8)
		{
			if (defaultPosition == 0)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
		}
		else if (i == 18)
		{
			if (defaultPosition == 0)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
		}
		else if (i == 20)
		{
			if (defaultPosition == 0)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
		}
		else if (i == 24)
		{
			if (defaultPosition == 0)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 26)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
		}
		else if (i == 26)
		{
			if (defaultPosition == 0)
				cuby.Rotate(180.0f, 0.0f, -90.0f);
			else if (defaultPosition == 2)
				cuby.Rotate(180.0f, 0.0f, 0.0f);
			else if (defaultPosition == 6)
				cuby.Rotate(0.0f, 0.0f, 180.0f);
			else if (defaultPosition == 8)
				cuby.Rotate(0.0f, 0.0f, -90.0f);
			else if (defaultPosition == 18)
				cuby.Rotate(180.0f, 0.0f, 180.0f);
			else if (defaultPosition == 20)
				cuby.Rotate(180.0f, 0.0f, 90.0f);
			else if (defaultPosition == 24)
				cuby.Rotate(0.0f, 0.0f, 90.0f);
		}
	}



	protected void PhysicRotateCorner(string desiredOrientation, Transform cuby)
    {
		if (cuby.name.Equals(desiredOrientation))
			return;

		int position = FindCuby(cuby.name);

		Vector3 rotation;

		if (position == 0)
			rotation = new Vector3(0.0f, 90.0f, 90.0f);
		else if (position == 2)
			rotation = new Vector3(-90.0f, 0.0f, 90.0f);
		else if (position == 6)
			rotation = new Vector3(90.0f, 0.0f, 90.0f);
		else if (position == 8)
			rotation = new Vector3(0.0f, -90.0f, 90.0f);
		else if (position == 18)
			rotation = new Vector3(90.0f, 0.0f, -90.0f);
		else if (position == 20)
			rotation = new Vector3(0.0f, 90.0f, -90.0f);
		else if (position == 24)
			rotation = new Vector3(0.0f, -90.0f, -90.0f);
		else if (position == 26)
			rotation = new Vector3(-90.0f, 0.0f, -90.0f);
		else
			throw new Exception($"Position {position} invalide");


		for (int i = 0; i < 2; i++)
        {
			cuby.Rotate(rotation, Space.World);
			RotateCorner(currentPattern, cuby, CLOCKWISE);

			if (cuby.name.Equals(desiredOrientation))
				return;
        }

		throw new Exception($"Impossible de trouver l'orientation {desiredOrientation} pour {cuby.name}");
    }



	protected void PhysicRotateEdge(string desiredOrientation, Transform cuby)
	{
		if (cuby.name.Equals(desiredOrientation))
			return;

		int position = FindCuby(cuby.name);

		Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);

		Dictionary<int, Vector3> edgesFrame = new Dictionary<int, Vector3>();
		edgesFrame.Add(1, new Vector3(0.0f, 0.0f, 0.0f));
		edgesFrame.Add(3, new Vector3(0.0f, -90.0f, 0.0f));
		edgesFrame.Add(5, new Vector3(0.0f, 90.0f, 0.0f));
		edgesFrame.Add(7, new Vector3(0.0f, 180.0f, 0.0f));
		edgesFrame.Add(9, new Vector3(0.0f, 0.0f, -90.0f));
		edgesFrame.Add(11, new Vector3(0.0f, 0.0f, 90.0f));
		edgesFrame.Add(15, new Vector3(0.0f, 180.0f, -90.0f));
		edgesFrame.Add(17, new Vector3(0.0f, 180.0f, 90.0f));
		edgesFrame.Add(19, new Vector3(0.0f, 0.0f, 180.0f));
		edgesFrame.Add(21, new Vector3(180.0f, -90.0f, 0.0f));
		edgesFrame.Add(23, new Vector3(180.0f, 90.0f, 0.0f));
		edgesFrame.Add(25, new Vector3(0.0f, 180.0f, 180.0f));


		cuby.Rotate(edgesFrame[position], Space.World);
		cuby.Rotate(rotation, Space.World);
		cuby.Rotate(-edgesFrame[position], Space.World);
		RotateEdge(cuby);

		if (cuby.name.Equals(desiredOrientation))
			return;

		throw new Exception($"Impossible de trouver l'orientation {desiredOrientation} pour {cuby.name}");
	}



	protected static void RotateCorner(string[] pattern, Transform cuby, byte orientation)
	{
		//int position = FindCuby(pattern, cuby);

		if (orientation == CLOCKWISE)
			//pattern[position] = pattern[position][2].ToString() + pattern[position][0].ToString() + pattern[position][1].ToString();
			cuby.name = cuby.name[2].ToString() + cuby.name[0].ToString() + cuby.name[1].ToString();
		else
			//pattern[position] = pattern[position][1].ToString() + pattern[position][2].ToString() + pattern[position][0].ToString();
			cuby.name = cuby.name[1].ToString() + cuby.name[2].ToString() + cuby.name[0].ToString();
	}


	protected void RotateEdge(Transform cuby)
	{
		Dictionary<int, Vector3> edgeRotations = new Dictionary<int, Vector3>();
		edgeRotations.Add(1, new Vector3(-90.0f, 180.0f, 0.0f));
		edgeRotations.Add(3, new Vector3(0.0f, 90.0f, 180.0f));
		edgeRotations.Add(5, new Vector3(0.0f, -90.0f, 180.0f));
		edgeRotations.Add(7, new Vector3(90.0f, 180.0f, 0.0f));
		edgeRotations.Add(9, new Vector3(0.0f, 180.0f, 90.0f));
		edgeRotations.Add(11, new Vector3(0.0f, 180.0f, -90.0f));
		edgeRotations.Add(15, new Vector3(0.0f, 180.0f, -90.0f));
		edgeRotations.Add(17, new Vector3(0.0f, 180.0f, 90.0f));
		edgeRotations.Add(19, new Vector3(90.0f, 180.0f, 0.0f));
		edgeRotations.Add(21, new Vector3(0.0f, -90.0f, 180.0f));
		edgeRotations.Add(23, new Vector3(0.0f, 90.0f, 180.0f));
		edgeRotations.Add(25, new Vector3(-90.0f, 0.0f, 180.0f));

		//Transform cuby = transform.Find(position.ToString());

		cuby.Rotate(edgeRotations[FindCuby(cuby.name)], Space.World);

		//currentPattern[position] = currentPattern[position][1].ToString() + currentPattern[position][0].ToString();
		cuby.name = cuby.name[1].ToString() + cuby.name[0].ToString();
	}

















	public string[] GetPattern()
    {
		string[] pattern = new string[27];

		for (int i = 0; i < 27; i++)
        {
			

			for (int j = 0; j < 27; j++)
            {
				Transform cuby = transform.GetChild(j);

				Vector3 cubyPosition = new Vector3(Mathf.RoundToInt(cuby.localPosition.x), Mathf.RoundToInt(cuby.localPosition.y), Mathf.RoundToInt(cuby.localPosition.z));

				if (cubyPosition == worldPositions[i])
                {
					pattern[i] = cuby.name;
					break;
                }
            }
        }

		return pattern;
    }



	public string GetJSONPattern()
    {
		string[] pattern = GetPattern();
		string json = "[";

		for (int i = 0; i < pattern.Length; i++)
        {
			json += "\"" + pattern[i] + "\"";
			if (i + 1 != pattern.Length)
            {
				json += ",";
            }
        }

		json += "]";

		return json;
    }



















	protected int[] rotationMap = new int[9];
	protected float[] directionMap = new float[9];

	/*
	public void SetRotationMap(string[] targetPattern)
	{

		string top = targetPattern[10];
		string down = targetPattern[16];
		string front = targetPattern[4];
		string back = targetPattern[22];
		string left = targetPattern[12];
		string right = targetPattern[14];

		if (top.Equals("W"))
		{
			if (front.Equals("B"))
			{
				rotationMap[0] = 0;
				rotationMap[1] = 1;
				rotationMap[2] = 2;
				rotationMap[3] = 3;
				rotationMap[4] = 4;
				rotationMap[5] = 5;
				rotationMap[6] = 6;
				rotationMap[7] = 7;
				rotationMap[8] = 8;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("O"))
			{
				rotationMap[0] = 8;
				rotationMap[1] = 7;
				rotationMap[2] = 6;
				rotationMap[3] = 3;
				rotationMap[4] = 4;
				rotationMap[5] = 5;
				rotationMap[6] = 0;
				rotationMap[7] = 1;
				rotationMap[8] = 2;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("G"))
			{
				rotationMap[0] = 2;
				rotationMap[1] = 1;
				rotationMap[2] = 0;
				rotationMap[3] = 3;
				rotationMap[4] = 4;
				rotationMap[5] = 5;
				rotationMap[6] = 8;
				rotationMap[7] = 7;
				rotationMap[8] = 6;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("R"))
			{
				rotationMap[0] = 6;
				rotationMap[1] = 7;
				rotationMap[2] = 8;
				rotationMap[3] = 3;
				rotationMap[4] = 4;
				rotationMap[5] = 5;
				rotationMap[6] = 2;
				rotationMap[7] = 1;
				rotationMap[8] = 0;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
		}
		else if (top.Equals("B"))
		{
			if (front.Equals("W"))
			{
				rotationMap[0] = 3;
				rotationMap[1] = 4;
				rotationMap[2] = 5;
				rotationMap[3] = 0;
				rotationMap[4] = 1;
				rotationMap[5] = 2;
				rotationMap[6] = 8;
				rotationMap[7] = 7;
				rotationMap[8] = 6;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("O"))
			{
				rotationMap[0] = 8;
				rotationMap[1] = 7;
				rotationMap[2] = 6;
				rotationMap[3] = 3;
				rotationMap[4] = 4;
				rotationMap[5] = 5;
				rotationMap[6] = 0;
				rotationMap[7] = 1;
				rotationMap[8] = 2;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("Y"))
			{
				rotationMap[0] = 5;
				rotationMap[1] = 4;
				rotationMap[2] = 3;
				rotationMap[3] = 0;
				rotationMap[4] = 1;
				rotationMap[5] = 2;
				rotationMap[6] = 6;
				rotationMap[7] = 7;
				rotationMap[8] = 8;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("R"))
			{
				rotationMap[0] = 6;
				rotationMap[1] = 7;
				rotationMap[2] = 8;
				rotationMap[3] = 0;
				rotationMap[4] = 1;
				rotationMap[5] = 2;
				rotationMap[6] = 3;
				rotationMap[7] = 4;
				rotationMap[8] = 5;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
		}
		else if (top.Equals("Y"))
		{
			if (front.Equals("B"))
			{
				rotationMap[0] = 0;
				rotationMap[1] = 1;
				rotationMap[2] = 2;
				rotationMap[3] = 5;
				rotationMap[4] = 4;
				rotationMap[5] = 3;
				rotationMap[6] = 8;
				rotationMap[7] = 7;
				rotationMap[8] = 6;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("R"))
			{
				rotationMap[0] = 6;
				rotationMap[1] = 7;
				rotationMap[2] = 8;
				rotationMap[3] = 5;
				rotationMap[4] = 4;
				rotationMap[5] = 3;
				rotationMap[6] = 0;
				rotationMap[7] = 1;
				rotationMap[8] = 2;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("G"))
			{
				rotationMap[0] = 2;
				rotationMap[1] = 1;
				rotationMap[2] = 0;
				rotationMap[3] = 5;
				rotationMap[4] = 4;
				rotationMap[5] = 3;
				rotationMap[6] = 6;
				rotationMap[7] = 7;
				rotationMap[8] = 8;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("O"))
			{
				rotationMap[0] = 8;
				rotationMap[1] = 7;
				rotationMap[2] = 6;
				rotationMap[3] = 5;
				rotationMap[4] = 4;
				rotationMap[5] = 3;
				rotationMap[6] = 2;
				rotationMap[7] = 1;
				rotationMap[8] = 0;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
		}
		else if (top.Equals("G"))
		{
			if (front.Equals("W"))
			{
				rotationMap[0] = 3;
				rotationMap[1] = 4;
				rotationMap[2] = 5;
				rotationMap[3] = 2;
				rotationMap[4] = 1;
				rotationMap[5] = 0;
				rotationMap[6] = 6;
				rotationMap[7] = 7;
				rotationMap[8] = 8;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("R"))
			{
				rotationMap[0] = 6;
				rotationMap[1] = 7;
				rotationMap[2] = 8;
				rotationMap[3] = 2;
				rotationMap[4] = 1;
				rotationMap[5] = 0;
				rotationMap[6] = 5;
				rotationMap[7] = 4;
				rotationMap[8] = 3;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("Y"))
			{
				rotationMap[0] = 5;
				rotationMap[1] = 4;
				rotationMap[2] = 3;
				rotationMap[3] = 2;
				rotationMap[4] = 1;
				rotationMap[5] = 0;
				rotationMap[6] = 8;
				rotationMap[7] = 7;
				rotationMap[8] = 6;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("O"))
			{
				rotationMap[0] = 8;
				rotationMap[1] = 7;
				rotationMap[2] = 6;
				rotationMap[3] = 2;
				rotationMap[4] = 1;
				rotationMap[5] = 0;
				rotationMap[6] = 3;
				rotationMap[7] = 4;
				rotationMap[8] = 5;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
		}
		else if (top.Equals("R"))
		{
			if (front.Equals("W"))
			{
				rotationMap[0] = 3;
				rotationMap[1] = 4;
				rotationMap[2] = 5;
				rotationMap[3] = 6;
				rotationMap[4] = 7;
				rotationMap[5] = 8;
				rotationMap[6] = 0;
				rotationMap[7] = 1;
				rotationMap[8] = 2;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("G"))
			{
				rotationMap[0] = 2;
				rotationMap[1] = 1;
				rotationMap[2] = 0;
				rotationMap[3] = 6;
				rotationMap[4] = 7;
				rotationMap[5] = 8;
				rotationMap[6] = 3;
				rotationMap[7] = 4;
				rotationMap[8] = 5;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("Y"))
			{
				rotationMap[0] = 5;
				rotationMap[1] = 4;
				rotationMap[2] = 3;
				rotationMap[3] = 6;
				rotationMap[4] = 7;
				rotationMap[5] = 8;
				rotationMap[6] = 2;
				rotationMap[7] = 1;
				rotationMap[8] = 0;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("B"))
			{
				rotationMap[0] = 0;
				rotationMap[1] = 1;
				rotationMap[2] = 2;
				rotationMap[3] = 6;
				rotationMap[4] = 7;
				rotationMap[5] = 8;
				rotationMap[6] = 5;
				rotationMap[7] = 4;
				rotationMap[8] = 3;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = -1.0f;
				directionMap[4] = -1.0f;
				directionMap[5] = -1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
		}
		else if (top.Equals("O"))
		{
			if (front.Equals("W"))
			{
				rotationMap[0] = 3;
				rotationMap[1] = 4;
				rotationMap[2] = 5;
				rotationMap[3] = 8;
				rotationMap[4] = 7;
				rotationMap[5] = 6;
				rotationMap[6] = 2;
				rotationMap[7] = 1;
				rotationMap[8] = 0;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("G"))
			{
				rotationMap[0] = 2;
				rotationMap[1] = 1;
				rotationMap[2] = 0;
				rotationMap[3] = 8;
				rotationMap[4] = 7;
				rotationMap[5] = 6;
				rotationMap[6] = 5;
				rotationMap[7] = 4;
				rotationMap[8] = 3;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = 1.0f;
				directionMap[7] = 1.0f;
				directionMap[8] = 1.0f;
			}
			else if (front.Equals("Y"))
			{
				rotationMap[0] = 5;
				rotationMap[1] = 4;
				rotationMap[2] = 3;
				rotationMap[3] = 8;
				rotationMap[4] = 7;
				rotationMap[5] = 6;
				rotationMap[6] = 0;
				rotationMap[7] = 1;
				rotationMap[8] = 2;

				directionMap[0] = -1.0f;
				directionMap[1] = -1.0f;
				directionMap[2] = -1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
			else if (front.Equals("B"))
			{
				rotationMap[0] = 0;
				rotationMap[1] = 1;
				rotationMap[2] = 2;
				rotationMap[3] = 8;
				rotationMap[4] = 7;
				rotationMap[5] = 6;
				rotationMap[6] = 3;
				rotationMap[7] = 4;
				rotationMap[8] = 5;

				directionMap[0] = 1.0f;
				directionMap[1] = 1.0f;
				directionMap[2] = 1.0f;
				directionMap[3] = 1.0f;
				directionMap[4] = 1.0f;
				directionMap[5] = 1.0f;
				directionMap[6] = -1.0f;
				directionMap[7] = -1.0f;
				directionMap[8] = -1.0f;
			}
		}

	}

	*/











































	public void Randomize()
    {
		randomCounter = 20;
		lastDirection = direction;
		lastIndex = index;

		NextRandom();
	}



	protected void NextRandom()
    {

		index = random.Next(0, 9);
		direction = random.Next(0, 2) == 0 ? 1.0f : -1.0f;


		randomCounter--;

		RotateBegin();
	}



















	protected void RotateBegin()
    {
		isRotating = true;
		rotationAnchor.transform.rotation = Quaternion.identity;
		angleCounter = 0;
		

		if (index <= 2)
		{
			currentAxis = Vector3.back;
		}
		else if (index <= 5)
        {
			currentAxis = Vector3.down;
		}
		else
        {
			currentAxis = Vector3.right;
		}





		
		for (int i = 0; i < 9; i++)
        {
			Transform cuby = cubyPositions[rotationPositions[index, i]];
			cuby.parent = rotationAnchor.transform;
		}
		

	}




	protected void RotateAnimation()
    {
		rotationAnchor.transform.Rotate(currentAxis, ROTATION * direction);
		angleCounter += 1;


		if (angleCounter == ROTATION_STEP)
			RotateEnd();
	}


	protected void RotateEnd()
    {

		while (rotationAnchor.transform.childCount > 0)
        {
			Transform cuby = rotationAnchor.transform.GetChild(0);
			cuby.parent = transform;

		}


		UpdatePositions();

		isRotating = false;
	}




	//Let the code be aware of cubies' position
	protected void UpdatePositions()
    {
		/*
			Here, there is a pattern that is exploited and reusable for all rotations.
			For example we want to rotate the blue face to the left. This is index
			0 in rotation_positions. Each number represents a small cube (cuby) from
			left to right and top to bottom.
			0|1|2
			3|4|5
			6|7|8
			
			Before rotating, the first cuby_positions are : 
			[0, 1, 2, 3, 4, 5, 6, 7, 8]
			
			And after rotation, the result expected is : 
			[2, 5, 8, 1, 4, 7, 0, 3, 6]
			
			If we take a look, we can see there is a pattern. The code below just do
			the correct translations in the array.
		 */

		
		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPositions[currentPosition] = cubyPositions[rotationPositions[index, i + j]];






				if (IsCorner(cuby))
				{

				






					Transform cuby = cubyPositions[currentPosition];


						Transform oldPosition = cubyPositions[rotationPositions[index, currentPosition]];


						if (IsCubyOn(currentPattern, UP, oldPosition))
						{
							if (IsCubyOn(currentPattern, UP, cuby))
							{
								Debug.Log("clockwise rotating " + cuby.name);
								RotateCorner(currentPattern, cuby, CLOCKWISE);
							}
							else
							{
								RotateCorner(currentPattern, cuby, COUNTERCLOCKWISE);
							}
						}
						else
						{
							if (IsCubyOn(currentPattern, DOWN, cuby))
							{
								RotateCorner(currentPattern, cuby, COUNTERCLOCKWISE);
							}
							else
							{
								Debug.Log("clockwise rotating " + cuby.name);
								RotateCorner(currentPattern, cuby, CLOCKWISE);
							}
						}


				}
		
				else if (IsEdge(cuby))
				{

				}

				








				currentPosition += 1;
			}
		}
		





		//If we want to rotate right, the array is the reverse of what expected to
		//left. So, we just have to reverse the order in the buffer.
		if (direction == -1.0f)
		Array.Reverse(bufferPositions);





		for (int i = 0; i < 9; i++)
        {
			cubyPositions[rotationPositions[index, i]] = bufferPositions[i];
		}


















		rotationAnchor.transform.SetAsLastSibling();



		//Print(GetPattern());
		Debug.Log(GetJSONPattern());







		
		/*
		string[] bufferPattern = new string[9];



		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPattern[currentPosition] = currentPattern[i];
				currentPosition += 1;
			}
		}






		currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				if (IsCorner(bufferPattern[currentPosition]))
                {	
					if (rotationMap[index] <= 2 || rotationMap[index] >= 6)
                    {
						if
						(
							(
								IsCubyOn(bufferPattern, UP, bufferPattern[currentPosition]) &&
								IsCubyOn(currentPattern, UP, currentPattern[rotationPositions[rotationMap[index], i + j]])
							) ||
							(
								IsCubyOn(bufferPattern, DOWN, bufferPattern[currentPosition]) &&
								IsCubyOn(currentPattern, DOWN, currentPattern[rotationPositions[rotationMap[index], i + j]])
							)
						)
						{
							RotateCorner(bufferPattern, bufferPattern[currentPosition], COUNTERCLOCKWISE);
						}
						else
						{
							RotateCorner(bufferPattern, bufferPattern[currentPosition], CLOCKWISE);
						}
					}
					
				}

				currentPosition += 1;
			}
		}







		//If we want to rotate right, the array is the reverse of what expected to
		//left. So, we just have to reverse the order in the buffer.
		if (direction * directionMap[index] == -1.0f)
			Array.Reverse(bufferPattern);


		







		for (int i = 0; i < 9; i++)
        {
			currentPattern[i] = bufferPattern[i];
			Debug.Log(currentPattern[i]);
		}

		
		*/

	}











	protected static void Print(string[] pattern)
    {
		string message = "[";
		for (int i = 0; i < pattern.Length; i++)
		{
			message += pattern[i];
			if (i + 1 != pattern.Length)
				message += ", ";
		}

		message += "]";

		Debug.Log(message);
    }










}
