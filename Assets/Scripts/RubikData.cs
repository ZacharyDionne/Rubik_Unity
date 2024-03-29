using System.Collections.Generic;
using UnityEngine;

public class RubikData
{
    public static string[] DEFAULT_PATTERN = {
		"WBR",	
		"WB",	
		"WOB",	
		"RB",	
		"B",	
		"OB",	
		"YRB",	
		"YB",	
		"YBO",	
		"WR",	
		"W",	
		"WO",	
		"R",	
		"none", 
		"O",	
		"YR",	
		"Y",	
		"YO",	
		"WRG",	
		"WG",	
		"WGO",	
		"RG",	
		"G",	
		"OG",	
		"YGR",	
		"YG",	
		"YOG"	
	};



	public static readonly int[] UP = { 0, 1, 2, 9, 10, 11, 18, 19, 20};
	public static readonly int[] DOWN = { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
	public static readonly int[] FRONT = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
	public static readonly int[] BACK = { 18, 19, 20, 21, 22, 23, 24, 25, 26 };
	public static readonly int[] LEFT = { 0, 9, 18, 3, 12, 21, 6, 15, 24};
	public static readonly int[] RIGHT = { 2, 11, 20, 5, 14, 23, 8, 17, 26 };

	public static readonly int[] CORNERS = { 0, 2, 6, 8, 18, 20, 24, 26 };
	public static readonly int[] EDGES = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
	public static readonly int[] MIDDLES = { 4, 10, 12, 14, 16, 22 };

	public static int[][] indexFaceMap = new int[][] {
		FRONT,
		new int[]{},
		BACK,
		UP,
		new int[]{},
		DOWN,
		LEFT,
		new int[]{},
		RIGHT
	};

	//si longueur == 3: l'ordre est UP/DOWN, LEFT/RIGHT et FRONT/BACK
	//si longueur == 2: l'ordre est ((UP ou DOWN)/(LEFT ou RIGHT)) ou ((LEFT ou RIGHT)/(FRONT ou BACK))
	public static readonly int[][][] TOUCHING_FACES = {
		new int[][]{ UP, LEFT, FRONT },
		new int[][]{ UP, FRONT },
		new int[][]{ UP, RIGHT, FRONT },
		new int[][]{ LEFT, FRONT },
		new int[][]{ FRONT },
		new int[][]{ RIGHT, FRONT },
		new int[][]{ DOWN, LEFT, FRONT },
		new int[][]{ DOWN, FRONT },
		new int[][]{ DOWN, RIGHT, FRONT },
		new int[][]{ UP, LEFT },
		new int[][]{ UP },
		new int[][]{ UP, RIGHT },
		new int[][]{ LEFT },
		new int[][]{},
		new int[][]{ RIGHT },
		new int[][]{ DOWN, LEFT },
		new int[][]{ DOWN },
		new int[][]{ DOWN, RIGHT },
		new int[][]{ UP, LEFT, BACK },
		new int[][]{ UP, BACK },
		new int[][]{ UP, RIGHT, BACK },
		new int[][]{ LEFT, BACK },
		new int[][]{ BACK },
		new int[][]{ RIGHT, BACK },
		new int[][]{ DOWN, LEFT, BACK },
		new int[][]{ DOWN, BACK },
		new int[][]{ DOWN, RIGHT, BACK }
	};

	public static Dictionary<int[], int[]> opposedFace = new Dictionary<int[], int[]>()
	{
		{ UP , DOWN },
		{ DOWN , UP },
		{ LEFT , RIGHT },
		{ RIGHT , LEFT },
		{ FRONT , BACK },
		{ BACK , FRONT }
	};

	public static Vector3[] worldPositions = {
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


	//meme protocole que celui de la couleur des cubies
	public static readonly int[][][] colorFaceMap = new int[][][]
	{
		new int[][]{ UP, FRONT, LEFT },
		new int[][]{ UP, FRONT },
		new int[][]{ UP, RIGHT, FRONT },
		new int[][]{ LEFT, FRONT },
		new int[][]{ FRONT },
		new int[][]{ RIGHT, FRONT },
		new int[][]{ DOWN, LEFT, FRONT },
		new int[][]{ DOWN, FRONT },
		new int[][]{ DOWN ,FRONT, RIGHT },
		new int[][]{ UP, LEFT },
		new int[][]{ UP },
		new int[][]{ UP, RIGHT },
		new int[][]{ LEFT },
		new int[][]{ },
		new int[][]{ RIGHT },
		new int[][]{ DOWN, LEFT },
		new int[][]{ DOWN },
		new int[][]{ DOWN, RIGHT },
		new int[][]{ UP, LEFT, BACK },
		new int[][]{ UP, BACK },
		new int[][]{ UP, BACK, RIGHT },
		new int[][]{ LEFT, BACK },
		new int[][]{ BACK },
		new int[][]{ RIGHT, BACK },
		new int[][]{ DOWN, BACK, LEFT },
		new int[][]{ DOWN, BACK },
		new int[][]{ DOWN, RIGHT, BACK },
	};

	public static readonly byte CLOCKWISE = 1;
	public static readonly byte COUNTERCLOCKWISE = 2;

	/*
	public static Dictionary<int, Vector3> middlesFrame = new Dictionary<int, Vector3>()
	{
		{4, new Vector3(0.0f, 0.0f, 0.0f)},
		{10, new Vector3(90.0f, 0.0f, 0.0f)},
		{12, new Vector3(0.0f, 0.0f, -90.0f)},
		{14, new Vector3(0.0f, 0.0f, 90.0f)},
		{16, new Vector3(-90.0f, 0.0f, 0.0f)},
		{22, new Vector3(180.0f, 0.0f, 0.0f)}
	};
	*/

	public static Dictionary<int, Dictionary<int, Vector3>> middlesFrame = new Dictionary<int, Dictionary<int, Vector3>>()
	{
		{4, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(-90.0f, 0.0f, 0.0f)},
				{10, new Vector3(180.0f, 0.0f, 0.0f)},
				{12, new Vector3(0.0f, 0.0f, 90.0f)},
				{14, new Vector3(0.0f, 0.0f, -90.0f)},
				{16, new Vector3(0.0f, 0.0f, 0.0f)},
				{22, new Vector3(90.0f, 0.0f, 0.0f)}
			}
		},
		{10, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(0.0f, 0.0f, 0.0f)},
				{10, new Vector3(-90.0f, 0.0f, 0.0f)},
				{12, new Vector3(0.0f, -90.0f, 0.0f)},
				{14, new Vector3(0.0f, 90.0f, 0.0f)},
				{16, new Vector3(90.0f, 0.0f, 0.0f)},
				{22, new Vector3(0.0f, 180.0f, 0.0f)}
			}
		},
		{12, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(0.0f, -90.0f, 0.0f)},
				{10, new Vector3(0.0f, 0.0f, 90.0f)},
				{12, new Vector3(0.0f, 0.0f, 0.0f)},
				{14, new Vector3(0.0f, 180.0f, 0.0f)},
				{16, new Vector3(0.0f, 0.0f, -90.0f)},
				{22, new Vector3(0.0f, 90.0f, 0.0f)}
			}
		},
		{14, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(0.0f, 90.0f, 0.0f)},
				{10, new Vector3(0.0f, 0.0f, -90.0f)},
				{12, new Vector3(0.0f, 180.0f, 0.0f)},
				{14, new Vector3(0.0f, 0.0f, 0.0f)},
				{16, new Vector3(0.0f, 0.0f, 90.0f)},
				{22, new Vector3(0.0f, -90.0f, 0.0f)}
			}
		},
		{16, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(0.0f, 180.0f, 0.0f)},
				{10, new Vector3(90.0f, 0.0f, 0.0f)},
				{12, new Vector3(0.0f, -90.0f, 0.0f)},
				{14, new Vector3(0.0f, 90.0f, 0.0f)},
				{16, new Vector3(-90.0f, 0.0f, 0.0f)},
				{22, new Vector3(0.0f, 0.0f, 0.0f)}
			}
		},
		{22, new Dictionary<int, Vector3>()
			{
				{4, new Vector3(90.0f, 0.0f, 0.0f)},
				{10, new Vector3(0.0f, 0.0f, 0.0f)},
				{12, new Vector3(0.0f, 0.0f, -90.0f)},
				{14, new Vector3(0.0f, 0.0f, 90.0f)},
				{16, new Vector3(0.0f, 0.0f, 180.0f)},
				{22, new Vector3(-90.0f, 0.0f, 0.0f)}
			}
		}
	};

	public static Dictionary<int, Vector3> edgesFrame = new Dictionary<int, Vector3>()
	{
		{1, new Vector3(0.0f, 0.0f, 0.0f)},
		{3, new Vector3(0.0f, -90.0f, 0.0f)},
		{5, new Vector3(0.0f, 90.0f, 0.0f)},
		{7, new Vector3(0.0f, 180.0f, 0.0f)},
		{9, new Vector3(0.0f, 0.0f, -90.0f)},
		{11, new Vector3(0.0f, 0.0f, 90.0f)},
		{15, new Vector3(0.0f, 180.0f, -90.0f)},
		{17, new Vector3(0.0f, 180.0f, 90.0f)},
		{19, new Vector3(0.0f, 0.0f, 180.0f)},
		{21, new Vector3(180.0f, -90.0f, 0.0f)},
		{23, new Vector3(180.0f, 90.0f, 0.0f)},
		{25, new Vector3(0.0f, 180.0f, 180.0f)}
	};


	public static int[,] rotationPositions = {
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


	public static Dictionary<int, Vector3> edgeRotations = new Dictionary<int, Vector3>()
	{
		{1, new Vector3(-90.0f, 180.0f, 0.0f)},
		{3, new Vector3(0.0f, 90.0f, 180.0f)},
		{5, new Vector3(0.0f, -90.0f, 180.0f)},
		{7, new Vector3(90.0f, 180.0f, 0.0f)},
		{9, new Vector3(0.0f, 180.0f, 90.0f)},
		{11, new Vector3(0.0f, 180.0f, -90.0f)},
		{15, new Vector3(0.0f, 180.0f, -90.0f)},
		{17, new Vector3(0.0f, 180.0f, 90.0f)},
		{19, new Vector3(90.0f, 180.0f, 0.0f)},
		{21, new Vector3(0.0f, -90.0f, 180.0f)},
		{23, new Vector3(0.0f, 90.0f, 180.0f)},
		{25, new Vector3(-90.0f, 0.0f, 180.0f)}
	};

	public static Dictionary<int, Vector3> cornerRotations = new Dictionary<int, Vector3>()
	{
		{0, new Vector3(0.0f, 90.0f, 90.0f)},
		{2, new Vector3(-90.0f, 0.0f, 90.0f)},
		{6, new Vector3(90.0f, 0.0f, 90.0f)},
		{8, new Vector3(0.0f, -90.0f, 90.0f)},
		{18, new Vector3(90.0f, 0.0f, -90.0f)},
		{20, new Vector3(0.0f, 90.0f, -90.0f)},
		{24, new Vector3(0.0f, -90.0f, -90.0f)},
		{26, new Vector3(-90.0f, 0.0f, -90.0f)}
	};



	public static Dictionary<int, Dictionary<int, Vector3>> cornerCorrections = new Dictionary<int, Dictionary<int, Vector3>>()
	{
		{
			0, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(0.0f, 0.0f, 0.0f)},
				{2, new Vector3(0.0f, 0.0f, 90.0f)},
				{6, new Vector3(180.0f, 0.0f, 90.0f)},
				{8, new Vector3(180.0f, 0.0f, 180.0f)},
				{18, new Vector3(0.0f, 0.0f, -90.0f)},
				{20, new Vector3(0.0f, 0.0f, 180.0f)},
				{24, new Vector3(180.0f, 0.0f, 0.0f)},
				{26, new Vector3(180.0f, 0.0f, -90.0f)}
			}
		},
		{
			2, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(0.0f, 0.0f, -90.0f)},
				{2, new Vector3(0.0f, 0.0f, 0.0f)},
				{6, new Vector3(180.0f, 0.0f, 180.0f)},
				{8, new Vector3(180.0f, 0.0f, -90.0f)},
				{18, new Vector3(0.0f, 0.0f, 180.0f)},
				{20, new Vector3(0.0f, 0.0f, 90.0f)},
				{24, new Vector3(180.0f, 0.0f, 90.0f)},
				{26, new Vector3(180.0f, 0.0f, 0.0f)}
			}
		},
		{
			6, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(180.0f, 0.0f, 90.0f)},
				{2, new Vector3(180.0f, 0.0f, 180.0f)},
				{6, new Vector3(0.0f, 0.0f, 0.0f)},
				{8, new Vector3(0.0f, 0.0f, 90.0f)},
				{18, new Vector3(180.0f, 0.0f, 0.0f)},
				{20, new Vector3(180.0f, 0.0f, -90.0f)},
				{24, new Vector3(0.0f, 0.0f, -90.0f)},
				{26, new Vector3(0.0f, 0.0f, 180.0f)}
			}
		},
		{
			8, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(180.0f, 0.0f, 180.0f)},
				{2, new Vector3(180.0f, 0.0f, -90.0f)},
				{6, new Vector3(0.0f, 0.0f, -90.0f)},
				{8, new Vector3(0.0f, 0.0f, 0.0f)},
				{18, new Vector3(180.0f, 0.0f, 90.0f)},
				{20, new Vector3(180.0f, 0.0f, 0.0f)},
				{24, new Vector3(0.0f, 0.0f, 180.0f)},
				{26, new Vector3(0.0f, 0.0f, 90.0f)}
			}
		},
		{
			18, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(0.0f, 0.0f, 90.0f)},
				{2, new Vector3(0.0f, 0.0f, 180.0f)},
				{6, new Vector3(180.0f, 0.0f, 0.0f)},
				{8, new Vector3(180.0f, 0.0f, 90.0f)},
				{18, new Vector3(0.0f, 0.0f, 0.0f)},
				{20, new Vector3(0.0f, 0.0f, 90.0f)},
				{24, new Vector3(180.0f, 0.0f, -90.0f)},
				{26, new Vector3(180.0f, 0.0f, 180.0f)}
			}
		},
		{
			20, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(0.0f, 0.0f, 180.0f)},
				{2, new Vector3(0.0f, 0.0f, -90.0f)},
				{6, new Vector3(180.0f, 0.0f, -90.0f)},
				{8, new Vector3(180.0f, 0.0f, 0.0f)},
				{18, new Vector3(0.0f, 0.0f, 90.0f)},
				{20, new Vector3(0.0f, 0.0f, 0.0f)},
				{24, new Vector3(180.0f, 0.0f, 180.0f)},
				{26, new Vector3(180.0f, 0.0f, 90.0f)}
			}
		},
		{
			24, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(180.0f, 0.0f, 0.0f)},
				{2, new Vector3(180.0f, 0.0f, 90.0f)},
				{6, new Vector3(0.0f, 0.0f, 90.0f)},
				{8, new Vector3(0.0f, 0.0f, 180.0f)},
				{18, new Vector3(180.0f, 0.0f, -90.0f)},
				{20, new Vector3(180.0f, 0.0f, 180.0f)},
				{24, new Vector3(0.0f, 0.0f, 0.0f)},
				{26, new Vector3(0.0f, 0.0f, -90.0f)}
			}
		},
		{
			26, new Dictionary<int, Vector3>()
			{
				{0, new Vector3(180.0f, 0.0f, -90.0f)},
				{2, new Vector3(180.0f, 0.0f, 0.0f)},
				{6, new Vector3(0.0f, 0.0f, 180.0f)},
				{8, new Vector3(0.0f, 0.0f, -90.0f)},
				{18, new Vector3(180.0f, 0.0f, 180.0f)},
				{20, new Vector3(180.0f, 0.0f, 90.0f)},
				{24, new Vector3(0.0f, 0.0f, 90.0f)},
				{26, new Vector3(0.0f, 0.0f, 0.0f)}
			}
		}
	};



	public static readonly Dictionary<int, float> DIRECTION_CORRECTION = new Dictionary<int, float>()
	{
		{ 0, -1.0f },
		{ 2, 1.0f },
		{ 3, -1.0f },
		{ 5, 1.0f },
		{ 6, 1.0f },
		{ 8, -1.0f }
	};

	public static readonly Dictionary<int, int[]> LURULURU_MAP = new Dictionary<int, int[]>()
	{
		{ 6, new int[]{ 8, 6 } },
		{ 8, new int[]{ 2, 0 } },
		{ 24, new int[]{ 0, 2 } },
		{ 26, new int[]{ 6, 8 } }
	};

	public static readonly Dictionary<int, int[]> RULURULU_MAP = new Dictionary<int, int[]>()
	{
		{ 6, new int[]{ 0, 2 } },
		{ 8, new int[]{ 8, 6 } },
		{ 24, new int[]{ 6, 8 } },
		{ 26, new int[]{ 2, 0 } }
	};

}
