using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI<T> : MonoBehaviour where T : MonoBehaviour
{
    private Action onHideCallback;

    #region Events

    public void EventInactive()
    {
        gameObject.SetActive(false);

        onHideCallback?.Invoke();
    }

    #endregion

    public void Show(Action<T> refreshState)
    {
        Refresh(refreshState);

        Show();
    }

    public void Hide(Action onHideCallback)
    {
        this.onHideCallback = onHideCallback;

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Refresh(Action<T> refreshState)
    {
        refreshState.Invoke(this as T);

        OnRefresh();
    }

    protected abstract void OnRefresh();
}
