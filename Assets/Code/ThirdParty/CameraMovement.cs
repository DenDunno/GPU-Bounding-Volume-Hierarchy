using UnityEngine;

namespace Code.ThirdParty
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _sensitivity = 10f;
        [SerializeField] private float _minimumY = -60F;
        [SerializeField] private float _maximumY = 60F;
        private float _rotationY;
    
        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            float moveSpeed = GetMoveSpeed();
            transform.position += transform.forward * (Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
            transform.position += transform.right * (Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed);
        }

        private float GetMoveSpeed()
        {
            float moveSpeed = _speed;

            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed *= 2;
        
            return moveSpeed;
        }

        private void Rotate()
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;

            _rotationY += Input.GetAxis("Mouse Y") * _sensitivity;
            _rotationY = Mathf.Clamp (_rotationY, _minimumY, _maximumY);

            transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
        }
    }
}