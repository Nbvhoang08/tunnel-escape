using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    // Danh sách các Observer đã đăng ký
    private List<IObserver> observers = new List<IObserver>();
    // Thêm một Observer vào danh sách
    public void RegisterObserver(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    // Gỡ bỏ một Observer khỏi danh sách
    public void UnregisterObserver(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    // Thông báo sự kiện đến tất cả Observers
    public void NotifyObservers(string eventName, object eventData = null)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(eventName, eventData);
        }
    }
}
