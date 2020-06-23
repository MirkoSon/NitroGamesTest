using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Logics of interaction with cards, their appearence, activation and deactivation.
/// </summary>
public class CardBehaviour : MonoBehaviour, IPointerClickHandler
{
    private Animator _animator;
    private CardEntity _entity;
    private Image _image;

    private bool _isFacingUp;

    public static Action<int> OnCardFlipped;

    private PairChecker _pairChecker;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _pairChecker = dependencyContainer.Resolve<PairChecker>();
        _animator = GetComponent<Animator>();
        _entity = GetComponent<CardEntity>();
        _image = GetComponent<Image>();
        _pairChecker.OnPairEvaluated += RemoveOrFlip;
    }

    public void OnDestroy()
    {
        _pairChecker.OnPairEvaluated -= RemoveOrFlip;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isFacingUp)
        {
            OnCardFlipped?.Invoke(_entity.Id);
            _animator.SetTrigger("Flip");
            _isFacingUp = true;
        }
    }

    public void RemoveOrFlip(bool isCorrect, int requestedId)
    {
        if (_isFacingUp)
        {
            if (isCorrect && requestedId == _entity.Id)
            {
                DeactivateCard();
            }
            else
            {
                _animator.SetTrigger("Flip");
                _isFacingUp = false;
            }
        }
    }

    public void DeactivateCard()
    {
        _image.raycastTarget = false;
        _entity.emoteImage.raycastTarget = false;
        _pairChecker.OnPairEvaluated -= RemoveOrFlip;
    }
}
