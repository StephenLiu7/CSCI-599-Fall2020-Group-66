using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Initial parameter
    /// </summary>
    
    //============================================= Character part  ===================================================================================
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector3 movement;
    //Vector2 moveDirec;
    //Vector2 lastMoveDirect;
    float facing = 1.0f; // -1 left, 1 right;
    // =================================================================================================================================================

    //============================================= initial Gun & shooting part  =======================================================================
    public Transform handgun;
    //public Transform player;
    public Transform cur_gun;

    public Transform missile_gun;
    //public GameObject bulletPrefabs;
    public Transform cur_bullet;
    public Transform missile;
    public Transform handgun_bullet;

    double wait_time = 0.4;


    public float proTime = 0.0f;
    public float NextTime = 0.0f;
    bool LOR = false;       // initial facing right
    // Update is called once per frame
    // =================================================================================================================================================

    private void Awake()
    {
        //missile.gameObject.setActive(false);
        missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
        // set in the Unity those are backup code
        //Destroy(missile_gun.gameObject);
        //player = transform.Find("Player");
        //player = transform.Find("Player");
        //handgun = transform.Find("handgun");

        //handgun = GameObject.Find("handgun").transform;
        //missile_gun = handgun = GameObject.Find("missile_gun").transform;
        //cur_gun = handgun;

        //missile = GameObject.Find("player_missle").transform;
        //handgun_bullet = handgun = GameObject.Find("player_bullet").transform;
        //cur_bullet = handgun_bullet;
        //handgun = transform.Find("handgun");
        //missile_gun = handgun = GameObject.Find("missile_gun").transform;
    }

    void Update()
    {
        // =====================================================Character & shooting script =================================================================
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.z = 0.0f;
        /*
        if((movement.x ==0 && movement.y == 0) && moveDirec.x != 0 || moveDirec.y !=0)
        {
            lastMoveDirect = moveDirec;
        }
        moveDirec = new Vector2(movement.x, movement.y).normalized;
        */
        Vector3 theScale = cur_gun.localScale;
         
        if(movement.x > 0.001f)
        {
            if(facing == -1.0f)     // we have a flip
            {
                LOR = false;
                theScale.x *= -1;
                cur_gun.localScale = theScale;
                Vector2 diff = new Vector2(0.25f, 0);
                cur_gun.Translate(diff,Space.World);        // space world == absolute axis    Didn't change with object rotate
            }
            facing = 1.0f;
        }
        else if(movement.x < -0.001f)
        {
            if (facing == 1.0f)     // we have a flip
            {
                LOR = true;
                theScale.x *= -1;
                cur_gun.localScale = theScale;
                Vector2 diff = new Vector2(-0.25f, 0);
                cur_gun.Translate(diff, Space.World);
            }
            facing = -1.0f;
        }
      
        animator.SetFloat("Horizontal",facing);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);
        animator.SetFloat("LastHori", facing);
        // =================================================================================================================================================

        // ================================================================ shooting time SEPA =============================================================
        proTime = Time.fixedTime;
        FollowMouseRotate();
        
        // test model for switch weapon
        if (Input.GetMouseButtonDown(1))
        {
            if(cur_bullet == missile)
            {
                wait_time = 0.5;
                missile_gun.gameObject.GetComponent<Renderer>().enabled = false;
                handgun.gameObject.GetComponent<Renderer>().enabled = true;
                cur_bullet = handgun_bullet;
                cur_gun = handgun;
                //cur_gun = Instantiate(handgun, transform.position, Quaternion.identity);
                //Destroy(missile_gun.gameObject);
            }
            else
            {
                wait_time = 1.2;
                missile_gun.gameObject.GetComponent<Renderer>().enabled = true;
                handgun.gameObject.GetComponent<Renderer>().enabled = false;
                cur_bullet = missile;
                cur_gun = missile_gun;
                //cur_gun = Instantiate(missile_gun, transform.position, Quaternion.identity);
                //Destroy(handgun.gameObject);
            }
        }
        Debug.Log(wait_time);
        if (proTime - NextTime >= wait_time)
        {
            
            Shooting();
            NextTime = proTime;
        }
        // =================================================================================================================================================
    }

    // =================================================================  CH update  =======================================================================
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement + moveSpeed * Time.fixedDeltaTime );
        transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
    }
    // =================================================================================================================================================

    // =============================================================== Shooting function ===================================================================
    // Below is Gun & Shooting
    private void FollowMouseRotate()
    {
        Vector3 mouse = Input.mousePosition;
        //Debug.Log(mouse);
        Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
        Vector3 direction = obj - mouse;
        if (LOR == true)
        { direction = obj - mouse; }
        else { direction = mouse - obj; }
        direction.z = 0f;
        //Debug.Log(direction);
        direction = direction.normalized;
        cur_gun.right = direction;
        //  Debug.Log(player.GetAxis("aimHor"));

        //transform.right = direction;         if rotate with Player together
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Vector3 bulletDirection = Input.mousePosition;
            Vector3 obj = Camera.main.WorldToScreenPoint(cur_gun.position);
            Vector3 direction = bulletDirection - obj;
            direction.Normalize();
            Transform bullet = Instantiate(cur_bullet, transform.position, Quaternion.identity);
            //bullet.right = bulletDirection;
            if (bullet == missile)
            { bullet.GetComponent<Rigidbody2D>().velocity = direction * 11; }
            else { bullet.GetComponent<Rigidbody2D>().velocity = direction * 5; }
            //bullet.GetComponent<Rigidbody2D>().velocity = direction * 11;
            bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Destroy(bullet.gameObject, 10.0f);
        }
    }
    // =================================================================================================================================================


}
