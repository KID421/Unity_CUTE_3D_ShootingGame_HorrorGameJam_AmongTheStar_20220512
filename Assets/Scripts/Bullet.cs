using UnityEngine;

namespace KID
{
    /// <summary>
    /// �l�u
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [Header("�l�u�����O"), Range(0, 100)]
        public float attack = 20;

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}

