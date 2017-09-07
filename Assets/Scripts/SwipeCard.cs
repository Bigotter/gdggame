using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SwipeCard : MonoBehaviour
{
    public GameObject CurrentCard;
    public float MoveTime = 0.5f;
    public Text CardDescText;
	public Text CardResponse;

#if UNITY_ANDROID || UNITY_IOS
    private CalculateDelta CalculateDelta = new CalculateDeltaTouch();
#else
    private CalculateDelta CalculateDelta = new CalculateDeltaMouse();
#endif

    private bool _animationInProgress;
    private float _inverseMoveTime;
    bool _pendingAnimation;
    private float _yInitialPosition;
    private CardProvider _cardProvider;
    private ProcessCard _porcessCard;

    private Animation currentAnimation = Animation.None;
    private ProcessCard _processCard;

	private string _rightText;
	private string _leftText;

    enum Animation
    {
        None,
        Restore,
        SwipeRight,
        SwipeLeft
    };

    protected virtual void Start()
    {
        _cardProvider = CardProvider.Instance();
        _processCard = ProcessCard.Instance();
        _processCard.Reset();

        _inverseMoveTime = 1f / MoveTime;
        _yInitialPosition = CurrentCard.transform.position.y;
        NextCard(CurrentCard);
    }

    private void NextCard(GameObject currentCard)
    {
		
        var nextCard = _cardProvider.NextCard();

        var guy = currentCard.transform.Find("guy");
        var spriteGuy = guy.GetComponent<SpriteRenderer>();
        var nextImage = nextCard.Image;
        spriteGuy.sprite = Sprite.Create(nextImage,
            new Rect(0, 0, nextImage.width, nextImage.height),
            new Vector2(0.5f, 0.5f));


        var background = currentCard.transform.Find("background");
        var spriteBackground = background.GetComponent<SpriteRenderer>();
        spriteBackground.color = nextCard.Color;

        CardDescText.text = nextCard.Description;
		_rightText = nextCard.RightText;
		_leftText = nextCard.LeftText;

		CardResponse.enabled = false;
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
                if (_pendingAnimation)
                {
                    _pendingAnimation = false;
                    _animationInProgress = true;
                    if (CheckRestoreToPosition())
                    {
                        currentAnimation = Animation.Restore;
                        StartCoroutine(SmoothMovement(new Vector3(0, _yInitialPosition, 0),
                            new Quaternion(0, 0, 0, 0), 1.0f));
                    }
                    else
                    {
                        if (CurrentCard.transform.position.x > 0)
                        {
                            currentAnimation = Animation.SwipeRight;
                            StartCoroutine(SmoothMovement(new Vector3(6.0f, -6.0f, 0),
                                new Quaternion(0, 0, -90f, 0), 2.5f));
                        }
                        else
                        {
                            currentAnimation = Animation.SwipeLeft;
                            StartCoroutine(SmoothMovement(new Vector3(-6.0f, -6.0f, 0),
                                new Quaternion(0, 0, 90f, 0), 2.5f));
                        }
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

        Vector3 newPos = CalculatePosition(CurrentCard.transform.position, diffInWorld);


        Vector3 cardRotation = Vector3.zero;

		if (!IsCardXLimit (newPos)) {
			cardRotation = CalculateRotation (diff);
		} 

		if (!mustShowText (newPos)) {
			CardResponse.enabled = false;
		} 
		else 
		{
			if (!CardResponse.enabled) {
				CardResponse.enabled = true;
				var animName = "";
				if (newPos.x > 0) {
					CardResponse.text = _rightText;
					CardResponse.alignment = TextAnchor.UpperRight;
					animName = "textAnimationMovingRight";
				} else {
					CardResponse.text = _leftText;
					CardResponse.alignment = TextAnchor.UpperLeft;
					animName = "textAnimationMovingLeft";
				}

				var anim = CardResponse.GetComponent<Animator> ();
				anim.Play (animName);
			}
		}

        CurrentCard.transform.position = newPos;
        CurrentCard.transform.Rotate(cardRotation);
    }

	private bool mustShowText(Vector3 newPos)
	{
		return newPos.x > 0.6f || newPos.x < -0.6f;
	}

    private bool IsCardXLimit(Vector3 newPos)
    {
        return newPos.x == 1.0f || newPos.x == -1.0f;
    }

    private Vector3 CalculatePosition(Vector3 currentPosition, Vector3 diffInWorld)
    {
        Vector3 newPos = new Vector3();
        float newX = currentPosition.x + diffInWorld.x;

        if (newX > 1.0f)
        {
            newX = 1.0f;
        }

        if (newX < -1.0f)
        {
            newX = -1.0f;
        }

        newPos.x = newX;


        float newY = currentPosition.y + diffInWorld.y;
        if (newY > -1.0f)
        {
            newY = -1.0f;
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
        return CurrentCard.transform.position.x > -0.5f && CurrentCard.transform.position.x < 0.5f;
    }

    protected IEnumerator SmoothMovement(Vector3 end, Quaternion endRotation, float accel)
    {
        float sqrRemainingDistance = (CurrentCard.transform.position - end).sqrMagnitude;

        float offsetZ = endRotation.z - CurrentCard.transform.rotation.z;
        float deltaToRotateZ = offsetZ / _inverseMoveTime;
        Vector3 deltaToRotate = new Vector3(0, 0, offsetZ);

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion =
                Vector3.MoveTowards(CurrentCard.transform.position, end, _inverseMoveTime * Time.deltaTime * accel);

            CurrentCard.transform.position = newPostion;
            CurrentCard.transform.Rotate(deltaToRotate * Time.deltaTime * accel);

            sqrRemainingDistance = (CurrentCard.transform.position - end).sqrMagnitude;

            yield return null;
        }

        CurrentCard.transform.position = new Vector3(0, _yInitialPosition, 0f);
        CurrentCard.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        if (currentAnimation == Animation.SwipeLeft || currentAnimation == Animation.SwipeRight)
        {
			
			_processCard.ApplyCardEffect(_cardProvider.CurrentCard, 
				currentAnimation == Animation.SwipeLeft);	
			_cardProvider.AddCards (currentAnimation == Animation.SwipeLeft);
            NextCard(CurrentCard);
        }
        _animationInProgress = false;
        currentAnimation = Animation.None;
    }
}