using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement; //물리법칙 적용 변수
    Animator anim;  //애니메이션 적용변수
    Rigidbody playerRigidbody;  //물리법칙 적용 변수
    int floorMask;  //바닥 마스크
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent <Animator> ();
        playerRigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate ()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move (float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //카메라에서 마우스 포인트를 따라 레이를 발사 함
        RaycastHit floorHit;
        //발사된 레이가 바닥에 충돌시 바닥의 좌표를 저장
        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask ))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);

        }
    }

    void Animating(float h, float v)
    {
        bool walking =( h != 0f || v != 0f );
        anim.SetBool("IsWalking", walking);        
    }
}
