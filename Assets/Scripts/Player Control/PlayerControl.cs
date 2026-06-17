using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 dir;
    public float Speed = 15;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Sprite[] PlayerSprites;
    [SerializeField] private SpriteRenderer playerSR;
    private string currentAnimation = "";

    void FixedUpdate()
    {
        if (!DialogueUI.Instance.isInDialogue) // && !ActivateBoard.MatchThreeStarted.Invoke() && !DayUI.IsDayChanging() && !ToggleMenu.Instance.isOpen && !GetPlayerName.Instance.isActive)
        {
           MoveFunction();
        }
        //MoveFunction();
    }

    void MoveFunction()
    {
        dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(dir * Speed * Time.deltaTime);
        //CheckAnimation();
    }

    private void CheckAnimation()
    {
        //Movement: WASD or Arrow Keys
        if (dir.y == 1)
        {
            //Move Forward (up) Animation
            ChangeSpriteDirection(PlayerSprites[0]);
            //ChangeAnimation("Walk");
        }
        else if (dir.y == -1)
        {
            //Move Backwards (down) Animation
            ChangeSpriteDirection(PlayerSprites[1]);
            //ChangeAnimation("Walk");
        }
        else if (dir.x == 1)
        {
            //Move Right Animation
            ChangeSpriteDirection(PlayerSprites[2]);
            //ChangeAnimation("Walk");
        }
        else if (dir.x == -1)
        {
            //Move Left Animation
            ChangeSpriteDirection(PlayerSprites[3]);
            //ChangeAnimation("Walk");
        }
        else
        {
            //Idle Animation
            //ChangeAnimation("Idle");
        }

    }

    private void ChangeAnimation(string animation, float crossfade = 0.2f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            playerAnimator.CrossFade(animation, crossfade);
        }
    }

    private void ChangeSpriteDirection(Sprite playerSprite)
    {
        playerSR.sprite = playerSprite;
    }
}
