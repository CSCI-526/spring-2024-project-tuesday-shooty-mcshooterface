using UnityEngine;

namespace Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField]
        private float baseSpeed = 2f;

        [SerializeField]
        private float mouseSensitivity = 5f;

        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private GameObject headPivot;

        void Update()
        {

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            headPivot.transform.Rotate(Vector3.left * mouseY);

            float x = Input.GetAxis("Horizontal") * baseSpeed;
            float z = Input.GetAxis("Vertical") * baseSpeed;

            Vector3 move = (headPivot.transform.right * x + headPivot.transform.forward * z) * baseSpeed;
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }

        public void UpdateMouseSensitivity(float newSensitivity)
        {
            mouseSensitivity = newSensitivity;
        }
    }
}
