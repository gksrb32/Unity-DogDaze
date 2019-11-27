using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player")]
    public float playerSpeed = 10;
    private Vector2 moveVelocity;
    private Vector2 moveInput;
    public Animator animator;

    Rigidbody2D playerBody;
    SpriteRenderer playerSprite;
    public GameMaster gameMaster;

    [Header("Items")] //From here
    private Vector3 target; //To here
    public Rigidbody2D[] Items;
    Rigidbody2D currentItem;
    public int nyumBall = 2;
    private GameObject[] getCount;

    [Header("Sounds")]
    public GameObject akMusicPlayer;
    public AK.Wwise.Event waterSpray;
    public AK.Wwise.Event tailWag;
    public AK.Wwise.Event soccerBall;
    public AK.Wwise.Event roombaSound;
    public AK.Wwise.Switch Junkyard;
    public AK.Wwise.Switch Street;
    public AK.Wwise.Switch room_1;
    public AK.Wwise.Switch room_2;


    private void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        gameMaster = FindObjectsOfType<GameMaster>()[0];
        // playerSprite = GetComponent<SpriteRenderer>();
        //target = projectile.transform.position; // here
        
    }

    private void Update()
    {
        animator.SetFloat("Speed", moveInput.magnitude * playerSpeed);
        PlayerInput();
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void FixedUpdate()
    {
        if (!gameMaster.doingSetup)
	    {
		    MovePlayer();
		    Fire();
		}
	}

    public void MovePlayer()
    {
        playerBody.MovePosition(playerBody.position + moveVelocity * Time.fixedDeltaTime);
    }


	private void Fire()
    {
        // Key 2. Spray
        if (Input.GetMouseButtonDown(0) && currentItem == Items[0])
        {
            waterSpray.Post(gameObject); //play sound
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Rigidbody2D newCurrentItem;

            var heading = target - transform.position;
            var distance = heading.magnitude;
            var direction = heading * 2 / distance;

            newCurrentItem = Instantiate(currentItem, transform.position, Quaternion.identity);
            target.z = newCurrentItem.transform.position.z;
            newCurrentItem.velocity = newCurrentItem.transform.TransformDirection(direction);
            Destroy(newCurrentItem.gameObject, 2.0f);
        }

        // Key 3. Soccer Ball
        else if (Input.GetMouseButtonDown(0) && currentItem == Items[1])
        {
            getCount = GameObject.FindGameObjectsWithTag("Balls");
            int count = getCount.Length;

            if (count < 2)
            {
                // Here Soccer Ball Sound
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Rigidbody2D newCurrentItem;

                var heading = target - transform.position;
                var distance = heading.magnitude;
                var direction = heading * 3 / distance;

                newCurrentItem = Instantiate(currentItem, transform.position + direction, Quaternion.identity);
                target.z = newCurrentItem.transform.position.z;
                newCurrentItem.velocity = newCurrentItem.transform.TransformDirection(target - transform.position);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Balls")
        {
            Destroy(collision.gameObject);
        }
    }



    public void PlayerInput()
    {
        // PLAYER MOVEMENT
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(hAxis, vAxis);
       
        moveVelocity = moveInput.normalized * playerSpeed;


        // ITEM INPUT
        int option = ItemOption();

        if (option == 0) {// Booster - Spacebar
            Booster();
            Invoke("NormalSpeed", 1f);
        } else if (option == 1) {// Tail Wagging - Alpha1
            animator.SetBool("IsWag", true);
            IsWagging();
        } else if (option == 2){// Spray
            animator.SetBool("IsKick", true);
            currentItem = Items[option - 2];
        } else if (option == 3){// Soccer Ball
            currentItem = Items[option - 2];
        }
    }

    private void IsWagging()
    {
        tailWag.Post(gameObject);//play sound
        Invoke("StopWagging", 3f);
    }
    private void StopWagging(){animator.SetBool("IsWag", false);}
    private void NormalSpeed(){playerSpeed = 10;}
    private void Booster(){playerSpeed = 20;}

    public int ItemOption()
    {
        if (Input.GetKeyDown(KeyCode.Space)){return 0;} // Booster
        else if (Input.GetKeyDown(KeyCode.Alpha1)){return 1;} // Tail Wagging
        else if (Input.GetKeyDown(KeyCode.Alpha2)){return 2;} // Spray
        else if (Input.GetKeyDown(KeyCode.Alpha3)){return 3;} // Soccer Ball
        else {return 5;}
    }

    void OnTriggerStay2D(Collider2D c)
	{
        if (c.tag == "SpecialItem" && Input.GetKey(KeyCode.C))
		{
			// TODO: save the item under the player
			gameMaster.UpdateCanvas("Street", true, -1);
            gameMaster.SetCurrentGameState(GameMaster.game_state_start_loading_level);
            Street.SetValue(akMusicPlayer);//music changes
            gameMaster.EnterScene("street");
        }
	}
}

