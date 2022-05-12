using UnityEngine;
using UnityEngine.AI;

namespace KID
{
    /// <summary>
    /// 外星人
    /// 追蹤玩家以及進入攻擊範圍內開槍
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region 資料
        [SerializeField, Header("追蹤速度"), Range(0, 10)]
        private float speed = 2.5f;
        [SerializeField, Header("追蹤距離"), Range(0, 50)]
        private float rangeTrack = 10;
        [SerializeField, Header("開槍距離"), Range(0, 50)]
        private float rangeStopToFire = 5;
        [SerializeField, Header("開槍冷卻時間"), Range(0, 10)]
        private float cdFire = 3;
        [SerializeField, Header("子彈物件")]
        private GameObject goBullet;
        [SerializeField, Header("生成子彈位置")]
        private Transform traFirePoint;
        [SerializeField, Header("子彈速度"), Range(0, 5000)]
        private float speedBullet = 1000;
        [SerializeField, Header("擊中特效")]
        private GameObject goHit;
        [SerializeField, Header("血量"), Range(0, 1000)]
        private float hp = 500;

        private float timerFire;
        private NavMeshAgent nav;
        private Animator ani;
        private Rigidbody rig;
        private Transform traPlayer;
        private string namePlayer = "FPS 控制器";
        private string parameterWaitToFire = "開關等待持槍";
        private string parameterWalk = "開關走路";
        private string parameterDead = "開關死亡";
        private string parameterHurt = "觸發受傷";
        private string parameterFire = "觸發開槍";
        private string nameBulletPlayer = "Bullet_Prefab";

        private float distanceWithPlayer { get => Vector3.Distance(transform.position, traPlayer.position); }
        #endregion

        #region 事件
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

        #region 方法
        /// <summary>
        /// 檢查距離
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
        /// 追蹤
        /// </summary>
        private void Track()
        {
            nav.isStopped = false;
            nav.SetDestination(traPlayer.position);
            ani.SetBool(parameterWalk, true);
            ani.SetBool(parameterWaitToFire, false);
        }

        /// <summary>
        /// 開槍
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
        /// 發射子彈
        /// </summary>
        private void ShootBullet()
        {
            GameObject temp = Instantiate(goBullet, traFirePoint.position, Quaternion.identity);
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * speedBullet);
        }

        /// <summary>
        /// 受傷
        /// </summary>
        private void Hurt(Vector3 hitPoint, float damage)
        {
            ani.SetTrigger(parameterHurt);
            Instantiate(goHit, hitPoint, Quaternion.identity);
            hp -= damage;

            if (hp <= 0) Dead();
        }

        /// <summary>
        /// 死亡
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

