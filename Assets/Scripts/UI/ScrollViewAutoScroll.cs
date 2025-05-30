using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAutoScroll : MonoBehaviour
{
    [SerializeField] private RectTransform _viewportRectTransform;
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _transitionDuration = 0.2f;

    private TransitionHelper _transitionHelper = new TransitionHelper();

    public float viewportTopBorderY;
    public float viewportBottomBorderY;
    public float targetTopBorderY;
    public float targetTopWithViewportOffset;
    public float targetBottomBorderY;
    public float targetBottomWithViewportOffset;
    public float topDiff;
    public float bottomDiff;

    private void Update()
    {
        if (_transitionHelper.InProgress)
        {
            _transitionHelper.Update();
            _content.transform.localPosition = _transitionHelper.PosCurrent;
        }
    }

    public void HandleOnSelectChange(GameObject gObj)
    {
        Debug.Log("Am I here?");

        viewportTopBorderY = GetBorderTopYLocal(_viewportRectTransform.gameObject);
        viewportBottomBorderY = GetBorderBottomYLocal(_viewportRectTransform.gameObject);

        //top
        targetTopBorderY = GetBorderTopYRelative(gObj);
        targetTopWithViewportOffset = targetTopBorderY + viewportTopBorderY;

        //bottom
        targetBottomBorderY = GetBorderBottomYRelative(gObj);
        targetBottomWithViewportOffset = targetBottomBorderY - viewportBottomBorderY;

        //topDiff
        topDiff = targetTopWithViewportOffset - viewportTopBorderY;
        Debug.Log($"top diff {topDiff}");
        if (topDiff > 0)
        {
            MoveContentObjectYByAmount((topDiff * 100) + GetVerticalLayoutGroup().padding.top);
        }

        //bottomDiff
        bottomDiff = targetBottomWithViewportOffset - viewportBottomBorderY;
        Debug.Log($"bottom diff {bottomDiff}");
        if (bottomDiff > 0)
        {
            MoveContentObjectYByAmount((bottomDiff * 100) - GetVerticalLayoutGroup().padding.bottom);
        }

    }

    private float GetBorderTopYLocal(GameObject gObj)
    {
        Vector3 pos = gObj.transform.position / 100f;

        return pos.y;
    }

    private float GetBorderBottomYLocal(GameObject gObj)
    {
        Vector2 rectSize = gObj.GetComponent<RectTransform>().rect.size * 0.01f;
        Vector3 pos = gObj.transform.position / 100f;

        pos.y -= rectSize.y;
        return pos.y;
    }

    public float GetBorderTopYRelative(GameObject gObj)
    {
        float contentY = _content.transform.position.y / 100f;
        float targetBorderUpYLocal = GetBorderTopYLocal(gObj);
        float targetBorderUpRelative = targetBorderUpYLocal + contentY;

        return targetBorderUpRelative;
    }

    public float GetBorderBottomYRelative(GameObject gObj)
    {
        float contentY = _content.transform.position.y / 100f;
        float targetBorderBottomYLocal = GetBorderBottomYLocal(gObj);
        float targetBorderBottomRelative = targetBorderBottomYLocal + contentY;

        return targetBorderBottomRelative;
    }

    private void MoveContentObjectYByAmount(float  amount)
    {
        Vector2 posScrollFrom = _content.transform.position;
        Vector2 posScrollTo = posScrollFrom;
        posScrollTo.y += amount;

        _transitionHelper.TransitionPositionFromTo(posScrollTo, posScrollFrom, _transitionDuration);
    }

    private VerticalLayoutGroup GetVerticalLayoutGroup()
    {
        VerticalLayoutGroup verticalLayoutGroup = _content.GetComponent<VerticalLayoutGroup>();
        return verticalLayoutGroup;   
    }

    private class TransitionHelper
    {
        private float _duration = 0f; //the tottal time that this transtion will be completed in, from start to finish
        private float _timeElapsed = 0f; //keep track of time
        private float _progress = 0f; //total progress from start to finish values 0 - 1

        private bool _inProgress = false;

        private Vector2 _posCurrent;
        private Vector2 _posFrom;
        private Vector2 _posTo;

        public bool InProgress { get => _inProgress; }
        public Vector2 PosCurrent { get => _posCurrent; }

        public void Update()
        {
            Tick();

            CalculatePosition();
        }

        public void Clear()
        {
            _duration = 0f;
            _timeElapsed = 0f;
            _progress = 0f;

            _inProgress = false;
        }

        public void TransitionPositionFromTo(Vector2 posFrom, Vector2 posTo, float duration)
        {
            Clear();

            _posFrom = posFrom;
            _posTo = posTo;
            _duration = duration;

            _inProgress = true;
        }

        private void CalculatePosition()
        {
            _posCurrent.x = Mathf.Lerp(_posFrom.x, _posTo.x, _progress);
            _posCurrent.y = Mathf.Lerp(_posFrom.y, _posTo.y, _progress);
        }

        private void Tick()
        {
            if (_inProgress == false)
            {
                return;
            }

            _timeElapsed += Time.deltaTime;
            _progress = _timeElapsed / _duration;

            if (_progress > 1f)
            {
                _progress = 1f;
            }

            if (_progress >= 1f)
            {
                TransitionComplete();
            }
        }

        private void TransitionComplete()
        {
            _inProgress = false;
        }
    }

}
