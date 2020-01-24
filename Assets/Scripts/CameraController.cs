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

        void Start()
        {
            Camera camera = GetComponent<Camera>();
            camera.transform.rotation = Quaternion.Euler(45, 135, 0);


        }

        public void HandleCameraInput()
        {

            Camera camera = GetComponent<Camera>();

            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                camera.orthographicSize -= .5f;
            }
            else if (d < 0f)
            {
                camera.orthographicSize += .5f;
            }

            // rotate left one step when key pressed
            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(this.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }
            //rotate right one step when key pressed
            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(this.transform.position, Vector3.down, rotateSpeed * Time.deltaTime);
            }


            Vector3 rightMovement = transform.right * panSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = transform.up * panSpeed * Time.deltaTime * Input.GetAxis("Vertical");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            transform.position += rightMovement;
            transform.position += upMovement;

        }

        public void PositionCamera(Vector3 targetPosition)
        {

            float cameraX = targetPosition.x - 3;
            float cameraY = targetPosition.y + 3;
            float cameraZ = targetPosition.z + 3;


            Camera camera = GetComponent<Camera>();

            camera.transform.position = new Vector3(cameraX, cameraY, cameraZ);

        }

    }

}