using System;
using UnityEngine;

public class ResponceCheckingSystem : MonoBehaviour
{
    public static ResponceCheckingSystem Instance{get; private set;}

    public event EventHandler<CheckResponceEventArgs> CheckResponce;
    public class CheckResponceEventArgs : EventArgs
    {
        public int suspectID;
    }
    public event EventHandler HideResponceVerifySystem;

    private void Awake()
    {
        Instance = this;
    }

    public void ReviewResponce(int suspectID)
    {
        HideResponceVerifySystem?.Invoke(this, EventArgs.Empty);
        CheckResponce?.Invoke(this, new CheckResponceEventArgs { 
            suspectID = suspectID
        });
    }
}
