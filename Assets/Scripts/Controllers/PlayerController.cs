using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zomp.Managers;


namespace Zomp.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region properties
        public static PlayerController Instance { get; private set; }

        #endregion

        #region private fields
        [SerializeField]
        float maxSpeed = 1f / 0.6f;

        float speed = 0;
        bool paused = false;
        Rigidbody rb;
        bool building = false;
        bool jumping = false;
        System.DateTime lastBuildingTime;
        float buildingTime = 0.6f;
        Pillar buildingPillar;
        Pillar lastWalkingPillar;
        bool grounded = false;
        Collider coll;
        #endregion

        #region private methods

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                coll = GetComponent<Collider>();

                // Get rigid body
                rb = GetComponent<Rigidbody>();
                // Set starting position 
                rb.MovePosition(Vector3.left * (LevelController.PillarHorizontalDisplacement - 1f));
                rb.MovePosition(rb.position + Vector3.up * Pillar.BrickLength);
                // Rotate
                rb.MoveRotation(Quaternion.Euler(-90, 0, 0));
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            SetPaused(GameManager.Instance.IsPaused());
            // Set handles
            GameManager.Instance.OnPause += HandleOnPause;
        }

        // Update is called once per frame
        void Update()
        {
            if (paused)
                return;

            
            
            CheckBuilding();

            
            
            
        }

        private void FixedUpdate()
        {
            if (paused)
                return;

            // Run
            rb.MovePosition(rb.position + Vector3.up * speed * Time.fixedDeltaTime);
            //transform.position += Vector3.forward * speed * Time.deltaTime;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnPause -= HandleOnPause;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tags.Pillar.Equals(other.tag))
            {
                grounded = true;
                lastWalkingPillar = LevelController.Instance.GetPillar(other.gameObject);
                Debug.Log("Grounded:" + grounded);
                Debug.Log("LastWalkingPillar:" + lastWalkingPillar);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (Tags.Pillar.Equals(other.tag))
            {
                grounded = false;
               
                Debug.Log("Grounded:" + grounded);
            }
        }

        void CheckBuilding()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartBuildingPillar();

            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                StopBuildingPillar();
            }
#endif
            if (building)
            {
                // If we are building no pillar or the last pillar we were building is the one we
                // are walking now then we need a new pillar to build
                if(buildingPillar == null || buildingPillar == lastWalkingPillar || buildingPillar.IsCompleted())
                {
                    buildingPillar = LevelController.Instance.GetNextPillarToBuild(lastWalkingPillar);
                    Debug.Log("Pillar to build:" + buildingPillar);
                }

                if(buildingPillar != null)
                {
                    System.DateTime now = System.DateTime.UtcNow;
                    // Build
                    if ((now - lastBuildingTime).TotalSeconds > buildingTime)
                    {
                        lastBuildingTime = now;
                        buildingPillar.AddNewBrick();

                        
                    }
                }
                   
            }
        }



        void HandleOnPause(bool paused)
        {
            SetPaused(paused);
        }

        void SetPaused(bool value)
        {
            paused = value;
            if (paused)
            {
                speed = 0;
            }
            else
            {
                speed = maxSpeed;
            }
        }
        #endregion
        #region public methods
        public void StartBuildingPillar()
        {
            building = true;
            lastBuildingTime = System.DateTime.UtcNow;
        }

        public void StopBuildingPillar()
        {
            building = false;
        }
        #endregion
    }

}
