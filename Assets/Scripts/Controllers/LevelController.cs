using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zomp.Controllers
{
    public class LevelController : MonoBehaviour
    {
        #region private fields
        [SerializeField]
        GameObject pillarPrefab;

        int currentPillarId; // The id of the pillar the player is currently running

        List<Pillar> pillars = new List<Pillar>();
        int maxBricks = 5;
        #endregion

        #region private methods
        private void Awake()
        {
            // Create the starting pillar
            Pillar pillar = GameObject.Instantiate(pillarPrefab).GetComponent<Pillar>();
            pillar.IsFixed = true;
            pillar.MaxBricks = maxBricks;
            // Move the pillar up
            pillar.transform.position = Vector3.zero;
            pillar.transform.Translate(Vector3.up * pillar.MaxLength);
            pillars.Add(pillar);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Create the first pillar
            for(int i=0; i<maxBricks; i++)
                pillars[0].AddNewBrick();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
    }

}
