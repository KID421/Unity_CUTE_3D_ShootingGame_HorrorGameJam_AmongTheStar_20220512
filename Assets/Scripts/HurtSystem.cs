using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KID
{
    /// <summary>
    /// 受傷系統
    /// </summary>
    public class HurtSystem : MonoBehaviour
    {
        [SerializeField, Header("血條")]
        private Image imgHp;
        [SerializeField, Header("血量")]
        private float hp = 100;
        [SerializeField, Header("外星人子彈")]
        private string nameBulletAlien = "外星人子彈";
        [SerializeField, Header("死亡後要停止的行為")]
        private Behaviour[] behaviour;
        [SerializeField, Header("死亡介面")]
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
        /// 受傷
        /// </summary>
        /// <param name="damage">收到傷害</param>
        private void Hurt(float damage)
        {
            hp -= damage;
            imgHp.fillAmount = hp / hpMax;

            if (hp <= 0) Dead();
        }

        /// <summary>
        /// 死亡
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

