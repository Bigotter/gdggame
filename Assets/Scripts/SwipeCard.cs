using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCard : MonoBehaviour
{
    public GameObject currentCard;
    public float moveTime = 0.5f;

    private bool animationInProgress;
    private float inverseMoveTime;
    bool pendingAnimation;

#if UNITY_ANDROID || UNITY_IOS
	private CalculateDelta CalculateDelta = new CalculateDeltaTouch();
	#else
    private CalculateDelta CalculateDelta = new CalculateDeltaMouse();
#endif


    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
    }

    void Update()
    {
        if (!animationInProgress)
        {
            if (CalculateDelta.IsMoving())
            {
                pendingAnimation = true;
                MoveCard();
            }
            else
            {
                Debug.Log("restoring");
                if (CheckRestoreToPosition())
                {
                    if (pendingAnimation)
                    {
                        pendingAnimation = false;
                        animationInProgress = true;
                        StartCoroutine(SmoothMovement(new Vector3(0, -1.5f, 0), new Quaternion(0, 0, 0, 0)));
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

        Vector3 newPos = CalculatePosition(currentCard.transform.position, diffInWorld);


        Vector3 cardRotation = Vector3.zero;

        if (!IsCardXLimit(newPos))
        {
            cardRotation = CalculateRotation(diff);
        }

        currentCard.transform.position = newPos;
        currentCard.transform.Rotate(cardRotation);
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

        if (newY < -1.5f)
        {
            newY = -1.5f;
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
        return currentCard.transform.position.x > -0.5f && currentCard.transform.position.x < 0.5f;
    }

    protected IEnumerator SmoothMovement(Vector3 end, Quaternion endRotation)
    {
        float sqrRemainingDistance = (currentCard.transform.position - end).sqrMagnitude;

        float deltaToRotateZ = currentCard.transform.rotation.z / moveTime;
        Vector3 deltaToRotate = new Vector3(0, 0, -deltaToRotateZ);

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion =
                Vector3.MoveTowards(currentCard.transform.position, end, inverseMoveTime * Time.deltaTime);

            currentCard.transform.position = newPostion;
            currentCard.transform.Rotate(deltaToRotate);

            sqrRemainingDistance = (currentCard.transform.position - end).sqrMagnitude;

            yield return null;
        }

        currentCard.transform.rotation = endRotation;
        animationInProgress = false;
        Debug.Log("finish");
    }
}