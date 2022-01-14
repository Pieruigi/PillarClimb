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

        int currentPillarId; // The id of the pillar the player is currently running

        List<Pillar> pillars = new List<Pillar>();
        int minPillars = 10;

        float fixedPillarTime = 12f; // One fixed pillar every tot meters
        System.DateTime lastFixedPillarTime;
        float scrollSpeed = 1f;
        bool paused = false;
        System.DateTime lastPillarUpdateTime;
        float pillarUpdateTime = 1.66666666f;
        #endregion

        #region private methods
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Init();
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

            UpdatePillars();
        }

        void HandleOnPause(bool value)
        {
            paused = value;
        }

        void Init()
        {
            // Create the starting pillar
            CreateNewPillar(Pillar.MaxBricksPerPillar, true, true);
            Pillar lastPillar = pillars[pillars.Count - 1];
            System.DateTime fakeDateTime = System.DateTime.UtcNow.AddSeconds(lastPillar.MaxLength / scrollSpeed);
            lastFixedPillarTime = fakeDateTime;

            for (int i = 0; i < minPillars; i++)
            {
                // Check if we must add a fixed pillar
                if ((fakeDateTime - lastFixedPillarTime).TotalSeconds > fixedPillarTime)
                {

                    CreateNewPillar(Random.Range(3, Pillar.MaxBricksPerPillar + 1), true, true);
                    lastFixedPillarTime = fakeDateTime;

                }
                else
                {
                    CreateNewPillar(Random.Range(2, Pillar.MaxBricksPerPillar + 1), false, true);

                }
                lastPillar = pillars[pillars.Count - 1];
                fakeDateTime = fakeDateTime.AddSeconds((lastPillar.MaxLength + Pillar.BrickLength / 2f) / scrollSpeed);
            }

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

        #region public methods
        public void UpdatePillars()
        {

            // Destroy the older pillar
            Pillar p = pillars[0];
            pillars.Remove(p);

            // Create a new pillar to the top
            if ((System.DateTime.UtcNow - lastFixedPillarTime).TotalSeconds > fixedPillarTime)
            {

                CreateNewPillar(Random.Range(3, Pillar.MaxBricksPerPillar + 1), true, true);
                lastFixedPillarTime = System.DateTime.UtcNow;

            }
            else
            {
                CreateNewPillar(Random.Range(2, Pillar.MaxBricksPerPillar + 1), false, true);
            }
            lastPillarUpdateTime = System.DateTime.UtcNow;

        }
        #endregion
    }

}
