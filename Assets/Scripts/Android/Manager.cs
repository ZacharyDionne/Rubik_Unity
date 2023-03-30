using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Android
{
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        private Camera gameCamera;

        private RubiksCube cube;

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
