using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCard : MonoBehaviour
{
    public GameObject CurrentCard;
    public float MoveTime = 0.5f;

    private bool _animationInProgress;
    private float _inverseMoveTime;
    bool _pendingAnimation;
    private float _yInitialPosition;

#if UNITY_ANDROID || UNITY_IOS
	private CalculateDelta CalculateDelta = new CalculateDeltaTouch();
	#else
    private CalculateDelta CalculateDelta = new CalculateDeltaMouse();
#endif


    protected virtual void Start()
    {
        _inverseMoveTime = 1f / MoveTime;
        _yInitialPosition = CurrentCard.transform.position.y;
    }

    void Update()
    {
        if (!_animationInProgress)
        {
            if (CalculateDelta.IsMoving())
            {
                _pendingAnimation = true;
                MoveCard();
            }
            else
            {
                Debug.Log("restoring");
                if (_pendingAnimation)
                {
                    if (CheckRestoreToPosition())
                    {
                        _pendingAnimation = false;
                        _animationInProgress = true;
                        StartCoroutine(SmoothMovement(new Vector3(0, _yInitialPosition, 0),
                            new Quaternion(0, 0, 0, 0)));
                    }
                    else
                    {
                    }
                }
            }
        }
    }

    private void MoveCard()
    {
        CalculateDelta.Update();

        Vector3 diff = CalculateDelta.CalculateDiffInPx();

        Vector3 diffInWorld = CalculateDelta.CalculateDiffInWorld();

        Debug.Log("moving: " + diff.x + " " + diffInWorld.x);

        Vector3 newPos = CalculatePosition(CurrentCard.transform.position, diffInWorld);


        Vector3 cardRotation = Vector3.zero;

        if (!IsCardXLimit(newPos))
        {
            cardRotation = CalculateRotation(diff);
        }

        CurrentCard.transform.position = newPos;
        CurrentCard.transform.Rotate(cardRotation);
    }

    private bool IsCardXLimit(Vector3 newPos)
    {
        return newPos.x == 1.5f || newPos.x == -1.5f;
    }

    private Vector3 CalculatePosition(Vector3 currentPosition, Vector3 diffInWorld)
    {
        Vector3 newPos = new Vector3();
        float newX = currentPosition.x + diffInWorld.x;

        if (newX > 1.5f)
        {
            newX = 1.5f;
        }

        if (newX < -1.5f)
        {
            newX = -1.5f;
        }

        newPos.x = newX;


        float newY = currentPosition.y + diffInWorld.y;
        if (newY > 0.5f)
        {
            newY = 0.5f;
        }

        if (newY < -2.0f)
        {
            newY = -2.0f;
        }

        newPos.y = newY;
        newPos.z = 0;

        return newPos;
    }

    private Vector3 CalculateRotation(Vector3 diff)
    {
        float cardRotation = (diff.x * -90.0f) / Screen.width;
        Vector3 newRotation = new Vector3();
        newRotation.z = cardRotation;
        newRotation.x = 0;
        newRotation.y = 0;
        return newRotation;
    }

    bool CheckRestoreToPosition()
    {
        return CurrentCard.transform.position.x > -0.5f && CurrentCard.transform.position.x < 0.5f;
    }

    protected IEnumerator SmoothMovement(Vector3 end, Quaternion endRotation)
    {
        float sqrRemainingDistance = (CurrentCard.transform.position - end).sqrMagnitude;

        float deltaToRotateZ = CurrentCard.transform.rotation.z / MoveTime;
        Vector3 deltaToRotate = new Vector3(0, 0, -deltaToRotateZ);

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion =
                Vector3.MoveTowards(CurrentCard.transform.position, end, _inverseMoveTime * Time.deltaTime);

            CurrentCard.transform.position = newPostion;
            CurrentCard.transform.Rotate(deltaToRotate);

            sqrRemainingDistance = (CurrentCard.transform.position - end).sqrMagnitude;

            yield return null;
        }

        CurrentCard.transform.rotation = endRotation;
        _animationInProgress = false;
        Debug.Log("finish");
    }
}