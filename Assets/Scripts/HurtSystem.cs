using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KID
{
    /// <summary>
    /// ���˨t��
    /// </summary>
    public class HurtSystem : MonoBehaviour
    {
        [SerializeField, Header("���")]
        private Image imgHp;
        [SerializeField, Header("��q")]
        private float hp = 100;
        [SerializeField, Header("�~�P�H�l�u")]
        private string nameBulletAlien = "�~�P�H�l�u";
        [SerializeField, Header("���`��n����欰")]
        private Behaviour[] behaviour;
        [SerializeField, Header("���`����")]
        private GameObject goDead;

        private float hpMax;

        private void Awake()
        {
            hpMax = hp;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name.Contains(nameBulletAlien))
            {
                float attack = collision.gameObject.GetComponent<Bullet>().attack;
                Hurt(attack);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="damage">����ˮ`</param>
        private void Hurt(float damage)
        {
            hp -= damage;
            imgHp.fillAmount = hp / hpMax;

            if (hp <= 0) Dead();
        }

        /// <summary>
        /// ���`
        /// </summary>
        private void Dead()
        {
            goDead.SetActive(true);

            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].enabled = false;
            }

            Invoke("DelayReplay", 2);
        }

        private void DelayReplay()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

