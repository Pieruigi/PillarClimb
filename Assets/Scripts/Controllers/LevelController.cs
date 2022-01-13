using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zomp.Controllers
{
    public class LevelController : MonoBehaviour
    {
        public const float PillarHorizontalDisplacement = 4f;

        #region private fields
        [SerializeField]
        GameObject pillarPrefab;

        int currentPillarId; // The id of the pillar the player is currently running

        List<Pillar> pillars = new List<Pillar>();
        int minPillars = 10;

        float fixedPillarTime = 12f; // One fixed pillar every tot meters
        System.DateTime lastFixedPillarTime;
        float scrollSpeed = 1f;
  
        #endregion

        #region private methods
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Create the starting pillar
            CreateNewPillar(Pillar.MaxBricksPerPillar, true, true);
            Pillar lastPillar = pillars[pillars.Count - 1];
            System.DateTime fakeDateTime = System.DateTime.UtcNow.AddSeconds(lastPillar.MaxLength / scrollSpeed);
            lastFixedPillarTime = fakeDateTime;

            for (int i=0; i<minPillars; i++)
            {
                // Check if we must add a fixed pillar
                if((fakeDateTime - lastFixedPillarTime).TotalSeconds > fixedPillarTime)
                {
                    
                    CreateNewPillar(Random.Range(3, Pillar.MaxBricksPerPillar + 1), true, true);
                    lastFixedPillarTime = fakeDateTime;
                    
                }
                else
                {
                    CreateNewPillar(Random.Range(2, Pillar.MaxBricksPerPillar + 1), false, true);

                }
                lastPillar = pillars[pillars.Count - 1];
                fakeDateTime = fakeDateTime.AddSeconds((lastPillar.MaxLength + Pillar.BrickLength/2f) / scrollSpeed);
            }
                

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Init()
        {

        }

        void CreateNewPillar(int maxBricks, bool isFixed, bool toLeft)
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
            else
            {
                pillar.AddNewBrick();
            }
        }
        #endregion
    }

}
