using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using ReceivingSystem.BLL;
using ReceivingSystem.Entities;
using System.Data.Common;

namespace eBikeManagmentWebApp.Components.Pages.Receiving;

public partial class Receiving : ComponentBase
{
    [Inject]
    private IJSRuntime JS { get; set; }
    [Inject]
    private ReceivingService Data { get; set; }

    private bool _isOrderOpen;
    private bool _isUnorderedOpen;
    private bool _isReturnOpen;
    private int returnQty;
    private int unorderedQty;
    private string? unorderedItemDescription;
    private string? unorderedVendorPartNumber;
    private string? returnItemDescription;
    private string? returnVendorPartNumber;
    private string? returnReason;
    private string? forcedCloseReason = "";
    private PurchaseOrder? selectedPO;
    private List<PurchaseOrder>? outstandingOrdersData;
    private List<OrderDetailModel>? orderDetail;
    private List<ReturnedOrderDetail>? returnData;
    private List<UnorderedPurchaseItemCart>? unorderedData;
    private List<PurchaseOrder>? purchaseOrdersList;
    private List<Vendor>? vendorsList;

    protected override void OnInitialized()
    {
        outstandingOrdersData = Data.GetPurchaseOrders();
    }
    public void OrderDetail(PurchaseOrder? id)
    {
        orderDetail = Data.GetPurchaseOrder_DetailsJoinedData(id.PurchaseOrderID);
    }
    public void Unordered()
    {
        unorderedData = Data.GetStoredUnorderedItems();
        ToggleUnorderOpen();
    }
    private async Task Receive(PurchaseOrder? id)
    {
        try
        {
            var purchaseOrder = Data.GetPurchaseOrder(id.PurchaseOrderID);
            var purchaseOrderDetail = Data.GetPurchaseOrderDetails(id.PurchaseOrderID);
            var employeeID = Data.GetEmployeeID(id.PurchaseOrderID);

            var receiveOrder = new ReceiveOrder()
            {
                ReceiveDate = DateTime.Now,
                PurchaseOrderID = id.PurchaseOrderID,
                EmployeeID = employeeID,
            };
            var receiveOrderID = Data.InsertReceiveOrder(receiveOrder);

            if (orderDetail != null)
            {
                bool canClose = false;
                foreach (var receivedItem in orderDetail)
                {
                    if (receivedItem.RecQty <= 0)
                        break;

                    if (receivedItem.RecQty > receivedItem.OQty)
                    {
                        await JS.InvokeAsync<string>("showAlert", "Received qty cannot be larger than ordered quantity.");
                        return;
                    }

                    if (receivedItem.OQty - receivedItem.RecQty <= 0)
                        canClose = true;
                    else
                        canClose = false;

                    var receiveOrderDetails = new ReceiveOrderDetail()
                    {
                        PurchaseOrderDetailID = purchaseOrderDetail.PurchaseOrderDetailID,
                        ReceiveOrderID = receiveOrderID,
                        QuantityReceived = receivedItem.RecQty
                    };
                    var receiveOrderDetailID = Data.InsertReceiveOrderDetails(receiveOrderDetails);
                    purchaseOrderDetail.Quantity--;
                    if (purchaseOrderDetail.Quantity <= 0)
                        purchaseOrderDetail.Quantity = 1;
                    Data.UpdatePurchaseOrderDetail(purchaseOrderDetail);
                    Data.DecreaseOrderedQuantity(receivedItem.PartID, receivedItem.RecQty);
                }
                if (canClose)
                {
                    Data.ClosePurchaseOrder(purchaseOrder.PurchaseOrderID);
                    outstandingOrdersData = Data.GetOutstandingOrders();
                }
            }
            _isOrderOpen = false;
        }
        catch (DbException)
        {
            await JS.InvokeAsync<string>("showAlert", "Database Error!");
        }
    }
    private async Task ForceClose()
    {
        if (string.IsNullOrEmpty(forcedCloseReason))
        {
            await JS.InvokeAsync<string>("showAlert", "Please fill all fields.");
        }
        else
        {
            selectedPO.EmployeeID = Data.GetEmployeeID(selectedPO.PurchaseOrderID);
            selectedPO.Closed = true;
            selectedPO.Notes = forcedCloseReason;
            Data.UpdatePurchaseOrder(selectedPO);
            outstandingOrdersData = Data.GetOutstandingOrders();
            forcedCloseReason = "";
        }
        _isOrderOpen = false;
    }
    private async Task Return(PurchaseOrder? id)
    {
        try
        {
            if (!string.IsNullOrEmpty(returnItemDescription) && 
                !string.IsNullOrEmpty(returnVendorPartNumber) && 
                !string.IsNullOrEmpty(returnReason) &&
                selectedPO != null &&
                returnQty > 0)
            {
                var employeeID = Data.GetEmployeeID(selectedPO.PurchaseOrderID);
                var receiveOrderID = Data.GetLastReceiveOrderID();
                var purchaseOrderDetail = Data.GetPurchaseOrderDetails(selectedPO.PurchaseOrderID);
                var returnDetails = new ReturnedOrderDetail()
                {
                    PurchaseOrderDetailID = purchaseOrderDetail.PurchaseOrderDetailID,
                    ItemDescription = returnItemDescription,
                    Reason = returnReason,
                    Quantity = returnQty,
                    ReceiveOrderID = receiveOrderID,
                };
                Data.InsertReturnDetails(returnDetails);
                await JS.InvokeAsync<string>("showAlert", "Success.");
                ClearReturn();
            }
            else
            {
                await JS.InvokeAsync<string>("showAlert", "Please fill all fields and make sure Qty is greater than 0.");
            }
        }
        catch (DbUpdateException)
        {
            await JS.InvokeAsync<string>("showAlert", "Database Error! Could not log return.");
        }
    }
    private async Task ReceiveUnordered()
    {
        try
        {
            if (!string.IsNullOrEmpty(unorderedItemDescription) && !string.IsNullOrEmpty(unorderedVendorPartNumber))
            {
                var unorderedItemCart = new UnorderedPurchaseItemCart
                {
                    Description = unorderedItemDescription,
                    VendorPartNumber = unorderedVendorPartNumber,
                    Quantity = unorderedQty
                };

                Data.InsertUnorderedItem(unorderedItemCart);

                unorderedData = Data.GetStoredUnorderedItems();
                ClearUnordered();
            }
            else
            {
                await JS.InvokeAsync<string>("showAlert", "Please fill all fields.");
            }
        }
        catch (DbUpdateException)
        {
            await JS.InvokeAsync<string>("showAlert", "Database Error! Could not receive order.");
        }
    }

    private void DeleteUnordered(UnorderedPurchaseItemCart item)
    {
        Data.DeleteUnorderedItem(item);
        unorderedData = Data.GetStoredUnorderedItems();
    }
    private void ClearUnordered()
    {
        unorderedItemDescription = "";
        unorderedVendorPartNumber = "";
        unorderedQty = 0;
    }
    private void ClearReturn()
    {
        returnItemDescription = "";
        returnVendorPartNumber = "";
        returnReason = "";
        returnQty = 0;
    }
    private void TogglePurchaseOrderOpen(PurchaseOrder? oo)
    {
        if (oo == null)
        {
            _isOrderOpen = false;
            return;
        }
        else
        {
            _isOrderOpen = true;
            selectedPO = oo;
            OrderDetail(oo);
        }
    }
    private void ToggleUnorderOpen()
    {
        _isUnorderedOpen = !_isUnorderedOpen;
    }
    private void ToggleReturnOpen()
    {
        _isReturnOpen = !_isReturnOpen;
    }

}
