using TMPro;
using UnityEngine;
using DG.Tweening;

namespace BricksAndBalls.Views
{
    public class BrickView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshPro _hpText;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _sprite;

        [Header("Effects")]
        [SerializeField] private ParticleSystem _hitEffect;

        [Header("Color Settings")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _hitColor = Color.red;
        [SerializeField] private float _hitFlashDuration = 0.15f;

        [Header("HP Color Gradient")]
        [SerializeField] private Gradient _hpGradient;
        [SerializeField] private bool _useHpGradient = true;

        public int BrickID { get; set; }

        private int _maxHP;
        private Tween _flashTween;

        public void Initialize(int maxHP)
        {
            _maxHP = maxHP;
            ResetVisuals();
            SetText(maxHP);
            SetColor(maxHP);
        }

        public void UpdateHP(int newHP)
        {
            SetText(newHP);
            _hitEffect?.Play();
            PlayHitFlash();
            SetColor(newHP);
        }

        public void PlayDeathAnimation()
        {
            gameObject.SetActive(false);
        }

        public void ResetVisuals()
        {
            _flashTween?.Kill();
            _flashTween = null;

            if (_sprite != null)
            {
                _sprite.color = _normalColor;
            }

            if (_hpText != null)
            {
                _hpText.text = "";
            }

            transform.localScale = Vector3.one;
        }

        private void SetText(int newHP)
        {
            if (_hpText != null)
            {
                _hpText.text = newHP.ToString();
            }
        }

        private void SetColor(int newHP)
        {
            if (_useHpGradient && _hpGradient != null && _maxHP > 0)
            {
                var hpPercent = (float)newHP / _maxHP;
                var targetColor = _hpGradient.Evaluate(hpPercent);
                SetBaseColor(targetColor);
            }
        }

        private void SetBaseColor(Color color)
        {
            if (_sprite != null)
            {
                _sprite.color = color;
            }
        }

        private void PlayHitFlash()
        {
            if (_sprite == null)
            {
                return;
            }

            _flashTween?.Kill();

            // Создаём sequence для плавного перехода туда-обратно
            _flashTween = DOTween.Sequence()
                .Append(_sprite.DOColor(_hitColor, _hitFlashDuration * 0.5f))
                .Append(_sprite.DOColor(_sprite.color, _hitFlashDuration * 0.5f))
                .SetEase(Ease.InOutSine)
                .OnComplete(() => _flashTween = null);
        }

        private void OnDestroy()
        {
            _flashTween?.Kill();
        }
    }
}
