using core;
using UnityEngine;

public class ApplyIconValue
{
    private int _previousValue;
    private float _timeAnimColor;
    private Color _colorHighlight = Color.white;

    public void Apply(GameObject gameObjectIcon, int happiness)
    {
        if (_previousValue != happiness)
        {
            var objectToCrop = gameObjectIcon.transform.Find("mask");

            var totalWidht = objectToCrop.GetComponent<RectTransform>().sizeDelta.y;
            var size = totalWidht * happiness / ProcessCard.MaxHappiness;
            var moveCropper = totalWidht - size;
            objectToCrop.GetComponent<RectTransform>().localPosition = new Vector3(0f, -moveCropper, 0f);
            _colorHighlight = _previousValue > happiness ? Color.red : Color.green;

            _timeAnimColor = 0.5f;

            _previousValue = happiness;
        }

        if (_colorHighlight != Color.white)
        {
            if (_timeAnimColor > 0)
            {
                var icon = gameObjectIcon.transform.Find("fill");
                var sprite = icon.GetComponent<SpriteRenderer>();
                sprite.color = _colorHighlight;
                _timeAnimColor -= Time.deltaTime;
            }
            else
            {
                _colorHighlight = Color.white;
                var icon = gameObjectIcon.transform.Find("fill");
                var sprite = icon.GetComponent<SpriteRenderer>();
                sprite.color = _colorHighlight;
            }
        }
    }

    public void Init(int initValue)
    {
        _previousValue = initValue;
    }
}