using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;




public delegate bool RotationCondition();

public class RubiksCube : MonoBehaviour
{


	//-----------------------------------------------------PUBLIC STATIC----------------------------------------------------------------//
	public static RubiksCube GenerateCube(string[] pattern)
	{
		RubiksCube cube = Instantiate(Resources.Load<GameObject>("Rubik's Cube")).GetComponent<RubiksCube>();
		cube.currentPattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();

		for (int i = 0; i < RubikData.DEFAULT_PATTERN.Length; i++)
        	{
			Transform cuby = cube.transform.Find(RubikData.DEFAULT_PATTERN[i]);
			//cuby.position = RubikData.worldPositions[i];

			cube.cubyPositions[i] = cuby;

			/*
			int defaultPosition = FindCuby(RubikData.DEFAULT_PATTERN, pattern[i]);

			switch (pattern[i].Length)
            		{
				case 1:
					cuby.localRotation = Quaternion.Euler(RubikData.middlesFrame[defaultPosition][i]);
						
					break;

				case 2:
					
					cuby.Rotate(RubikData.edgesFrame[defaultPosition] - RubikData.edgesFrame[i]);
					cube.PhysicRotateEdge(pattern[i], cuby);
			
					if (
						defaultPosition == 9 && i == 15 ||
						defaultPosition == 15 && i == 9 ||
						defaultPosition == 11 && i == 17 ||
						defaultPosition == 17 && i == 11)
					cuby.localRotation *= Quaternion.Euler(180.0f, 180.0f, 0.0f);

					
					break;

				case 3:
					cube.CorrectCornerOrientation(cuby);
					cube.PhysicRotateCorner(pattern[i], cuby);

					break;
            		}
			*/


        	}

		List<Order> list = cube.StartSolve((string[]) RubikData.DEFAULT_PATTERN.Clone(), pattern);

		if (list == null)
        {
			Destroy(cube.gameObject);
			return null;
        }

		list.Reverse();

		foreach (RotateOrder order in list)
        {
			cube.Index = order.Index;
			cube.direction = order.Direction;
			cube.OrderList.Insert(0, order);
        }

		return cube;
    	}











