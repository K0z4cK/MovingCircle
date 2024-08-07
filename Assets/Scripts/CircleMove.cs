using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleMove : MonoBehaviour
{
    [SerializeField]
    private Scrollbar _speedBar;

    [SerializeField]
    private float _baseSpeed = 5;

    private List<Vector2> _path = new List<Vector2>();
    private bool _isMoving = false;

    private Tween _moveTween;

    private void Awake()
    {
        _speedBar.value = 0;
        _speedBar.onValueChanged.AddListener(delegate { SetSpeed(_speedBar.value); });
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _path.Add(mousePosition);
        }

        if(!_isMoving && _path.Count > 0)                  
            StartCoroutine(MoveCoroutine(_path[0]));
        
    }

    private IEnumerator MoveCoroutine(Vector2 target)
    {
        _isMoving = true;
        _moveTween = transform.DOMove(_path[0], Vector2.Distance(transform.position, _path[0]) / _baseSpeed).SetEase(Ease.Linear);
        SetSpeed(_speedBar.value);
        yield return _moveTween.WaitForCompletion();

        _path.Remove(target);
        _isMoving = false;
    }

    private void SetSpeed(float value)
    {
        if(_moveTween != null)
            _moveTween.timeScale = Mathf.Clamp(value, 0.1f, 1);
    }

    private bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject();
}
