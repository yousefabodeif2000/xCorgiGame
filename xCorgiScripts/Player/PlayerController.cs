using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lightbug.CharacterControllerPro.Core;
//[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;
    public enum PlayerStates
    {
        Moving, Jumping, Emoting, Falling, Hit, Non
    }
    public PlayerStates State;
    [Header("Settings")]
    public bool isActive;
    public bool pauseController;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float groundCheckDistance = 0.1f;
    public float turnSmoothTime = 0.1f;
    public float standupAfter;
    public float emotingTime = 3f;
    public LayerMask groundMask;
    [Header("References")]
    public Animator animator;
    public CharacterActor controller;
    

    [Header("VFX Reference")]
    public GameObject groundedVFX;
    public ParticleSystem moveEffect;



    Vector2 moveDirections = Vector2.zero;
    Vector3 velocity;
    bool isGrounded, isJumping;
    float turnSmoothVelocity;
    float storedSpeed;
    float storedGravity;
    PopupEmote Emoter;



    protected virtual void Awake()
    {
       // Physics.IgnoreLayerCollision(6, 7);
        Instance = this;
    }
    protected virtual void Start()
    {
        //PlayerState = PlayerStates.Non;
        storedSpeed = speed;
        storedGravity = gravity;
        if (moveEffect)
            moveEffect.Play();

        try
        {
            Emoter = GetComponent<PopupEmote>();
        }
        catch
        {
            Debug.LogError("Attach PopupEmote.cs to this object");
        }

    }
    void Emote()
    {
        print("emoting");
        StartCoroutine(Emoting());
    }
    IEnumerator Emoting()
    {
        animator.SetTrigger("Emote");
        PauseController("yes");
        Emoter.ShowEmote(Emoter.EmoteNames[UnityEngine.Random.Range(0, Emoter.EmoteNames.Length)]);
        yield return new WaitForSeconds(emotingTime);
        PauseController("no");
        Emoter.CloseEmote();
    }
    protected virtual void OnEnable()
    {

    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (pauseController)
            speed = 0f;
        else
            speed = storedSpeed;

        if (!isActive)
            return;

        moveDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 dir = new Vector3(moveDirections.x, 0, moveDirections.y).normalized;
        animator.SetFloat("InputX", moveDirections.x);
        animator.SetFloat("InputY", moveDirections.y);
        if (dir.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            animator.SetFloat("Move", speed, 0.06f, Time.deltaTime);
            State = PlayerStates.Moving;
        }


        else if (dir.magnitude < 0.1f)
        {
            animator.SetFloat("Move", 0, 0.06f, Time.deltaTime);
        }

        //jump
        animator.SetBool("Jump", isJumping);
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        //isGrounded = controller.isGrounded;
        if (isGrounded && isJumping)
        {
            GameObject vfx = null;
            if (groundedVFX)
            {
                vfx = Instantiate(groundedVFX, transform.position, Quaternion.identity);
                Destroy(vfx, 4f);
            }
            if (moveEffect)
                moveEffect.Play();

            //StartCoroutine(StandUp());
            isJumping = false;
        }
        if (!isGrounded && !isJumping)
        {
            isJumping = true;
            if (moveEffect)
                moveEffect.Stop();
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangePlayerState(PlayerStates.Emoting);
        }

    }
    public void ChangePlayerState(PlayerStates state)
    {
        State = state;
        switch (state)
        {
            case PlayerStates.Emoting:
                Emote();
                break;
            case PlayerStates.Falling:
                Emote();
                break;
        }
    }
    public void GetsHit()
    {
        StartCoroutine(StandUp());
    }
    IEnumerator StandUp()
    {
        print("Standing up");

        yield return new WaitForSeconds(standupAfter);

    }
    void Jump()
    {
        isJumping = true;
        State = PlayerStates.Jumping;
        //velocity.y = jumpHeight;
        // velocity = new Vector3(0, Mathf.Sqrt(jumpHeight * -2 * gravity), Mathf.Sqrt(jumpHeight * gravity));

        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //velocity.z = Mathf.Sqrt(jumpHeight * gravity);
        //animator.SetTrigger("Jump");
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Danger Zone":
                State = PlayerStates.Falling;
                break;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.transform.tag)
        {
            case "Moveable":
                GameObject obj = hit.gameObject;
                obj.SendMessage("AttachToPlayer", transform);
                break;
        }
    }
    public virtual void PauseController(string state)
    {
        if (state == "yes")
        {
            pauseController = true;
            gravity = 0f;
        }
        if (state == "no")
        {
            pauseController = false;
            gravity = storedGravity;
        }
    }



}
