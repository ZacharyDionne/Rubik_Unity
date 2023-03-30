using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Android
{
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        private Camera gameCamera;

        private RubiksCube cube;

        [SerializeField]
        private TMP_InputField inputField;

        private int index = 0;


        public int Index
        {
            get => index;
            set
            {
                if (value < 0)
                    return;
                if (value > 8)
                    return;
                index = value;
            }
        }




        void Start()
        {

        }

        
        void Update()
        {

        }

        private void Awake()
        {
            cube = RubiksCube.GenerateCube(RubikData.DEFAULT_PATTERN);
        }


        public void Generate()
        {
            Destroy(cube.gameObject);

            string json = inputField.text;

            


            string[] pattern = new string[27];
            pattern[13] = "none";
            int position = 0;
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i].Equals('"'))
                {
                    i++;
                    string cuby = "";
                    while (!json[i].Equals('"'))
                    {
                        cuby += json[i];
                        i++;
                    }
                    pattern[position] = cuby;
                    position++;
                    Debug.Log(cuby + ", " + position);
                }
            }

            RubiksCube.Print<string>(pattern);
            

            cube = RubiksCube.GenerateCube(pattern);

        }


        public void Left()
        {
            gameCamera.transform.RotateAround(cube.transform.position, Vector3.up, 90.0f);
        }

        public void Right()
        {
            gameCamera.transform.RotateAround(cube.transform.position, Vector3.up, -90.0f);
        }


        public void Rotate1()
        {
            cube.Rotate(Index, 1.0f);
        }
        public void Rotate2()
        {
            cube.Rotate(Index, -1.0f);
        }

        public void IndexUp()
        {
            Index++;
        }
        public void IndexDown()
        {
            cube.Index--;
        }




        public void Copy()
        {
            
            cube.GetJSON((json) => {
                /*TextEditor te = new TextEditor();
                te.text = json;
                te.SelectAll();
                te.Copy();*/
                Debug.Log("ok");
                GUIUtility.systemCopyBuffer = json;
            });
        }

        public void Copy2()
        {

            cube.GetJSON((json) => {
                TextEditor te = new TextEditor();
                te.text = json;
                te.SelectAll();
                te.Copy();
            });
        }

    }

    

}
