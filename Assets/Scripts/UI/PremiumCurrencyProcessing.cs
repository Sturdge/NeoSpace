using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiumCurrencyProcessing : TimedDialogLogic
{
    public override void PerformLogic()
    {
        Finished(TimedDialogResult.Success);
    }
}
