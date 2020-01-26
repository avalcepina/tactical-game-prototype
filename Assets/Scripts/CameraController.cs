using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        public float panSpeed = .5f;

        [SerializeField]
        public float rotateSpeed = 10f;

        new Camera camera;

        void Start()
        {
            camera = GetComponent<Camera>();
            camera.transform.rotation = Quaternion.Euler(45, 135, 0);


        }

        public void HandleCameraInput()
        {

            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                camera.orthographicSize -= .5f;
            }
            else if (d < 0f)
            {
                camera.orthographicSize += .5f;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(this.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(this.transform.position, Vector3.down, rotateSpeed * Time.deltaTime);
            }


            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {

                Vector3 rightMovement = transform.right * panSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                Vector3 upMovement = transform.up * panSpeed * Time.deltaTime * Input.GetAxis("Vertical");

                camera.transform.position += rightMovement;
                camera.transform.position += upMovement;

            }

        }

        public void PositionCamera(Vector3 targetPosition)
        {

            float cameraX = targetPosition.x - 3;
            float cameraY = targetPosition.y + 3;
            float cameraZ = targetPosition.z + 3;

            camera.transform.position = new Vector3(cameraX, cameraY, cameraZ);

        }

    }

}