using UnityEngine;

namespace KID
{
    /// <summary>
    /// 遊戲管理
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Header("結束介面")]
        private GameObject goFinish;
        [SerializeField, Header("群組敵人")]
        private GameObject goEnemy;

        private bool hasKey;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "鑰匙")
            {
                hasKey = true;
                Destroy(other.gameObject);
            }

            if (other.name == "太空船" && hasKey)
            {
                goFinish.SetActive(true);
                goEnemy.SetActive(false);
            }
        }
    }
}