	//-----------------------------------------PROTECTED STATIC-----------------------------------//
	protected static int FindCuby(string[] pattern, string name)
	{
		string cuby = Array.Find<string>(pattern, (string currentName) =>
		{


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






	protected static string[] GetCubiesOn(string[] pattern, int[] face)
    	{
		string[] cubies = new string[9];

		for (int i = 0; i < face.Length; i++)
        	{
			cubies[i] = pattern[i];
        	}

		return cubies;
    	}



	/*
		Effectue une rotation dans le nom d'un cuby.
	*/
	protected static void RotateCorner(Transform cuby, byte orientation)
	{
		if (orientation == RubikData.CLOCKWISE)
			cuby.name = cuby.name[2].ToString() + cuby.name[0].ToString() + cuby.name[1].ToString();
		else
			cuby.name = cuby.name[1].ToString() + cuby.name[2].ToString() + cuby.name[0].ToString();
	}

	public static string RotateCorner(string cuby, byte orientation)
	{
		if (orientation == RubikData.CLOCKWISE)
			return cuby[2].ToString() + cuby[0].ToString() + cuby[1].ToString();

		return cuby[1].ToString() + cuby[2].ToString() + cuby[0].ToString();
	}


	protected static void RotateEdge(Transform cuby)
	{
		cuby.name = cuby.name[1].ToString() + cuby.name[0].ToString();
	}

	public static string RotateEdge(string cuby)
	{
		return cuby[1].ToString() + cuby[0].ToString();
	}




	public static bool IsCubyOn(int[] face, int position)
    	{
		for (int i = 0; i < face.Length; i++)
		{
			if (position == face[i])
				return true;
		}

		return false;
	}

	public static bool IsCorner(string name)
	{
		return name.Length == 3;
	}
	
	public static bool IsEdge(string name)
	{
		return name.Length == 2;
	}

	public static bool IsMiddle(string name)
	{
		return name.Length == 1;
	}

	protected int[] GetCubyFaceFromColor(string cuby, int position, char color)
    {
		return RubikData.colorFaceMap[position][cuby.IndexOf(color)];
    }

	protected string GetCubyColorFromFace(string cuby, int position, int[] face)
    {
		return cuby[Array.IndexOf(RubikData.colorFaceMap[position], face)].ToString();
    }

	protected int[] GetFaceFromColor(string[] pattern, string color)
    {
		if (pattern[RubikData.FRONT[4]].Equals(color))
			return RubikData.FRONT;
		if (pattern[RubikData.BACK[4]].Equals(color))
			return RubikData.BACK;
		if (pattern[RubikData.LEFT[4]].Equals(color))
			return RubikData.LEFT;
		if (pattern[RubikData.RIGHT[4]].Equals(color))
			return RubikData.RIGHT;
		if (pattern[RubikData.UP[4]].Equals(color))
			return RubikData.UP;
		if (pattern[RubikData.DOWN[4]].Equals(color))
			return RubikData.DOWN;
		else
			throw new Exception("Face introuvable avec la couleur \"" + color + "\"");
	}





	
	//Permet de garder l'etat interne du cube sans effectuer de reelles rotations
	protected static void UpdatePositions(string[] pattern, int index, float direction)
    	{
		string[] bufferPositions = new string[9];

		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPositions[currentPosition] = pattern[RubikData.rotationPositions[index, i + j]];

				currentPosition++;
			}
		}
		
		if (direction == -1.0f)
			Array.Reverse(bufferPositions);

		for (int i = 0; i < 9; i++)
        	{
			string cuby = bufferPositions[i];
			int oldPosition = Array.IndexOf(pattern, cuby);
			int newPosition = Array.IndexOf(bufferPositions, cuby);


			if (IsCorner(cuby))
            		{
				if (index <= 2 || index >= 6)
				{
					bool a = index == 0 || index == 8;
					bool b = direction == 1.0f;
					bool c = IsCubyOn(RubikData.UP, oldPosition);
					bool d = IsCubyOn(RubikData.UP, newPosition);

					cuby = RotateCorner(bufferPositions[i], a != b != c != d ? RubikData.CLOCKWISE: RubikData.COUNTERCLOCKWISE);
				}
			}
			else if (IsEdge(cuby))
            		{
				if (index == 1 || index == 4 || index >= 6)
                		{
					cuby = RotateEdge(bufferPositions[i]);
                		}
            		}
			bufferPositions[i] = cuby;
		}
		

		for (int i = 0; i < bufferPositions.Length; i++)
			pattern[RubikData.rotationPositions[index, i]] = bufferPositions[i];
	}


	public static void Rotate(string[] pattern, List<Order> orderList, int index, float direction)
    {
		UpdatePositions(pattern, index, direction);
		orderList.Add(new RotateOrder(index, direction));
	}

	protected static void RotateUntil(string[] pattern, List<Order> orderList, int index, float direction, RotationCondition condition)
    {
		int counter = 0;
		while (!condition())
		{
			Rotate(pattern, orderList, index, direction);

			if (++counter > 3)
				throw new Exception("counter greater than 3, Side");
		}
		if (counter == 3)
		{
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.Add(new RotateOrder(index, -direction));
		}
	}

	protected static float RotateWith(string[] pattern, List<Order> orderList, int index, float direction, RotationCondition condition)
    {
		Rotate(pattern, orderList, index, 1.0f);

		if (!condition())
		{
			direction = -direction;
			UpdatePositions(pattern, index, direction);
			UpdatePositions(pattern, index, direction);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.Add(new RotateOrder(index, direction));
		}

		return direction;
	}








	//----------------PROTECTED ATTRIBUTS---------------------------------//
	
	//variable de controle
	protected bool isRotating;

	//variables pour l'animation de rotation
	protected const float ROTATION_SPEED = 10.0f * (90.0f / 60.0f);
	protected float degreeCounter;


	// -1.0f ou 1.0f, definisse dans quelle direction tourner
	protected float direction;


	//Variable utilisee pour melanger le cube
	protected int randomCounter;
	protected System.Random random;


	//Stocke l'information avant de melanger
	protected int lastIndex;
	protected float lastDirection;

	//Queue des actions que le cube doit faire
	public List<Order> OrderList { get; private set; }
	protected Order currentOrder;
	


	protected Vector3 currentAxis;


	//variables pour garder a l'interne l'etat du cube
	protected Transform[] cubyPositions;
	protected Transform[] bufferPositions;



	protected string[] currentPattern;
	


	//Entre 0 et 8, indique quel partie du cube est selectionnee pour tourner
	protected int index;

	public int Index {
		get => index;
		set {
			if (value < 0)
				return;
			if (value > 8)
				return;
			index = value;
		}
	}










	//----------------------PUBLIC METHODES------------------------------------------//

	public void Rotate(int index, float direction)
	{
		OrderList.Add(new RotateOrder(index, direction));
	}


    public void Solve(string[] objectivePattern)
    {
		OrderList.Add(new SolveOrder(objectivePattern));
	}

	
	public void GetJSON(Action<string> callback)
    {
		OrderList.Add(new GetJSONOrder(callback));
    }
	


	public void Randomize()
    {
		OrderList.Add(new RandomizeOrder());
	}










	//-------------PRIVATE METHODES-----------------------------------------------------//
	void FixedUpdate()
    	{
		if (isRotating)
			RotateAnimation();
		else if (currentOrder is RotateOrder)
		{
			currentOrder = null;
		}
		else if (randomCounter > 0)
			NextRandom();
		else if (randomCounter == 0)
		{
			direction = lastDirection;
			Index = lastIndex;
			randomCounter = -1;


			if (currentOrder is RandomizeOrder)
			{
				currentOrder = null;
			}


		}

		NextOrder();
	}


    void Awake()
    {
		isRotating = false;
		Index = 0;
		randomCounter = -1;

		bufferPositions = new Transform[9];
		random = new System.Random();
		cubyPositions = new Transform[27];
		OrderList = new List<Order>();
	}







	//---------PROTECTED METHODES-------------------------------------//


	protected void NextOrder()
	{
		if (currentOrder != null || OrderList.Count == 0)
			return;

		currentOrder = OrderList[0];
		OrderList.RemoveAt(0);

		if (currentOrder is RandomizeOrder)
		{
			StartRandomize();
		}
		else if (currentOrder is RotateOrder)
		{
			RotateOrder rotateOrder = (RotateOrder) currentOrder;

			Index = rotateOrder.Index;
			direction = rotateOrder.Direction;

			RotateBegin();
		}
		else if (currentOrder is SolveOrder)
		{
			SolveOrder solveOrder = (SolveOrder) currentOrder;

			string[] pattern = (string[])currentPattern.Clone();
			List<Order> result = StartSolve(pattern, solveOrder.Pattern);

			if (result == null)
			{
				Debug.Log("Introuvable");
				goto END;
			}

			result.Reverse();

			foreach (Order order in result)
			{
				OrderList.Insert(0, order);
			}

		END:
			currentOrder = null;



		}
		else if (currentOrder is GetJSONOrder)
        {
			GetJSONOrder getJSONOrder = (GetJSONOrder) currentOrder;

			string json = "[";

			for (int i = 0; i < currentPattern.Length; i++)
			{
				json += "\"" + currentPattern[i] + "\"";
				if (i + 1 != currentPattern.Length)
				{
					json += ",";
				}
			}

			json += "]";

			getJSONOrder.Callback(json);
			
			currentOrder = null;
		}
	}






	protected void StartRandomize()
	{
		randomCounter = 20;
		lastDirection = direction;
		lastIndex = Index;

		NextRandom();
	}

	protected List<Order> StartSolve(string[] pattern, string[] objectivePattern)
	{
		List<Order> orderList = new List<Order>();

		//Positionnement des milieux
		if (
			IsCubyOn(RubikData.FRONT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.BACK, FindCuby(pattern, objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.DOWN, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			RotateUntil(pattern, orderList, 7, 1.0f, () => {
				return pattern[RubikData.UP[4]] == objectivePattern[RubikData.UP[4]];
			});
		}
		else if (IsCubyOn(RubikData.LEFT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			Rotate(pattern, orderList, 1, -1.0f);
		}
		else if (IsCubyOn(RubikData.RIGHT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			Rotate(pattern, orderList, 1, 1.0f);
		}
		RotateUntil(pattern, orderList, 4, 1.0f, () => {
			return pattern[RubikData.FRONT[4]].Equals(objectivePattern[RubikData.FRONT[4]]);
		});



		foreach (int position in new int[] { RubikData.FRONT[4], RubikData.UP[4], RubikData.BACK[4], RubikData.DOWN[4], RubikData.LEFT[4], RubikData.RIGHT[4] })
        {
			if (!pattern[position].Equals(objectivePattern[position]))
            {
				Debug.Log("middles not ok");
				return null;
            }
        }





		//Faire la croix du dessus
		foreach (int position in new int[] { 1, 9, 11, 19 })
		{
			string cubyRef = pattern[position];
			string wantedCuby = objectivePattern[position];

			if (pattern[position].Equals(wantedCuby))
				continue;
			
			//Si sur UP, descendre
			if (IsCubyOn(RubikData.UP, FindCuby(pattern, wantedCuby)))
			{
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]);
				int refPosition = FindCuby(pattern, cubyRef);
				Rotate(pattern, orderList, index, -1.0f);
				cubyRef = pattern[refPosition];
			}//Si sur la face du bas, il faut aligner les references, monter et puis mettre a jour la reference
			else if (IsCubyOn(RubikData.DOWN, FindCuby(pattern, wantedCuby)))
			{
				RotateUntil(pattern, orderList, 3, 1.0f, () => {
					return RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1] == RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1];
				});
				
				
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]);

				Rotate(pattern, orderList, index, 1.0f);

				cubyRef = pattern[RubikData.rotationPositions[index, 1]];
			}

			//on aligne le haut
			RotateUntil(pattern, orderList, 3, 1.0f, () => {
				return (//---------------------bug recemment corrige ici---------------------------//
					pattern[FindCuby(pattern, wantedCuby)][0].ToString().Equals(objectivePattern[position][0].ToString()) ?
						RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1] :
						RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][0])
					.Equals(
					RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1]);
			});


			//go up until is on UP
			RotateUntil(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1]), 1.0f, () => {
				return IsCubyOn(RubikData.UP, FindCuby(pattern, wantedCuby));
			});


			//replacer le UP pour le prochain tour
			RotateUntil(pattern, orderList, 3, 1.0f, () => {
				return
					pattern[position]
					.Equals(
						objectivePattern[position]
					);
			});

		}
		


		

		//Faire la face du dessus
		foreach (int position in new int[]{ 0, 2, 18, 20 })
		{
			string wantedCuby = objectivePattern[position];

			if (pattern[position].Equals(wantedCuby))
				continue;

			

			//Si sur le dessus
			if (IsCubyOn(RubikData.UP, FindCuby(pattern, wantedCuby)))
			{
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]);
				float firstDirection = 1.0f;

				//descendre
				firstDirection = RotateWith(pattern, orderList, index, firstDirection, () => {
					return IsCubyOn(RubikData.DOWN, FindCuby(pattern, wantedCuby));
				});


				//on deplace le bas
				direction = RotateWith(pattern, orderList, 5, 1.0f, () => {
					return !(
						Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][0]) == index ||
						Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]) == index);
				});

				//remonter la face
				Rotate(pattern, orderList, index, -firstDirection);
			}
			
			/*---------------------------------------------correction recente ici----------------------------------------------*/
			//Si la couleur up du cuby est sur le dessous
			if (pattern[FindCuby(pattern, wantedCuby)][0].ToString().Equals(wantedCuby[0].ToString()))
			{
				//d'abord, on l'aligne avec le dessus
				RotateUntil(pattern, orderList, 5, 1.0f, () => {
					return RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1] == RubikData.TOUCHING_FACES[position][1] &&
						RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][2] == RubikData.TOUCHING_FACES[position][2];
				});



				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]);
				float oldDirection = 1.0f;
				int[][] oldFaces = RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)];

				//on descend
				oldDirection = RotateWith(pattern, orderList, index, oldDirection, () => {
					return !IsCubyOn(RubikData.UP, FindCuby(pattern, wantedCuby));
				});

				//On deplace le bas
				RotateWith(pattern, orderList, 5, 1.0f, () =>
				{
					return !IsCubyOn(oldFaces[1], FindCuby(pattern, wantedCuby));
				});

				//on remonte la face
				Rotate(pattern, orderList, index, -oldDirection);
			}

			//on realigne le cuby en vu de le monter
			RotateUntil(pattern, orderList, 5, 1.0f, () => {
				return RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1] == RubikData.TOUCHING_FACES[position][1] &&
					RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][2] == RubikData.TOUCHING_FACES[position][2];
			});

			//------------------------------BUG RECEMMENT CORRIGE ICI-----------------------------------------------------------------------//
			/*index = Array.IndexOf(
				RubikData.indexFaceMap,
				GetCubyFaceFromColor(
					pattern[FindCuby(pattern, wantedCuby)],
					FindCuby(pattern, wantedCuby),
					objectivePattern[RubikData.UP[4]][0]));*/
			index = Array.IndexOf(
				RubikData.indexFaceMap,
				GetCubyFaceFromColor(
					pattern[FindCuby(pattern, wantedCuby)],
					FindCuby(pattern, wantedCuby),
					objectivePattern[position][0]));
			int oldPosition = FindCuby(pattern, wantedCuby);
			direction = 1.0f;

			//on descend
			direction = RotateWith(pattern, orderList, index, direction, () => {
				return !IsCubyOn(RubikData.UP, FindCuby(pattern, wantedCuby));
			});

			//on tourne le bas jusqu'a sa bonne position
			RotateUntil(pattern, orderList, 5, 1.0f, () => {
				return FindCuby(pattern, objectivePattern[position]) == oldPosition;
			});


			//on remonte la face
			Rotate(pattern, orderList, index, -direction);
		}



		foreach (int i in RubikData.UP)
		{
			if (!pattern[i].Equals(objectivePattern[i]))
			{
				Debug.Log("----------nop for up");
				return null;
			}
		}
		
		
		//deuxieme etage
		foreach (int position in new int[]{ 3, 5, 21, 23})
		{
			string wantedCuby = objectivePattern[position];
			int[] face1, face2;
			float directionSide1, directionSide2, directionDown;
			string cubyRef;

			if (pattern[position].Equals(wantedCuby))
				continue;

			//Si cuby n'est pas sur la face du bas
			if (!IsCubyOn(RubikData.DOWN, FindCuby(pattern, wantedCuby)))
			{
				face1 = RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][0];
				face2 = RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1];
				directionSide1 = 1.0f;
				directionSide2 = 1.0f;
				directionDown = 1.0f;


				//descendre
				directionSide1 = RotateWith(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face1), directionSide1, () => {
					return IsCubyOn(RubikData.DOWN, FindCuby(pattern, wantedCuby));
				});

				//tourner le bas
				directionDown = RotateWith(pattern, orderList, 5, directionDown, () => {
					return RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1] == face2;
				});

				//remonte
				Rotate(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face1), -directionSide1);

				//realigne le coin blanc
				Rotate(pattern, orderList, 5, -directionDown);

				



				

				cubyRef = Array.Find<string>(pattern, (str) => {
					int position = FindCuby(pattern, str);

					return IsCubyOn(face1, position) && IsCubyOn(face2, position) && IsCubyOn(RubikData.DOWN, position);
				});

				

				//descendre le deuxieme cote
				directionSide2 = RotateWith(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face2), directionSide2, () => {
					return IsCubyOn(RubikData.DOWN, FindCuby(pattern, cubyRef));
				});

				//tourner pour que la reference soit sur la face 1
				RotateWith(pattern, orderList, 5, 1.0f, () => {
					return IsCubyOn(face1, FindCuby(pattern, cubyRef)) && IsCubyOn(face2, FindCuby(pattern, cubyRef));
				});

				//remonter le deuxieme cote
				Rotate(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face2), -directionSide2);
            		}


			

			//va a la face oppose ayant la couleur du bas du cuby
			RotateUntil(pattern, orderList, 5, 1.0f, () => {
				return RubikData.TOUCHING_FACES[FindCuby(pattern, wantedCuby)][1]
					==
					RubikData.opposedFace[RubikData.TOUCHING_FACES[position][
						wantedCuby[0].ToString().Equals(pattern[FindCuby(pattern, wantedCuby)][0].ToString()) ?
							0:
							1
					]];
			});


			
			cubyRef = pattern[position];
			directionSide1 = 1.0f;
			directionSide2 = 1.0f;
			directionDown = 1.0f;
			face1 = RubikData.TOUCHING_FACES[position][
				wantedCuby[0].ToString().Equals(pattern[FindCuby(pattern, wantedCuby)][0].ToString()) ?
							0:
							1];
			face2 = RubikData.TOUCHING_FACES[position][
				wantedCuby[0].ToString().Equals(pattern[FindCuby(pattern, wantedCuby)][0].ToString()) ?
							1:
							0];



			//tourne la face de couleur oppose pour que le cubyRef (cuby ayant la place ou on veut aller) soit en bas
			directionSide1 = RotateWith(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face1), directionSide1, () => {
				return IsCubyOn(RubikData.DOWN, FindCuby(pattern, cubyRef));
			});



			//tourne le bas afin que le cubyRef soit sur la face oppose
			directionDown = RotateWith(pattern, orderList, 5, directionDown, () => {
				return IsCubyOn(face2, FindCuby(pattern, cubyRef));
			});

			//remonte
			Rotate(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face1), -directionSide1);

			//replace le bas
			Rotate(pattern, orderList, 5, -directionDown);




			cubyRef = Array.Find<string>(pattern, (str) => {
				int position = FindCuby(pattern, str);

				return IsCubyOn(face1, position) && IsCubyOn(face2, position) && IsCubyOn(RubikData.DOWN, position);
			});


			directionSide2 = RotateWith(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face2), directionSide2, () => {
				return IsCubyOn(RubikData.DOWN, FindCuby(pattern, cubyRef));
			});


			RotateWith(pattern, orderList, 5, 1.0f, () => {
				return IsCubyOn(face1, FindCuby(pattern, cubyRef)) && IsCubyOn(face2, FindCuby(pattern, cubyRef));
			});

			Rotate(pattern, orderList, Array.IndexOf(RubikData.indexFaceMap, face2), -directionSide2);
        	}


		foreach (int i in new int[]{3, 5, 21, 23})
		{
			if (!pattern[i].Equals(objectivePattern[i]))
			{
				Debug.Log("----------nop for middle");
				return null;
			}
		}



		//Faire la croix du dessous
		//obtenir le nombre de jaune bien oriente
		List<int> wellOriented = new List<int>();



		//Definition des fonctions locales. Ces fonctions seront utiles uniquement ici,
		//alors on les declare a l'interieur. Ils ont les noms qui est donnee a ces sequences
		//de mouvement. Voir le livrable.
		void Fururf (int f, int r) {

			Rotate(pattern, orderList, f, RubikData.DIRECTION_CORRECTION[f]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, f, -RubikData.DIRECTION_CORRECTION[f]);

		};
		void Fruruf(int f, int r) {

			Rotate(pattern, orderList, f, RubikData.DIRECTION_CORRECTION[f]);
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, f, -RubikData.DIRECTION_CORRECTION[f]);

		};


		foreach (int position in new int[]{ 7, 15, 17, 25 })
		{
			if (pattern[FindCuby(pattern, objectivePattern[position])][0].ToString().Equals(objectivePattern[position][0].ToString()))
				wellOriented.Add(FindCuby(pattern, objectivePattern[position]));
		}

		switch (wellOriented.Count)
		{
			case 0:

				Fururf(0, 6);
				Rotate(pattern, orderList, 5, -1.0f);
				Fruruf(0, 6);

				break;

			case 2:
				if (RubikData.opposedFace[RubikData.TOUCHING_FACES[wellOriented[0]][1]] == RubikData.TOUCHING_FACES[wellOriented[1]][1])
				{
					if (wellOriented[0] == 7 || wellOriented[0] == 25)
					{

						Fruruf(6, 2);
					}
					else
					{

						Fruruf(0, 6);
					}
				}
				else
				{
					if (
						(wellOriented[0] == 25 || wellOriented[1] == 25) &&
						(wellOriented[0] == 15 || wellOriented[1] == 15)
						)
                    {

						Fururf(8, 0);


					}
					else if (
						(wellOriented[0] == 15 || wellOriented[1] == 15) &&
						(wellOriented[0] == 7 || wellOriented[1] == 7)
						)
                    {

						Fururf(2, 8);
					}
					else if (
						(wellOriented[0] == 7 || wellOriented[1] == 7) &&
						(wellOriented[0] == 17 || wellOriented[1] == 17)
						)
					{

						Fururf(6, 2);
					}
					else
					{

						Fururf(0, 6);
					}
				}			

				break;
		}


		foreach (int i in new int[]{7, 15, 17, 25})
		{
			if (!pattern[FindCuby(pattern, objectivePattern[i])].Equals(objectivePattern[i]))
			{
				Debug.Log("----------nop for down cross");
				return null;
			}
		}



		//bien placer la croix
		List<int> wellPlaced = new List<int>();

		void Rururuur(int r)
        {
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
		}
		void Ruururur(int r)
		{
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
		}


		//tourner jusqu'a au moins 1 soit bien placer et oriente
		RotateUntil(pattern, orderList, 5, 1.0f, () => {
			foreach (int position in new int[] { 7, 15, 17, 25 })
			{
				if (pattern[position].Equals(objectivePattern[position]))
					return true;
			}
			

			return false;
		});

		TRY_AGAIN:
		foreach (int position in new int[] { 7, 15, 17, 25 })
		{
			if (pattern[position].Equals(objectivePattern[position]))
				wellPlaced.Add(position);
		}

		int r = -1;
		string[] backupPattern = (string[]) pattern.Clone();
		switch (wellPlaced.Count)
		{
			case 1:

				if (wellPlaced[0] == 7)
				{
					r = 6;
				}
				else if (wellPlaced[0] == 15)
				{
					r = 2;
				}
				else if (wellPlaced[0] == 17)
				{
					r = 0;
				}
				else
				{
					r = 8;
				}
				Rururuur(r);

				if (
					!pattern[7].Equals(objectivePattern[7]) ||
					!pattern[15].Equals(objectivePattern[15]) ||
					!pattern[17].Equals(objectivePattern[17]) ||
					!pattern[25].Equals(objectivePattern[25])
				)
				{
					for (int i = 8; i != 0; i--)
						orderList.RemoveAt(orderList.Count - 1);

					pattern = backupPattern;

					Ruururur(r);
				}
				
				break;


			case 2:

				if ((wellPlaced[0] == 7 && wellPlaced[1] == 25) || (wellPlaced[0] == 25 && wellPlaced[1] == 7))
				{
					r = 0;
				}
				else if ((wellPlaced[0] == 15 && wellPlaced[1] == 17) || (wellPlaced[0] == 17 && wellPlaced[1] == 15))
				{
					r = 8;
				}
				else
                {
					foreach (int position in new int[]{ 7, 15, 17, 25 })
                    {
						bool found = false;
						for (int i = 0; i < wellPlaced.Count; i++)
                        {
							if (position == wellPlaced[i])
								found = true;
                        }

						if (!found)
                        {
							string chosenCuby = pattern[position];

							RotateUntil(pattern, orderList, 5, 1.0f, () => {
								return FindCuby(pattern, chosenCuby) == FindCuby(objectivePattern, chosenCuby);
							});

							wellPlaced.Clear();

							goto TRY_AGAIN;
                        }

                    }

                }

				Rururuur(r);

				Rotate(pattern, orderList, 5, -1.0f);

				Rururuur(r);

				break;
		}


		foreach (int position in new int[] { 7, 15, 17, 25})
        {
			if (!pattern[position].Equals(objectivePattern[position]))
            {
				Debug.Log("down cross misplaced");
				return null;
            }
        }


		//bien placer les coins
		wellPlaced.Clear();
		backupPattern = (string[]) pattern.Clone();

		void Luruluru(int l, int r)
        {
			Rotate(pattern, orderList, l, -RubikData.DIRECTION_CORRECTION[l]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, l, RubikData.DIRECTION_CORRECTION[l]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
		}

		void Rulurulu(int l, int r)
        {
			Rotate(pattern, orderList, r, RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, l, -RubikData.DIRECTION_CORRECTION[l]);
			Rotate(pattern, orderList, 5, 1.0f);
			Rotate(pattern, orderList, r, -RubikData.DIRECTION_CORRECTION[r]);
			Rotate(pattern, orderList, 5, -1.0f);
			Rotate(pattern, orderList, l, RubikData.DIRECTION_CORRECTION[l]);
			Rotate(pattern, orderList, 5, 1.0f);
		}

		void GetWellPlaced()
        {
			foreach (int position in new int[] { 6, 8, 24, 26 })
			{
				if (FindCuby(pattern, objectivePattern[position]) == position)
					wellPlaced.Add(position);
			}
		}


		GetWellPlaced();
		if (wellPlaced.Count == 0)
        {
			Luruluru(8, 6);
			GetWellPlaced();

			if (wellPlaced.Count == 0)
            {
				for (int i = 8; i != 0; i--)
					orderList.RemoveAt(orderList.Count - 1);

				pattern = backupPattern;

				Rulurulu(0, 2);

				GetWellPlaced();
            }
        }




		


		if (
			FindCuby(pattern, objectivePattern[6]) != 6 ||
			FindCuby(pattern, objectivePattern[8]) != 8 ||
			FindCuby(pattern, objectivePattern[24]) != 24 ||
			FindCuby(pattern, objectivePattern[26]) != 26
			)
        {
			int workCorner = wellPlaced[0];
			backupPattern = (string[]) pattern.Clone();
			Luruluru(RubikData.LURULURU_MAP[workCorner][0], RubikData.LURULURU_MAP[workCorner][1]);
			if (
				FindCuby(pattern, objectivePattern[6]) != 6 ||
				FindCuby(pattern, objectivePattern[8]) != 8 ||
				FindCuby(pattern, objectivePattern[24]) != 24 ||
				FindCuby(pattern, objectivePattern[26]) != 26
				)
			{
				for (int i = 8; i != 0; i--)
					orderList.RemoveAt(orderList.Count - 1);
				pattern = backupPattern;
				Rulurulu(RubikData.RULURULU_MAP[workCorner][0], RubikData.RULURULU_MAP[workCorner][1]);
			}
		}

		


		wellPlaced.Clear();
		GetWellPlaced();



		foreach (int position in new int[] { 6, 8, 24, 26 })
		{
			if (FindCuby(pattern, objectivePattern[position]) != position)
			{
				Debug.Log("down corners misplaced");
				return null;
			}
		}









		//bien oriente les coins
		List<string> badlyOriented = new List<string>();
		void Fdfd(int f)
		{
			for (int i = 2; i != 0; i--)
            {
				Rotate(pattern, orderList, f, RubikData.DIRECTION_CORRECTION[f]);
				Rotate(pattern, orderList, 3, -1.0f);
				Rotate(pattern, orderList, f, -RubikData.DIRECTION_CORRECTION[f]);
				Rotate(pattern, orderList, 3, 1.0f);
			}
		}
		void Dfdf(int f)
		{
			for (int i = 2; i != 0; i--)
			{
				Rotate(pattern, orderList, 3, -1.0f);
				Rotate(pattern, orderList, f, RubikData.DIRECTION_CORRECTION[f]);
				Rotate(pattern, orderList, 3, 1.0f);
				Rotate(pattern, orderList, f, -RubikData.DIRECTION_CORRECTION[f]);
			}
		}





		foreach (int position in new int[]{ 6, 8, 24, 26 })
        {
			if (!pattern[position].Equals(objectivePattern[position]))
				badlyOriented.Add(pattern[position]);
        }


		if (badlyOriented.Count == 0)
			goto PASS;

		int workPosition = FindCuby(pattern, badlyOriented[0]);
		while (badlyOriented.Count != 0)
        {
			string chosenCuby = badlyOriented[0];
			string objectiveCubyDownColor = objectivePattern[FindCuby(objectivePattern, chosenCuby)][0].ToString();

			RotateUntil(pattern, orderList, 5, 1.0f, () => {
				return FindCuby(pattern, chosenCuby) == workPosition;
			});

			if (workPosition == 6 || workPosition == 26)
            {
				//---------------------------------------------------bug recemment corrige ici-------------------------------------------------//
				if (chosenCuby[1].ToString().Equals(objectivePattern[FindCuby(objectivePattern, chosenCuby)][0].ToString()))
				{
					Dfdf(Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[workPosition][2]));
				}
				else
				{
					Fdfd(Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[workPosition][2]));
				}
			}
			else
            {//----------------------------------------------------------------------le meme bug ici aussi-----------------------------------------------------//
				if (chosenCuby[1].ToString().Equals(objectivePattern[FindCuby(objectivePattern, chosenCuby)][0].ToString()))
				{
					Fdfd(Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[workPosition][1]));
				}
				else
				{
					Dfdf(Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[workPosition][1]));
				}
			}
			badlyOriented.RemoveAt(0);
        }

		PASS:

		RotateUntil(pattern, orderList, 5, 1.0f, () => {
			return pattern[7].Equals(objectivePattern[7]);
		
		});



		for(int i = 0; i < 27; i++)
        {
			if (!pattern[i].Equals(objectivePattern[i]))
				return null;
        }
		return orderList;

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



	protected int FindCuby(string name)
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

	protected int FindCuby(Transform[] positions, Transform cuby)
    	{
		return Array.IndexOf(positions, cuby);
    	}



	protected bool IsCubyOn(int[] face, Transform cuby)
    	{
		int position = FindCuby(cuby);

		for (int i = 0; i < face.Length; i++)
        	{
			if (position == face[i])
				return true;
        	}

		return false;
    	}


	public bool IsSharingFace(int[] face, int position1, int position2)
	{
		return IsCubyOn(face, position1) && IsCubyOn(face, position2);
	}



	protected bool IsCorner(Transform cuby)
	{
		return Array.IndexOf(RubikData.CORNERS, FindCuby(cuby)) != -1;
	}

	protected bool IsMiddle(int position)
	{
		return Array.IndexOf(RubikData.MIDDLES, position) != -1;
	}

	protected bool IsEdge(Transform cuby)
	{
		return Array.IndexOf(RubikData.EDGES, FindCuby(cuby)) != -1;
	}
	







	protected void CorrectCornerOrientation(Transform cuby)
    	{
		int i = FindCuby(cuby.name);
		int defaultPosition = FindCuby(RubikData.DEFAULT_PATTERN, cuby.name);

		cuby.Rotate(RubikData.cornerCorrections[i][defaultPosition]);
	}



	protected void PhysicRotateCorner(string desiredOrientation, Transform cuby)
    	{
		if (cuby.name.Equals(desiredOrientation))
			return;

		int position = FindCuby(cuby.name);

		for (int i = 0; i < 2; i++)
        	{
			cuby.Rotate(RubikData.cornerRotations[position], Space.World);
			RotateCorner(cuby, RubikData.CLOCKWISE);

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
		Vector3 rotation = Vector3.zero;


		cuby.Rotate(RubikData.edgesFrame[position], Space.World);
		cuby.Rotate(rotation, Space.World);
		cuby.Rotate(-RubikData.edgesFrame[position], Space.World);


		cuby.Rotate(RubikData.edgeRotations[FindCuby(cuby.name)], Space.World);
		RotateEdge(cuby);


		if (cuby.name.Equals(desiredOrientation))
			return;


		throw new Exception($"Impossible de trouver l'orientation {desiredOrientation} pour {cuby.name}");
	}
























	protected void NextRandom()
    	{

		Index = random.Next(0, 9);
		direction = random.Next(0, 2) == 0 ? 1.0f : -1.0f;


		randomCounter--;

		RotateBegin();
	}




	protected void RotateBegin()
    {
		isRotating = true;
		degreeCounter = 0.0f;
		

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


	}



	protected void InstantRotate()
    {
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
			cubyPositions[RubikData.rotationPositions[index, i]].RotateAround(transform.position, currentAxis, 90.0f * direction);

		UpdatePositions();
	}


	protected void RotateAnimation()
    {

		float currentRotation = ROTATION_SPEED * Time.fixedDeltaTime * 60.0f;

		for (int i = 0; i < 9; i++)
			cubyPositions[RubikData.rotationPositions[index, i]].RotateAround(transform.position, currentAxis, currentRotation * direction);

		degreeCounter += currentRotation;


		if (degreeCounter >= 90.0f)
        {
			if (degreeCounter > 90.0f)
            {
				for (int i = 0; i < 9; i++)
					cubyPositions[RubikData.rotationPositions[index, i]].RotateAround(transform.position, currentAxis, 90.0f - degreeCounter);
			}

			RotateEnd();
		}
	}


	protected void RotateEnd()
    	{

		UpdatePositions();

		isRotating = false;
	}




	//Permet de garder l'etat interne du cube
	protected void UpdatePositions()
    	{
		/*
			Ici, on reutilise un meme principe pour tous les rotations.
			Voici un exemple: 
			0|1|2
			3|4|5
			6|7|8
			
			Avant la rotation, la position des cuby est : 
			[0, 1, 2, 3, 4, 5, 6, 7, 8]
			
			Et apres la rotation : 
			[2, 5, 8, 1, 4, 7, 0, 3, 6]
			
			La prochaine pile de 'for' ne fait que refleter ces
			changements.
		 */


		
		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPositions[currentPosition] = cubyPositions[RubikData.rotationPositions[index, i + j]];

				currentPosition++;
			}
		}
		

		//Il suffit de retourner le tableau dans l'autre sens pour
		//tourner dans l'autre sens.
		if (direction == -1.0f)
			Array.Reverse(bufferPositions);





		//Correction des noms des cubies une fois tourner, car
		//les cubies changent de noms selon leur position.
		for (int i = 0; i < 9; i++)
        	{
			Transform cuby = cubyPositions[RubikData.rotationPositions[index, i]];
			int oldPosition = i;
			int newPosition = Array.IndexOf(bufferPositions, cuby);


			if (IsCorner(cuby))
            		{
				if (index <= 2 || index >= 6)
				{
					bool a = index == 0 || index == 8;
					bool b = direction == 1.0f;
					bool c = IsCubyOn(RubikData.UP, oldPosition);
					bool d = IsCubyOn(RubikData.UP, newPosition);

					RotateCorner(bufferPositions[i], !(a != b != c != d) ? RubikData.CLOCKWISE: RubikData.COUNTERCLOCKWISE);
				}
			}
			else if (IsEdge(cuby))
            		{
				if (index == 1 || index == 4 || index >= 6)
                		{
					RotateEdge(bufferPositions[i]);
                		}
            		}
			

			cubyPositions[RubikData.rotationPositions[index, i]] = bufferPositions[i];
		}


		//Met a jour currentPattern pour refleter les changements
		UpdatePositions(currentPattern, index, direction);
		
	}


	protected void Print<T>(T[] pattern)
	{
		string message = "[";

		for (int i = 0; i < pattern.Length; i++)
		{
			message += pattern[i];
			if (i + 1 != pattern.Length)
			message += ", ";
		}

		Debug.Log(message);
	}


}
