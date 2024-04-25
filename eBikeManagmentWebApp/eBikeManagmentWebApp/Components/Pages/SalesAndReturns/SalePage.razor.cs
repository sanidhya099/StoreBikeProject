
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SalesAndReturnSystem.BLL;
using SalesAndReturnSystem.Entities;
using SalesAndReturnSystem.ViewModels;

namespace eBikeManagmentWebApp.Components.Pages.SalesAndReturns;

public partial class SalePage: ComponentBase
{
    [Inject]
    public IJSRuntime JS { get; set; }
    [Inject]
    public SalesService Data { get; set; }

    private int selectedItemQty;
    private int selectedItemID;
    private int saleID;
    private int couponID = -1;
    private int categoryID = -1;
    private int partID = -1;
    private int saleInvoiceID;
    private decimal selectedItemPrice;
    private decimal subTotal, tax, total, discount;
    private string? CategoryDescription;
    private string? selectedItemDesc;
    private string? couponCode;
    private string? selectedPaymentOption = "Radio 1";
    private List<Category>? categories;
    private List<Part>? parts;
    private List<Part>? filteredParts;
    private List<TableData>? tableItems;

    public bool ReadOnly { get; set; }

    protected override void OnInitialized()
    {
        saleID = Data.GetLastSaleID() + 1;
        categories = Data.GetCategories();

        parts = Data.GetParts();

        filteredParts = parts;

        tableItems = new List<TableData>();
    }

    private void HandleCategorySelect()
    {
        filteredParts = parts?.Where(part => part.CategoryID == categoryID).ToList();
        CategoryDescription = categories?.Where(c => c.CategoryID == categoryID).FirstOrDefault<Category>()?.Description;
    }

    private void HandlePartSelect()
    {
        if (filteredParts != null)
        {
            var part = filteredParts.Where(p => p.PartID == partID).First<Part>();

            selectedItemID = part.PartID;
            selectedItemDesc = part.Description;
            selectedItemPrice = (decimal)part.SellingPrice;
        }
        else
            selectedItemQty = 0;
    }

    private void AddItemToTable()
    {
        var tableItem = new TableData
        {
            Id = selectedItemID,
            Item = selectedItemDesc ?? "",
            Qty = selectedItemQty,
            Price = selectedItemPrice,
            Total = selectedItemQty * selectedItemPrice
        };

        if (tableItems != null && !tableItems.Any(item => item.Item == tableItem.Item))
        {
            tableItems.Add(tableItem);
            subTotal = tableItems.Sum(item => item.Total);
        }
        UpdateTotals();
    }
    public async Task CheckOut()
    {
        try
        {
            if (tableItems.Count >= 1)
            {
                DateTime SaleDate = DateTime.Now;
                int EmployeeID = 1;
                decimal TaxAmount = tax;
                string PaymentType = "M";
                if (selectedPaymentOption == "Radio 1")
                {
                    PaymentType = "M";
                    Console.WriteLine("Cash radio button is selected");
                }
                else if (selectedPaymentOption == "Radio 2")
                {
                    PaymentType = "C";
                    Console.WriteLine("Credit radio button is selected");
                }
                else if (selectedPaymentOption == "Radio 3")
                {
                    PaymentType = "D";
                    Console.WriteLine("Debit radio button is selected");
                }

                saleID = Data.GetLastSaleID() + 1;
                Sale newSale = new Sale
                {
                    SaleDate = SaleDate,
                    EmployeeID = EmployeeID,
                    TaxAmount = (decimal)TaxAmount,
                    SubTotal = (decimal)subTotal,
                    CouponID = couponID >= 0 ? couponID : null,
                    PaymentType = PaymentType
                };
                Data.InsertNewSale(newSale);

                var salesDetailsDbContext = Data.GetAllSalesDetails();

                tableItems.ForEach((tableItem) =>
                {
                    SaleDetail newSalesDetail = new SaleDetail
                    {
                        SaleID = saleID,
                        PartID = tableItem.Id,
                        Quantity = tableItem.Qty,
                        SellingPrice = (decimal)Convert.ToSingle(tableItem.Price)
                    };

                    var existingEntity = salesDetailsDbContext.FirstOrDefault(e => e.SaleDetailID == newSalesDetail.SaleDetailID);
                    if (existingEntity != null)
                    {
                        salesDetailsDbContext[salesDetailsDbContext.IndexOf(existingEntity)] = newSalesDetail;
                    }
                    else
                    {
                        salesDetailsDbContext.Add(newSalesDetail);
                        Data.UpdateSalesDetails(salesDetailsDbContext);
                    }
                });
                await JS.InvokeAsync<string>("showAlert", "Success!");
            }
            else
            {
                await JS.InvokeAsync<string>("showAlert", "Please add parts!");
            }
        }
        catch (DbUpdateException)
        {
            await JS.InvokeAsync<string>("showAlert", "Database Error!, Could not insert sale.");
        }
    }

    private void VerifyCoupon()
    {
        var coupon = Data.GetCoupon(couponCode);
        if (coupon != null)
        {
            discount = coupon.CouponDiscount;
            couponID = coupon.CouponID;
        }
        else
        {
            couponCode = "";
            discount = 0;
            couponID = -1;
        }
        UpdateTotals();
    }
    protected void OnQtyChangeHandler(TableData data)
    {
        data.Total = data.Qty * data.Price;
        UpdateTotals();
    }
    protected void OnTaxValueChangeHandler()
    {
        var taxPct = tax / 100;
        var discountPct = discount / 100;
        total = Math.Round(subTotal + (subTotal * (taxPct - discountPct)), 2);
    }
    protected void OnDiscountValueChangeHandler()
    {
        var taxPct = tax / 100;
        var discountPct = discount / 100;
        total = Math.Round(subTotal + (subTotal * (taxPct - discountPct)), 2);
    }
    private void HandleDelete(TableData context)
    {
        Console.WriteLine("=============", tableItems);
        tableItems?.Remove(context);
        subTotal = subTotal - context.Total;
        UpdateTotals();
    }
    private void UpdateTotals()
    {
        if (tableItems != null)
        {
            subTotal = Math.Round(tableItems.Sum(item => item.Total), 2);
            total = Math.Round((subTotal + tax - discount), 2);
        }
    }
    private void ClearTable()
    {
        tableItems?.Clear();
        subTotal = 0;
        tax = 0;
        discount = 0;
        total = 0;
    }
}
