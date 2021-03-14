using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StandardMenu : MonoBehaviour
{
    public UnityEvent onShow, onClose;

    protected Animator animator;

    public bool isMenuOpen { get; protected set; }


    protected bool isInitialized;

    public virtual void Initialize()
    {
        if (!isInitialized)
        {
            animator = GetComponent<Animator>();
            isInitialized = true;
        }
    }

    protected void Start()
    {
        Initialize();
    }

    public virtual void Show()
    {
        Initialize();

        onShow?.Invoke();

        gameObject.SetActive(true);
        if (animator != null) { animator.SetTrigger("Show"); }

        isMenuOpen = true;
    }

    public virtual void Hide()
    {
        onClose?.Invoke();

        if (animator == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            animator.SetTrigger("Close");

            //float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            //Invoke(nameof(Deactivate), animationDuration);
        }

        isMenuOpen = false;
    }

    public void ShowOrHide()
    {
        if (isMenuOpen) { Hide(); } else { Show(); }
    }

    //private void Deactivate()
    //{
    //    gameObject.SetActive(false);
    //}
}
