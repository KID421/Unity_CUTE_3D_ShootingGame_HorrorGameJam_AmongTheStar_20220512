using UnityEngine;

namespace KID
{
    /// <summary>
    /// �C���޲z
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Header("��������")]
        private GameObject goFinish;
        [SerializeField, Header("�s�ռĤH")]
        private GameObject goEnemy;

        private bool hasKey;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "�_��")
            {
                hasKey = true;
                Destroy(other.gameObject);
            }

            if (other.name == "�ӪŲ�" && hasKey)
            {
                goFinish.SetActive(true);
                goEnemy.SetActive(false);
            }
        }
    }
}

