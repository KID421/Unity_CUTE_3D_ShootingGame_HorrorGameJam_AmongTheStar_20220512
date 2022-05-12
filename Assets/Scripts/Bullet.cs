using UnityEngine;

namespace KID
{
    /// <summary>
    /// ¤l¼u
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [Header("¤l¼u§ðÀ»¤O"), Range(0, 100)]
        public float attack = 20;

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}

