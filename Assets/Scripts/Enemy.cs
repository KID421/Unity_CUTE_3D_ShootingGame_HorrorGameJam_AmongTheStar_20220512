using UnityEngine;
using UnityEngine.AI;

namespace KID
{
    /// <summary>
    /// �~�P�H
    /// �l�ܪ��a�H�ζi�J�����d�򤺶}�j
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region ���
        [SerializeField, Header("�l�ܳt��"), Range(0, 10)]
        private float speed = 2.5f;
        [SerializeField, Header("�l�ܶZ��"), Range(0, 50)]
        private float rangeTrack = 10;
        [SerializeField, Header("�}�j�Z��"), Range(0, 50)]
        private float rangeStopToFire = 5;
        [SerializeField, Header("�}�j�N�o�ɶ�"), Range(0, 10)]
        private float cdFire = 3;
        [SerializeField, Header("�l�u����")]
        private GameObject goBullet;
        [SerializeField, Header("�ͦ��l�u��m")]
        private Transform traFirePoint;
        [SerializeField, Header("�l�u�t��"), Range(0, 5000)]
        private float speedBullet = 1000;
        [SerializeField, Header("�����S��")]
        private GameObject goHit;
        [SerializeField, Header("��q"), Range(0, 1000)]
        private float hp = 500;

        private float timerFire;
        private NavMeshAgent nav;
        private Animator ani;
        private Rigidbody rig;
        private Transform traPlayer;
        private string namePlayer = "FPS ���";
        private string parameterWaitToFire = "�}�����ݫ��j";
        private string parameterWalk = "�}������";
        private string parameterDead = "�}�����`";
        private string parameterHurt = "Ĳ�o����";
        private string parameterFire = "Ĳ�o�}�j";
        private string nameBulletPlayer = "Bullet_Prefab";

        private float distanceWithPlayer { get => Vector3.Distance(transform.position, traPlayer.position); }
        #endregion

        #region �ƥ�
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.35f);
            Gizmos.DrawSphere(transform.position, rangeTrack);

            Gizmos.color = new Color(1, 0, 0, 0.35f);
            Gizmos.DrawSphere(transform.position, rangeStopToFire);
        }

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            ani = GetComponent<Animator>();
            rig = GetComponent<Rigidbody>();
            nav.speed = speed;

            traPlayer = GameObject.Find(namePlayer).transform;
        }

        private void Update()
        {
            CheckDistance();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name.Contains(nameBulletPlayer))
            {
                float bulletAttack = collision.gameObject.GetComponent<Bullet>().attack;
                Hurt(collision.GetContact(0).point, bulletAttack);
            }
        }
        #endregion

        #region ��k
        /// <summary>
        /// �ˬd�Z��
        /// </summary>
        private void CheckDistance()
        {
            if (distanceWithPlayer <= rangeTrack && distanceWithPlayer > rangeStopToFire) Track();
            else if (distanceWithPlayer <= rangeStopToFire) Fire();
            else
            {
                nav.isStopped = true;
                ani.SetBool(parameterWalk, false);
            }
        }

        /// <summary>
        /// �l��
        /// </summary>
        private void Track()
        {
            nav.isStopped = false;
            nav.SetDestination(traPlayer.position);
            ani.SetBool(parameterWalk, true);
            ani.SetBool(parameterWaitToFire, false);
        }

        /// <summary>
        /// �}�j
        /// </summary>
        private void Fire()
        {
            nav.isStopped = true;

            if (timerFire < cdFire)
            {
                timerFire += Time.deltaTime;
                ani.SetBool(parameterWaitToFire, true);
            }
            else
            {
                Vector3 posLook = traPlayer.position;
                posLook.y = transform.position.y;
                transform.LookAt(posLook);
                ani.SetTrigger(parameterFire);
                ShootBullet();
                timerFire = 0;
            }
        }

        /// <summary>
        /// �o�g�l�u
        /// </summary>
        private void ShootBullet()
        {
            GameObject temp = Instantiate(goBullet, traFirePoint.position, Quaternion.identity);
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * speedBullet);
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Hurt(Vector3 hitPoint, float damage)
        {
            ani.SetTrigger(parameterHurt);
            Instantiate(goHit, hitPoint, Quaternion.identity);
            hp -= damage;

            if (hp <= 0) Dead();
        }

        /// <summary>
        /// ���`
        /// </summary>
        private void Dead()
        {
            nav.isStopped = true;
            nav.enabled = false;
            ani.SetBool(parameterDead, true);
            GetComponent<Collider>().enabled = false;
            rig.constraints = RigidbodyConstraints.FreezeAll;
            enabled = false;
        }
        #endregion
    }
}

