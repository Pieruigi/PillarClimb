using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zomp.Controllers;

namespace Zomp.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region actions
        public UnityAction<bool> OnPause;
        #endregion

        #region properties
        public static GameManager Instance { get; private set; }
        #endregion

        #region private methods
        int mainSceneIndex = 0;
        int gameSceneIndex = 1;
        bool paused = false;
        #endregion

        #region private methods
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                paused = GameManager.Instance.IsPaused();
                SceneManager.sceneLoaded += HandleOnSceneLoaded;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (IsInGame())
            {
                // Pause to add some delay on start
                SetPaused(true);
            }
            else
            {
                SetPaused(false);
            }
        }
        #endregion

        #region public methods
        public bool IsInGame()
        {
            return SceneManager.GetActiveScene().buildIndex == gameSceneIndex;
        }

        public void SetPaused(bool value)
        {
            paused = value;

            OnPause?.Invoke(value);
        }

        public bool IsPaused()
        {
            return paused;
        }
        #endregion
    }

}
