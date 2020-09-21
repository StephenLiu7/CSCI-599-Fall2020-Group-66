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
    private Transform handgun;
    public GameObject bulletPrefabs;
    public float proTime = 0.0f;
    public float NextTime = 0.0f;
    bool LOR = false;       // initial facing right
    // Update is called once per frame
    // =================================================================================================================================================

    private void Awake()
    {
        handgun = transform.Find("handgun");
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
        Vector3 theScale = handgun.localScale;
         
        if(movement.x > 0.001f)
        {
            if(facing == -1.0f)     // we have a flip
            {
                LOR = false;
                theScale.x *= -1;
                handgun.localScale = theScale;
                Vector2 diff = new Vector2(0.25f, 0);
                handgun.Translate(diff,Space.World);        // space world == absolute axis    Didn't change with object rotate
            }
            facing = 1.0f;
        }
        else if(movement.x < -0.001f)
        {
            if (facing == 1.0f)     // we have a flip
            {
                LOR = true;
                theScale.x *= -1;
                handgun.localScale = theScale;
                Vector2 diff = new Vector2(-0.25f, 0);
                handgun.Translate(diff, Space.World);
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
        if (proTime - NextTime >= 0.03)
        {
            FollowMouseRotate();
            Shooting();
            NextTime = proTime;
        }
        // =================================================================================================================================================
    }

    // =================================================================  CH update  =======================================================================
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement + moveSpeed * Time.fixedDeltaTime );
        transform.position = transform.position + movement * Time.deltaTime;
    }
    // =================================================================================================================================================

    // =============================================================== Shooting function ===================================================================
    // Below is Gun & Shooting
    private void FollowMouseRotate()
    {
        Vector3 mouse = Input.mousePosition;
        //Debug.Log(mouse);
        Vector3 obj = Camera.main.WorldToScreenPoint(handgun.position);
        Vector3 direction = obj - mouse;
        if (LOR == true)
        { direction = obj - mouse; }
        else { direction = mouse - obj; }
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
    // =================================================================================================================================================


}
