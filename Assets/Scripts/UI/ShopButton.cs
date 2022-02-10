using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    protected Dropdown dropDownBox = null;
    [SerializeField]
    protected MTXItem item = MTXItem.ExtraLasers;
    [SerializeField]
    protected DialogBox confirmationDialog = null;
    [SerializeField]
    protected TimedDialog processingDialog = null;
    [SerializeField]
    protected DialogBox completeDialog = null;
    [SerializeField]
    protected int[] quantities = null;
    [SerializeField]
    private Button button;

    protected DialogBox confirmation;
    protected TimedDialog processing;

    bool inUse = false;
    int amountToPurchase;

    private void HandlePurchase()
    {
        confirmation.OnConfirm -= HandlePurchase;

        processing = Instantiate(processingDialog, processingDialog.transform.position, processingDialog.transform.rotation).GetComponent<TimedDialog>();

        processing.OnFinished += PurchaseComplete;
    }

    private void PurchaseComplete(TimedDialogResult result)
    {
        processing.OnFinished -= PurchaseComplete;

        if (result == TimedDialogResult.Success)
        {
            string key = Enum.GetName(typeof(MTXItem), item);

            int newQuantity = PlayerPrefs.GetInt(key);
            newQuantity += amountToPurchase;

            PlayerPrefs.SetInt(key, newQuantity);

            Instantiate(completeDialog, completeDialog.transform.position, completeDialog.transform.rotation);
        }
    }

    public void Purchase(int amount)
    {
        if (!inUse)
        {
            inUse = true;

            amountToPurchase = amount;
            string itemPurchased = Enum.GetName(typeof(MTXItem), item);

            Analytics.CustomEvent($"MTX Purchase", new Dictionary<string, object>
            {
                {itemPurchased, amount }
            });

            confirmation = Instantiate(confirmationDialog, confirmationDialog.transform.position, confirmationDialog.transform.rotation).GetComponent<DialogBox>();

            confirmation.OnConfirm += HandlePurchase;
        }
    }
}
