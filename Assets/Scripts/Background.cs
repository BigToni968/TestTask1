using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField] private float _fade = 1f;
    [SerializeField] private float _timening = 5f;
    [SerializeField] private List<Sprite> _sprites = null;

    private Image _bg = null;

    private Singleton _singleton = Singleton.GetInstance();

    private Sequence _sequence = null;
    private int _currentBg = 0;

    private void Awake()
    {
        _bg ??= GetComponentInChildren<Image>();
        if (_sprites.Count > 0)
            _bg.sprite = _sprites[_currentBg = Random.Range(0, _sprites.Count)];

    }

    private void Start() => BackgroundAnimation();

    private void BackgroundAnimation()
    {
        if (_singleton.Data.IsAnimationBackground)
        {
            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(_timening);
            _sequence.Append(_bg.DOFade(0, _fade)).AppendCallback(() => BackgroundReplice());
            _sequence.Append(_bg.DOFade(1, _fade)).AppendCallback(() => BackgroundAnimation());
            _sequence.Play();
        }
    }

    private void BackgroundReplice()
    {
        if (_currentBg < _sprites.Count - 1) _currentBg++;
        else _currentBg = 0;

        _bg.sprite = _sprites[_currentBg];
    }

    private void OnDestroy() => Stop();

    public void Stop() => _sequence.Kill();

}
