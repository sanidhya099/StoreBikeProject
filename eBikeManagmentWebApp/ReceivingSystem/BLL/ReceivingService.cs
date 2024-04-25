using ReceivingSystem.DAL;
using ReceivingSystem.Entities;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReceivingSystem.BLL;
public class ReceivingService
{
    private readonly ReceivingDataContext Repository;
    public ReceivingService(ReceivingDataContext WFS_2590Context)
    {
        Repository = WFS_2590Context;
    }

    public List<PurchaseOrder> GetPurchaseOrders()
    {
        return GetPurchaseOrder_VendorsJoinedData();
    }

    public List<PurchaseOrder> GetPurchaseOrder_VendorsJoinedData()
    {
        try
        {
            var joinedData = from po in Repository.PurchaseOrders
                             join vendor in Repository.Vendors on po.VendorID equals vendor.VendorID
                             where po.Closed == false
                             select new PurchaseOrder
                             {
                                 PurchaseOrderID = po.PurchaseOrderID,
                                 OrderDate = po.OrderDate.HasValue ? po.OrderDate : default,
                                 Vendor = vendor,
                             };
            return joinedData.ToList(); ;
        }
        catch (DbException ex)
        {
            throw;
        }
    }

    public List<OrderDetailModel> GetPurchaseOrder_DetailsJoinedData(int id)
    {
        try
        {
            var joinedData = from parts in Repository.Parts
                             join pod in Repository.PurchaseOrderDetails on parts.PartID equals pod.PartID
                             join po in Repository.PurchaseOrders on pod.PurchaseOrderID equals po.PurchaseOrderID
                             where po.PurchaseOrderID == id
                             select new OrderDetailModel
                             {
                                 PartID = parts.PartID,
                                 Description = parts.Description,
                                 OQty = parts.QuantityOnOrder,
                                 OStd = parts.QuantityOnHand
                             };

            return joinedData.ToList(); 
        }
        catch (DbException)
        {
            throw;
        }
    }

    public int GetEmployeeID(int purchaseOrderId)
    {
        return (from po in Repository.PurchaseOrders
                where po.PurchaseOrderID == purchaseOrderId
                select po.EmployeeID).FirstOrDefault();
    }
    public PurchaseOrder GetPurchaseOrder(int purchaseOrderID)
    {
        return Repository.PurchaseOrders.Where(po => po.PurchaseOrderID == purchaseOrderID).First();
    }
    public PurchaseOrderDetail GetPurchaseOrderDetails(int purchaseOrderID)
    {
        return Repository.PurchaseOrderDetails.Where(po => po.PurchaseOrderID == purchaseOrderID).First();
    }
    public List<PurchaseOrder>? GetOutstandingOrders()
    {
        return Repository.PurchaseOrders.Where(po => po.Closed == false).ToList(); ;
    }
    public List<UnorderedPurchaseItemCart>? GetStoredUnorderedItems()
    {
        return Repository.UnorderedPurchaseItemCarts.ToList();
    }

    public int GetLastReceiveOrderID()
    {
        return Repository.ReceiveOrders.Max(ro => ro.ReceiveOrderID);
    }

    public int InsertReceiveOrder(ReceiveOrder receiveOrder)
    {
        Repository.ReceiveOrders.Add(receiveOrder);
        Repository.SaveChanges();
        return Repository.ReceiveOrders.Max(ro => ro.ReceiveOrderID);
    }

    public int InsertReceiveOrderDetails(ReceiveOrderDetail receiveOrderDetails)
    {
        Repository.ReceiveOrderDetails.Add(receiveOrderDetails);
        Repository.SaveChanges();
        return Repository.ReceiveOrderDetails.Max(ro => ro.ReceiveOrderDetailID);
    }

    public void UpdatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
    {
        Repository.PurchaseOrderDetails.Update(purchaseOrderDetail);
        Repository.SaveChanges();
    }
    public void UpdatePurchaseOrder(PurchaseOrder selectedPO)
    {
        Repository.PurchaseOrders.Update(selectedPO);
        Repository.SaveChanges();
    }

    public void IncreasePartQuantity(int partID, int recQty)
    {
        Repository.Parts.Where(p => p.PartID == partID).First().QuantityOnHand += recQty;
        Repository.SaveChanges();
    }

    public void DecreasePartQuantity(int partID, int recQty)
    {
        Repository.Parts.Where(p => p.PartID == partID).First().QuantityOnHand -= recQty;
        Repository.SaveChanges();
    }
    public void DecreaseOrderedQuantity(int partID, int recQty)
    {
        var QtyOnHand = Repository.Parts.Where(p => p.PartID == partID).First().QuantityOnOrder;
        if (QtyOnHand - recQty <= 0)
            Repository.Parts.Where(p => p.PartID == partID).First().QuantityOnOrder = 1;
        else
            Repository.Parts.Where(p => p.PartID == partID).First().QuantityOnOrder -= recQty;
        Repository.SaveChanges();
    }

    public void ClosePurchaseOrder(int purchaseOrderID)
    {
        Repository.PurchaseOrders.Where(po => po.PurchaseOrderID == purchaseOrderID).First<PurchaseOrder>().Closed = true;
        Repository.SaveChanges();
    }

    public void DeleteUnorderedItem(UnorderedPurchaseItemCart item)
    {
        Repository.UnorderedPurchaseItemCarts.Remove(item);
        Repository.SaveChanges();
    }

    public void InsertUnorderedItem(UnorderedPurchaseItemCart unorderedItemCart)
    {
        Repository.UnorderedPurchaseItemCarts.Add(unorderedItemCart);
        Repository.SaveChanges();
    }
    public void InsertReturnDetails(ReturnedOrderDetail returnDetails)
    {
        Repository.ReturnedOrderDetails.Add(returnDetails);
        Repository.SaveChanges();
    }

}
