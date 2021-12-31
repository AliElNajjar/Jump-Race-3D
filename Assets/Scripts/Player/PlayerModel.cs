using UnityEngine;

namespace Models
{
    public class Player
    {
        private readonly Rigidbody _rigidBody;
        private readonly Animator _animator;

        public Player(
            Rigidbody rigidBody, MeshRenderer renderer, Animator animator)
        {
            _rigidBody = rigidBody;
            _animator = animator;
        }

        public Animator Animator
        {
            get { return _animator; }
        }

        public bool IsDead
        {
            get; set;
        }

        public Vector3 LookDir
        {
            get { return _rigidBody.transform.forward; }
        }

        public Quaternion Rotation
        {
            get { return _rigidBody.rotation; }
            set { _rigidBody.rotation = value; }
        }

        public Vector3 Position
        {
            get { return _rigidBody.position; }
            set { _rigidBody.position = value; }
        }

        public Vector3 Velocity
        {
            get { return _rigidBody.velocity; }
        }

        public void AddForce(Vector3 force)
        {
            _rigidBody.AddForce(force);
        }
    }
}