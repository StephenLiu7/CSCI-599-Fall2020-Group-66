using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 这个文件无效  是前期武器单独测试用的
/// </summary>

public class PlayerWeapon : MonoBehaviour
{
    //public int playerId = 0;
    //private Player player;
    private Transform handgun;
    public GameObject bulletPrefabs;
    public float proTime = 0.0f;
    public float NextTime = 0.0f;
    //public Animator animator;
    //float facing = 1.0f;

    private void Awake()
    {
        //player = ReInput.players.GetPlayer(playerId);
        handgun = transform.Find("handgun");
    }

    // Update is called once per frame
    void Update()
    {
        proTime = Time.fixedTime;
        if (proTime - NextTime >= 0.03)
        {
            FollowMouseRotate();
            Shooting();
            NextTime = proTime;
        }
    }
    private void FollowMouseRotate()
    {
        Vector3 mouse = Input.mousePosition;
        //Debug.Log(mouse);
        Vector3 obj = Camera.main.WorldToScreenPoint(handgun.position);
        Vector3 direction = obj - mouse;
        direction.z = 0f;
        //Debug.Log(direction);
        direction = direction.normalized;
        handgun.right = direction;
      //  Debug.Log(player.GetAxis("aimHor"));

        //transform.right = direction;         if rotate with Player together
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Vector3 bulletDirection = Input.mousePosition;
            Vector3 obj = Camera.main.WorldToScreenPoint(handgun.position);
            Vector3 direction = bulletDirection - obj;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            //bullet.right = bulletDirection;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 4;
            bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Destroy(bullet, 2.0f);
        }
    }
}
