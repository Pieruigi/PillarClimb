using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zomp.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        float verticalDisplacement;

        [SerializeField]
        float distance;

        private void Awake()
        {
            transform.position = Vector3.zero;
            transform.Translate(Vector3.back * distance);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            Vector3 pos = PlayerController.Instance.transform.position;
            pos.x = 0;
            pos.z = transform.position.z;
            transform.position = pos;
            transform.Translate(Vector3.up * verticalDisplacement);
        }
    }

}
