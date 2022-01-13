using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zomp
{
    public class Pillar : MonoBehaviour
    {
        #region properties
        public bool IsFixed
        {
            get { return isFixed; }
            set { SetFixed(value); }
        }

        public int MaxBricks
        {
            get { return maxBricks; }
            set { maxBricks = value; }
        }

        public float MaxLength
        {
            get { return maxBricks * brickLength; }
        }
        #endregion

        #region private fields
        /// <summary>
        /// 0: top brick
        /// 1: middle brick
        /// 2: bottom brick
        /// </summary>
        [SerializeField]
        GameObject[] brickPrefabs = new GameObject[3];
        
        [SerializeField]
        GameObject[] fixedBrickPrefabs = new GameObject[3];


        bool isFixed = false;

        List<GameObject> bricks = new List<GameObject>();

        int maxBricks; // The maximum number of bricks this pillar can be made of
        float brickLength = 1;
        
        #endregion

        #region private methods
        private void Awake()
        {
            

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetFixed(bool value)
        {
            if (!value)
                return;

            brickPrefabs = fixedBrickPrefabs;
        }
        #endregion

        #region public methods
        public void SetMaxBricks(int value)
        {
            maxBricks = value;
        }

        public void AddNewBrick()
        {
            if (bricks.Count >= maxBricks) // No more room to add
                return;

            // Increase the pillar length
            GameObject prefab = brickPrefabs[1];
            if(bricks.Count == maxBricks - 1)
            {
                // Use the bottom brick prefab
                prefab = brickPrefabs[2];
            }
            else
            {
                if(bricks.Count == 0)
                {
                    // Use the first brick prefab
                    prefab = brickPrefabs[0];
                }
            }

            // Create the new brick
            GameObject brick = GameObject.Instantiate(prefab, transform);
            // Set position 
            brick.transform.localPosition = Vector3.down * bricks.Count * brickLength;
            // Add to the list
            bricks.Add(brick);
        }


        #endregion
    }

}
