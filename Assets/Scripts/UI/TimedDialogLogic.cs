using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedDialogLogic : MonoBehaviour
{
    public delegate void ProcessEvent(TimedDialogResult result);
    public event ProcessEvent OnFinished;

    protected void Finished(TimedDialogResult result)
    {
        OnFinished?.Invoke(result);
    }

    public abstract void PerformLogic();
}
