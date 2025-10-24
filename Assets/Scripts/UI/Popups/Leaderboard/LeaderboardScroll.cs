using System.Collections.Generic;
using BricksAndBalls.Services.Leaderboard;
using UnityEngine;

namespace BricksAndBalls.UI.Popups.Leaderboard
{
    public class LeaderboardScroll : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform content;
        [SerializeField] private PlayerItemView itemPrefab;

        [Header("Settings")]
        [SerializeField] private int visibleCount = 8;
        [SerializeField] private float itemHeight = 100f;
        [SerializeField] private float spacing = 10f;
        [SerializeField] private float lerpSpeed = 10f;
        [SerializeField] private float swipeSensitivity = 1f;

        private List<LeaderboardEntry> _data;
        private readonly List<PlayerItemView> _visibleItems = new();
        private float _scrollPos;
        private float _currentContentY;
        private int _firstVisibleIndex;

        
        private Vector2 _touchStartPos;
        private bool _isDragging;

        public void SetData(List<LeaderboardEntry> data)
        {
            _data = data;
            _scrollPos = 0;
            _currentContentY = 0;
            _firstVisibleIndex = 0;
            
            InitPool();
            UpdateVisibleItems();
            ResizeContent();
        }

        private void InitPool()
        {
            foreach (var item in _visibleItems)
                Destroy(item.gameObject);
            _visibleItems.Clear();

            for (var i = 0; i < visibleCount; i++)
            {
                var item = Instantiate(itemPrefab, content);
                _visibleItems.Add(item);
            }
        }

        private void ResizeContent()
        {
            var totalHeight = _data.Count * (itemHeight + spacing);
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
        }

        private void Update()
        {
            if (_data == null || _data.Count == 0)
            {
                return;
            }

            HandleInput();
            UpdateContentPosition();
            UpdateVisibleItemsIfNeeded();
        }

        private void HandleInput()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _scrollPos -= Input.mouseScrollDelta.y * (itemHeight + spacing) * 0.5f;
                ClampScrollPos();
            }

            if (Input.GetMouseButtonDown(0))
            {
                _touchStartPos = Input.mousePosition;
                _isDragging = true;
            }

            if (_isDragging && Input.GetMouseButton(0))
            {
                Vector2 currentPos = Input.mousePosition;
                var delta = (currentPos.y - _touchStartPos.y) * swipeSensitivity;
                
                _scrollPos += delta;
                ClampScrollPos();
                
                _touchStartPos = currentPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _touchStartPos = touch.position;
                    _isDragging = true;
                }
                else if (touch.phase == TouchPhase.Moved && _isDragging)
                {
                    var delta = (touch.position.y - _touchStartPos.y) * swipeSensitivity;
                    _scrollPos += delta;
                    ClampScrollPos();
                    _touchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isDragging = false;
                }
            }
        }

        private void ClampScrollPos()
        {
            var maxScroll = Mathf.Max(0, _data.Count - visibleCount) * (itemHeight + spacing);
            _scrollPos = Mathf.Clamp(_scrollPos, 0, maxScroll);
        }

        private void UpdateContentPosition()
        {
            _currentContentY = Mathf.Lerp(_currentContentY, _scrollPos, Time.deltaTime * lerpSpeed);
            content.anchoredPosition = new Vector2(0, _currentContentY);
        }

        private void UpdateVisibleItemsIfNeeded()
        {
            var newFirstIndex = Mathf.FloorToInt(_currentContentY / (itemHeight + spacing));
            
            if (newFirstIndex != _firstVisibleIndex)
            {
                _firstVisibleIndex = newFirstIndex;
                UpdateVisibleItems();
            }
        }

        private void UpdateVisibleItems()
        {
            for (var i = 0; i < _visibleItems.Count; i++)
            {
                var dataIndex = _firstVisibleIndex + i;
                var item = _visibleItems[i];

                if (dataIndex < _data.Count)
                {
                    item.gameObject.SetActive(true);
                    
                    var rank = dataIndex + 1;
                    item.SetData(_data[dataIndex], rank);

                    var yPos = -dataIndex * (itemHeight + spacing);
                    item.Rect.anchoredPosition = new Vector2(0, yPos);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}