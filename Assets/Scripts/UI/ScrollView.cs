using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private RectTransform _content;

    [SerializeField] private int viewportOffsetValue = -150;

    private TransitionHelper _transitionHelper = new TransitionHelper();

    [Space(10)]
    [Header("Scroll View Events")]
    [SerializeField] private ItemButtonEvent _eventItemOnSelect;
    [SerializeField] private ItemButtonEvent _eventItemOnSubmit;

    private void Start()
    {
        initButtons();
    }

    private void Update()
    {
        if (_transitionHelper.InProgress)
        {
            _transitionHelper.Update();
            _viewport.anchoredPosition = _transitionHelper.PosCurrent;
        }
    }

    public void initButtons()
    {
        ItemButton[] children = _content.transform.GetComponentsInChildren<ItemButton>();

        ItemButton item;

        int _viewportOffset = 0;

        for (int i = 0; i < children.Length; i++)
        {
            item = children[i];

            item.viewportOffset = _viewportOffset;
            _viewportOffset += viewportOffsetValue;

            //add event listeners
            //children[i].OnSelectEvent.AddListener((ItemButton) => { HandleEventItemOnSelect(children[i]); });
            //children[i].OnSubmitEvent.AddListener((ItemButton) => { HandleEventItemOnSubmit(children[i]); });
            //item.OnSelectEvent.AddListener(HandleEventItemOnSelect(item));
        }

        

    }

    private void HandleOnSelect(ItemButton item)
    {
        Debug.Log("AM here" + item.viewportOffset);
        Vector2 positionFrom = _viewport.anchoredPosition;
        positionFrom.x = item.viewportOffset;

        _transitionHelper.TransitionPositionFromTo(_viewport.anchoredPosition, positionFrom, 0.2f);
    }

    private void HandleEventItemOnSubmit(ItemButton item)
    {
        _eventItemOnSubmit.Invoke(item);
    }

    public void HandleEventItemOnSelect(ItemButton item)
    {
        HandleOnSelect(item);
        _eventItemOnSelect.Invoke(item);
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
