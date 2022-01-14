using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zomp.Controllers;

namespace Zomp.Utility
{
    public class PillarDestroyer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tags.Pillar.Equals(other.tag))
            {
                LevelController.Instance.UpdatePillars();
            }
        }
    }

}

