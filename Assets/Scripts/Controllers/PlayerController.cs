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
        #endregion

        #region private methods

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

               

                // Get rigid body
                rb = GetComponent<Rigidbody>();
                // Set starting position 
                rb.MovePosition(Vector3.left * (LevelController.PillarHorizontalDisplacement - 1f));
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
            paused = GameManager.Instance.IsPaused();
            // Set handles
            GameManager.Instance.OnPause += HandleOnPause;
        }

        // Update is called once per frame
        void Update()
        {
            if (paused)
                return;

            speed = maxSpeed;
            
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

        #endregion
    }

}
