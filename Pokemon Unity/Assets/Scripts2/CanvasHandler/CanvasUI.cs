using System.Collections;
using System.Collections.Generic;

public class CanvasUI : UnityEngine.MonoBehaviour
    {
        private UnityEngine.GameObject _canvasScene;
        private UnityEngine.CanvasGroup _canvasGroup;
        private UnityEngine.Animator _animator;
        //public UICanvas CanvasUITag { get; private set; }

        private bool _isActive
        {
            get
            {
                return _canvasScene.activeSelf;
            }
            set
            {
                _canvasScene.SetActive(value);
            }
        }
        public bool IsActive
        {
            get
            {
                return _animator.GetBool("");
            }
            set
            {
                _animator.SetBool("", value);
            }
        }

        public void Awake()
        {
            _canvasScene = gameObject;
            _animator = GetComponent<UnityEngine.Animator>();
        }

        public void Update()
        {
            //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(""))
            //    _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
            //else _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
    }
