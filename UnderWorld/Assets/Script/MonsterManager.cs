using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터의 AI 스크립트

public class MonsterManager : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    // 현재 애니메이션 처리가 무엇인지에 대한 변수
    private AnimState _AnimState;

    // 현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    private string CurrentAnimation;

    public enum AnimState
    {
        Idle, Run
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Think();

        Invoke(nameof(Think), 2);
    }


    void FixedUpdate()
    {
        if(nextMove == 0)
            _AnimState = AnimState.Idle;
        else
            _AnimState = AnimState.Run;


        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

        SetCurrentAnimation(_AnimState);
    }

    //재귀 함수
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        //anim.SetInteger("WalkSpeed", nextMove);
        //Flip Sprite
        if (nextMove != 0)
        {
            if (nextMove == 1)
            {
                skeletonAnimation.skeleton.ScaleX = Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
            }
            else if(nextMove == -1)
            {
                skeletonAnimation.skeleton.ScaleX = -Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
            }
        }

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke(nameof(Think), nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        //spriteRenderer.flipX = nextMove == 1;
        if (nextMove == 1)
        {
            skeletonAnimation.skeleton.ScaleX = Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
        }
        else if (nextMove == -1)
        {
            skeletonAnimation.skeleton.ScaleX = -Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
        }
        CancelInvoke();
        Invoke(nameof(Think), 2);
    }

    private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        // 동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if (animClip.name.Equals(CurrentAnimation))
            return;

        // 해당 애니메이션으로 변경
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        // 현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    }
}