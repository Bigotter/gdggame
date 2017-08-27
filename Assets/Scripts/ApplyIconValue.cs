using System;
using core;
using UnityEngine;

public class ApplyIconValue
{
    private int _previousValue;
    private float _timeAnimColor;
    private Color _colorHighlight = Color.white;
    private int _maxValue;

    public void Apply(GameObject gameObjectIcon, int newValue)
    {
        if (_previousValue != newValue)
        {
            UpdateIcon(gameObjectIcon, newValue);

            _timeAnimColor = 0.5f;
            _previousValue = newValue;
        }

        if (_colorHighlight != Color.white)
        {
            UpdateAnimation(gameObjectIcon);
        }
    }

    private void UpdateAnimation(GameObject gameObjectIcon)
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

    private void UpdateIcon(GameObject gameObjectIcon, int newValue)
    {
        var objectToCrop = gameObjectIcon.transform.Find("mask");
        var totalWidht = objectToCrop.GetComponent<RectTransform>().sizeDelta.y;
        var size = totalWidht * newValue / _maxValue;
        var moveCropper = totalWidht - size;
        objectToCrop.GetComponent<RectTransform>().localPosition = new Vector3(0f, -moveCropper, 0f);
        _colorHighlight = _previousValue > newValue ? Color.red : Color.green;
    }

    public void Init(int initValue, int maxValue, GameObject gameObjectIcon)
    {
        _previousValue = initValue;
        _maxValue = maxValue;
        UpdateIcon(gameObjectIcon, initValue);
    }
}