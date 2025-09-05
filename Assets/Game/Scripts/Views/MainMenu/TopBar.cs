using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Currency;
using DG.Tweening;

namespace Game.Views
{
    public class TopBar : MonoBehaviour
    {
        public static TopBar Instance { get; private set; }
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private RectTransform goldIcon; 
        [SerializeField] private Image coinPrefab;        
        [SerializeField] private int coinsPerSpend = 3;
        [SerializeField] private float flyDuration = 0.5f;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.Subscribe<GoldChangedEvent>(OnGoldChangedEvent);
            if (GoldManager.Instance != null)
            {
                HandleGoldChanged(GoldManager.Instance.Gold);
            }
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<GoldChangedEvent>(OnGoldChangedEvent);
        }

        private void HandleGoldChanged(int value)
        {
            if (goldText != null)
                goldText.text = value.ToString();
        }

        private void OnGoldChangedEvent(GoldChangedEvent e)
        {
            HandleGoldChanged(e.NewValue);
        }

        public void PlaySpendEffectTo(RectTransform target, Action onComplete)
        {
            if (!isActiveAndEnabled || target == null || goldIcon == null || coinPrefab == null)
            {
                onComplete?.Invoke();
                return;
            }

            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                onComplete?.Invoke();
                return;
            }

            var canvasRect = canvas.transform as RectTransform;
            Vector3 start = goldIcon.position;
            Vector3 end = target.position;

            int coinCount = Mathf.Max(1, coinsPerSpend);
            int completed = 0;

            for (int i = 0; i < coinCount; i++)
            {
                var img = Instantiate(coinPrefab, canvasRect);
                img.raycastTarget = false;
                var rt = img.rectTransform;
                rt.position = start;
                rt.localScale = Vector3.one * 0.5f;
                var delay = i * 0.03f; 
                var seq = DOTween.Sequence().SetUpdate(true);
                seq.AppendInterval(delay);
                seq.Append(rt.DOMove(end, flyDuration).SetEase(Ease.InOutQuad));
                seq.OnComplete(() =>
                {
                    if (img != null) Destroy(img.gameObject);
                    completed++;
                    if (completed >= coinCount)
                        onComplete?.Invoke();
                });
            }
        }
    }
}
