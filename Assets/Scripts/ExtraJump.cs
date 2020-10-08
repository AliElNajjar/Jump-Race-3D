using UnityEngine;

public class ExtraJump : MonoBehaviour
{
    [SerializeField] private float m_jumpForce = 15;

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }

    }
}