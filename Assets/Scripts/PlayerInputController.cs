using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float jumpPower = 10f;

    [SerializeField]
    private float gravity = 0.5f;

    [SerializeField]
    private float slideTime = 2f;

    [SerializeField]
    private float slideDistance = 10f;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    [SerializeField]
    private LayerMask groundLayer;

    private Vector2 fingerDown;
    private Vector2 fingerUp;

    private bool detectSwipeOnlyAfterRelease = false;
    
    private bool canRun = true;
    private bool InSlide = false;
    private int PlayerLine = 0;

    private Rigidbody rBody;

    private Vector3 lastPosition = Vector3.zero;

    private float timer = 0;

    private bool Stuck = false;

    private bool OnGround;

    private List<Touch> touches = new();

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer);

        if (!OnGround)
            rBody.velocity += gravity * Vector3.down;

        if (canRun)
        {
            rBody.velocity = new Vector3(speed, rBody.velocity.y, 0);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                SmoothJump();

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                StartCoroutine(SideSlide(1));

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                StartCoroutine(SideSlide(-1));
        }

        CheckIfStuck();

        GetTouches(touches);

        CalculateTouches();
    }

    private void CalculateTouches()
    {
        for (int i = 0; i < touches.Count; i++)
        {
            var touch = touches[i];

            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDown = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                DetectSwipe();
            }
        }
    }

    private IEnumerator SideSlide(int Side)
    {
        if(!InSlide && PlayerLine != Side)
        {
            InSlide = true;
            float time = 0;
            float z_position = rBody.position.z;

            while (time < slideTime)
            {
                rBody.position = Vector3.Lerp(rBody.position, new Vector3(rBody.position.x, rBody.position.y, z_position + slideDistance * Side), time);
                time += Time.deltaTime;

                yield return null;
            }

            InSlide = false;
            PlayerLine += Side;
        }
    }

    private void SmoothJump()
    {
        if (OnGround)
            rBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    private IEnumerator SmoothSlideDown()
    {
        yield return null;
    }
    private float ColculateSpeed()
    {
        float speed = transform.position.x - lastPosition.x;
        lastPosition = transform.position;
        return speed;
    }

    private void CheckIfStuck()
    {
        if (timer > 0.5f)
        {
            speed += 0.05f;
            timer = 0;

            if (ColculateSpeed() < 1 && !Stuck)
            {
                Stuck = true;
                StartCoroutine(DeathTimer());
            }
            else
            {
                Stuck = false;
            }
        }
        timer += Time.deltaTime;
    }
    private IEnumerator DeathTimer()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1);
        }
        if (Stuck)
            Debug.Log("You're stuck!");
        else
            Debug.Log("NoLonger stuck");
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(fingerDown, fingerUp) > minDistanceForSwipe)
        {
            // Swipe direction
            float deltaX = fingerDown.x - fingerUp.x;
            float deltaY = fingerDown.y - fingerUp.y;

            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                if (deltaX > 0)
                {
                    StartCoroutine(SideSlide(-1));
                }
                else if (deltaX < 0)
                {
                    StartCoroutine(SideSlide(1));
                }
            }
            else
            {
                if (deltaY > 0)
                {
                    SmoothJump();
                }
                else if (deltaY < 0)
                {
                    Debug.Log("Swipe Up to Down");
                }
            }

            fingerUp = fingerDown;
        }
    }

    private void GetTouches(List<Touch> touches)
    {
        touches.Clear();

        for (int i = 0; i < Input.touchCount; i++)
        {
            touches.Add(Input.GetTouch(i));
        }
    }

    public IEnumerator SlowDown()
    {
        var currentSpeed = speed;
        speed *= 0.7f;
        yield return new WaitForSeconds(3);
        speed = currentSpeed;
    }

    public void StopPlayerFromMoving()
    {
        canRun = false;
        rBody.velocity = Vector3.zero;
    }
}
