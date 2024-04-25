
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SalesAndReturnSystem.BLL;
using SalesAndReturnSystem.Entities;
using SalesAndReturnSystem.ViewModels;

namespace eBikeManagmentWebApp.Components.Pages.SalesAndReturns;

public partial class Returns: ComponentBase
{
    [Inject]
    public IJSRuntime JS { get; set; }
    [Inject]
    public SalesService Data { get; set; }

    private int currentCount = 0;
    private int value = 0;
    private int saleInvoiceID;
    private int returnID = 0;
    private decimal subtotal = 0;
    private decimal taxPct = 0;
    private decimal total = 0;
    private decimal totalSum;
    private decimal tax = 0;
    private decimal discount = 0;
    private List<ReturnData> returnItems = new();
    private Sale sale;

    private void LookupSale()
    {
        returnItems = new();
        subtotal = returnItems.Sum(item => item.Price);
        var invoiceItem = Data.GetInvoiceItems(saleInvoiceID);
        if (invoiceItem.Count() >= 1)
        {
            invoiceItem.ForEach((item) =>
            {
                var part = Data.GetPart(item.PartID);
                var returnItem = new ReturnData
                {
                    Item = part.Description ?? "",
                    OrgQty = item.Quantity,
                    Price = (decimal)item.SellingPrice,
                    RtnQty = 0,
                    Ref = "0",
                    Qty = item.Quantity,
                    Reason = "",
                    Part = part
                };

                returnItems.Add(returnItem);
            });
        }
        UpdateTable();
    }
    private void ChangeRefundStatus(ReturnData returnItem)
    {
        if (returnItem.Ref == "0")
            returnItem.Ref = "1";
        else if (returnItem.Ref == "1")
            returnItem.Ref = "0";
        UpdateTable();
    }
    private async Task UpdateTable()
    {
        subtotal = 0;
        Sale? sale = null;
        try
        {
            sale = Data.GetSale(saleInvoiceID);
        }
        catch (Exception)
        {
            await JS.InvokeAsync<string>("showAlert", "Could not find sales ID");
        }
        var appliedCouponId = Data.GetCouponID(saleInvoiceID);
        if (appliedCouponId != 0 && appliedCouponId != null)
        {
            var coupon = Data.GetCouponByID(appliedCouponId);
            if (sale != null)
            {
                tax = Math.Round((decimal)(100 * (sale.TaxAmount / (sale.SubTotal + sale.TaxAmount))), 2);
                discount = Math.Round((decimal)coupon.CouponDiscount, 2);
            }
        }
        foreach (var item in returnItems.Where(i => i.Ref == "1"))
        {
            subtotal += Math.Round((item.RtnQty * item.Price), 2);
        }
        total = Math.Round(subtotal + (subtotal * ((tax - discount) / 100)), 2);
    }
    private async Task Refund()
    {
        try
        {
            var refund = new SaleRefund()
            {
                SubTotal = subtotal,
                SaleRefundDate = DateTime.Now,
                SaleID = saleInvoiceID,
                TaxAmount = tax,
                EmployeeID = 1
            };

            int returnID = Data.InsertRefund(refund);

            foreach (var item in returnItems.Where(i => i.Ref == "1"))
            {
                if (item.RtnQty == 0)
                {
                    await JS.InvokeAsync<string>("showAlert", "Return qty must be greater than 0.");
                    return;
                }
                if (item.RtnQty > item.OrgQty)
                {
                    await JS.InvokeAsync<string>("showAlert", "Cannot return more than purchased.");
                    return;
                }
                if (string.IsNullOrEmpty(item.Reason))
                {
                    await JS.InvokeAsync<string>("showAlert", "All returns must have a reason.");
                    return;
                }
                UpdateSaleDetail(item);
                var refundDetail = new SaleRefundDetail()
                {
                    SaleRefundID = returnID,
                    Quantity = item.RtnQty,
                    Reason = item.Reason,
                    PartID = item.Part.PartID

                };

                Data.InsertRefundDetail(refundDetail);
            }
            await JS.InvokeAsync<string>("showAlert", "Success!");
        }
        catch (DbUpdateException ex)
        {
            await JS.InvokeAsync<string>("showAlert", "Database error! Could save refund refund.");
        }
    }
    private void UpdateSaleDetail(ReturnData returnItem)
    {
        var newQty = returnItem.Qty - returnItem.RtnQty;
        if (newQty > 0)
        {
            foreach (var item in Data.GetSaleDetails(saleInvoiceID))
            {
                if (item.PartID == returnItem.Part.PartID)
                {
                    item.Quantity = newQty;
                }
            }
        }
        else
        {
            var detailpart = Data.GetSaleDetailByReturnItem(returnItem.Part.PartID, saleInvoiceID);
            Data.DeleteSaleDetail(detailpart);
        }
    }
    private void ClearTable()
    {
        returnItems.Clear();
        subtotal = tax = discount = total = 0;
    }
    private void ClearSaleInvoice()
    {
        saleInvoiceID = 0;
        ClearTable();
    }
}
