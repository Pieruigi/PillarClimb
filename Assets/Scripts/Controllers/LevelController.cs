using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zomp.Managers;

namespace Zomp.Controllers
{
    public class LevelController : MonoBehaviour
    {
        #region properties
        public static LevelController Instance { get; private set; }

        public const float PillarHorizontalDisplacement = 4f;

        #endregion

        #region private fields
        [SerializeField]
        GameObject pillarPrefab;
               

        List<Pillar> pillars = new List<Pillar>();
        
        float fixedPillarTime = 12f; // One fixed pillar every tot meters
        System.DateTime lastFixedPillarTime;
        bool paused = false;
        int maxBuilding = 2;
        #endregion

        #region private methods
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
              
                CreateStartingPillar();
            }
            else
            {
                Destroy(gameObject);
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            paused = GameManager.Instance.IsPaused();
            GameManager.Instance.OnPause += HandleOnPause;
           
        }

        // Update is called once per frame
        void Update()
        {
            if (paused)
                return;

        }

        void HandleOnPause(bool value)
        {
            paused = value;
        }

       
        void CreateStartingPillar()
        {
            CreateNewPillar(5, true, true);
            
        }

        Pillar CreateNewPillar(int maxBricks, bool isFixed, bool toLeft)
        {
            // Get the last pillar to compute distance
            float lastY = 0;
            if (pillars.Count > 0)
                lastY = pillars[pillars.Count - 1].transform.position.y;

            // Create the new pillar
            Pillar pillar = GameObject.Instantiate(pillarPrefab).GetComponent<Pillar>();
            pillar.IsFixed = isFixed;
            pillar.MaxBricks = maxBricks;

            // Set in position
            pillar.transform.position = (toLeft ? Vector3.left : Vector3.right) * PillarHorizontalDisplacement;
            pillar.transform.Translate(Vector3.up * ( lastY + pillar.MaxLength + Pillar.BrickLength/2f));
            // Add pillar to the list
            pillars.Add(pillar);

            // If the pillar is fixed we show all the brick, otherwise we show only the first one.
            if (isFixed)
            {
                for (int i = 0; i < maxBricks; i++)
                    pillar.AddNewBrick();
            }
           

            return pillar;
        }
        #endregion

        #region public methods
     
        /// <summary>
        /// Get the next pillar to build by the player.
        /// </summary>
        /// <param name="lastWalkingPillar"></param>
        /// <returns></returns>
        public Pillar GetNextPillarToBuild(Pillar lastWalkingPillar)
        {
            int index = pillars.IndexOf(lastWalkingPillar);
            if (pillars.Count - 1 - index > maxBuilding)
                return null;
            
            return CreateNewPillar(Random.Range(2, Pillar.MaxBricksPerPillar + 1), false, true);
           
        }

        /// <summary>
        /// Get the pillar the brick passed as parameter belongs to.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public Pillar GetPillar(GameObject brick)
        {
            foreach(Pillar pillar in pillars)
            {
                for(int i=0; i<pillar.BrickCount; i++)
                {
                    if (brick == pillar.GetBrickAt(i))
                        return pillar;
                }
            }

            return null;
        }
        #endregion
    }

}
