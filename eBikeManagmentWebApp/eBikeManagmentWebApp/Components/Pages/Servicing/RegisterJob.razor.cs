
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ServicingSystem.BLL;
using ServicingSystem.Entities;
using ServicingSystem.ViewModels;

namespace eBikeManagmentWebApp.Components.Pages.Servicing;

public partial class RegisterJob: ComponentBase
{
    [Inject]
    public IJSRuntime JS { get; set; }
    [Inject]
    public ServicingService Data { get; set; }

    private bool isEditMode = false;
    private int selectedJobId;
    private int couponID = -1;
    private int qty;
    private decimal couponValue;
    private decimal hours;
    private decimal totalHrs;
    private decimal shopRate;
    private decimal jobValue => Math.Round(totalHrs * shopRate, 2);
    private string? serviceName;
    private string? comment;
    private string? VINNumber;
    private string? couponCode;
    private string? searchName;
    private string? ServiceforVINLabel = "";
    private StandardJob? selectedService;
    private List<VinServicingModel>? vinservicing = new List<VinServicingModel>();
    private List<Part>? displayPart;
    private List<JobDetailModel> orderDetails = new List<JobDetailModel>();
    private List<StandardJob> services;
    private List<Part> parts;
    private List<Part> servicingPart = new List<Part>();
    private List<Customer> customers = new List<Customer>();
    private List<CustomerShowTable> allCustomers = new List<CustomerShowTable>();
    private List<CustomerShowTable> filteredCustomers = new List<CustomerShowTable>();
    private List<CustomerVehicle> vins = new List<CustomerVehicle>();
    private CustomerShowTable selectedCustomer = new CustomerShowTable();


    private void HandleJobSelection()
    {
        selectedService = Data.GetStandardJob(selectedJobId);
        serviceName = selectedService.Description;
        hours = selectedService.StandardHours;
    }


    protected override void OnInitialized()
    {
        parts = Data.GetAllParts();

        services = Data.GetStandardJobs();
        var jobsData = Data.GetAllJobs();
        var combinedData = jobsData.Join(Data.GetCustomerVehicles(),
        job => job.VehicleIdentification,
        vehicle => vehicle.VehicleIdentification,
        (job, vehicle) => new { VIN = job.VehicleIdentification, Model = vehicle.Model });

        foreach (var item in combinedData)
        {
            Console.WriteLine($"VIN: {item.VIN}, Model: {item.Model}");
            vinservicing.Add(new VinServicingModel
            {
                VIN = item.VIN,
                Model = item.Model
            });
        }
        var getData = Data.GetAllCustomers();
        allCustomers = getData.Select(c => new CustomerShowTable
        {
            CustomerID = c.CustomerID,
            Name = c.FirstName + " " + c.LastName,
            Phone = c.ContactPhone,
            Address = c.Address

        }).ToList();
        filteredCustomers = allCustomers;
    }

    private void UpdatePrice(JobDetailPart item)
    {
        item.SellingPrice = Math.Round(item.Quantity * Data.GetPart(item.PartID).SellingPrice, 2);
    }

    private void SelectVIN(VinServicingModel context)
    {
        ServiceforVINLabel = context.Model + '(' + context.VIN + ')';
    }

    private void Clear()
    {
        searchName = "";
        VINNumber = "";
        serviceName = "";
        comment = "";
        hours = 0;
        selectedService = new();
        orderDetails.Clear();
        filteredCustomers = allCustomers;
    }

    private void VerifyCoupon()
    {
        var coupon = Data.GetCoupon(couponCode);
        if (coupon != null)
        {
            couponValue = coupon.CouponDiscount;
            couponID = coupon.CouponID;
            OnShopRateChanged();
        }
        else
        {
            couponCode = "";
            couponValue = 0;
            couponID = -1;
        }
    }
    private void DeleteService(JobDetailModel deleteItem)
    {
        JobDetailModel itemToRemove = orderDetails.Where(o => o.Service == deleteItem.Service && o.Comments == deleteItem.Comments).First<JobDetailModel>();

        if (itemToRemove != null)
        {
            orderDetails.Remove(itemToRemove);
        }
        totalHrs = orderDetails.Sum(o => o.JobHours);
    }
    private void HandleCustomerSearchClick()
    {
        if (searchName == "")
        {
            filteredCustomers = allCustomers;
        }
        else
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                filteredCustomers = allCustomers.Where(c => c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
    }
    private void OnShopRateChanged()
    {
        if (couponValue != 0)
            shopRate = (shopRate - (shopRate * (couponValue / 100)));
    }

    private void SelectCustomer(CustomerShowTable item)
    {
        selectedCustomer = item;
        var cv = Data.GetCustomerVehicles();
        Console.WriteLine(cv.Count.ToString());
        vins = cv.Where(i => i.CustomerID == item.CustomerID).ToList();
    }

    private void Add()
    {
        var addItem = new JobDetailModel
        {
            JobHours = hours,
            Comments = comment,
            ShopRate = Convert.ToDecimal(shopRate),
            Service = selectedService ?? new StandardJob()
        };
        orderDetails.Add(addItem);
        totalHrs = orderDetails.Sum(o => o.JobHours);
    }

    private async Task RegisterNewJob()
    {
        var jobID = await InsertJob();
        if (jobID >= 0)
            await SaveJobDetails(jobID);
    }

    private async Task<int> InsertJob()
    {
        try
        {
            var newJob = new Job()
            {
                JobDateIn = DateTime.Now,
                JobDateStarted = DateTime.Now,
                EmployeeID = 1,
                ShopRate = Convert.ToDecimal(shopRate),
                VehicleIdentification = VINNumber
            };

            return Data.InsertJob(newJob);
        }
        catch (DbUpdateException)
        {
            await JS.InvokeAsync<string>("alert", "Error registering job, please check all fields and or database connection.");
            return -1;
        }
    }


    private async Task SaveJobDetails(int jobsID)
    {
        try
        {

            if (orderDetails.Count > 0)
            {
                foreach (var detail in orderDetails)
                {
                    var newJobDetail = new JobDetail
                    {
                        JobID = jobsID,
                        Description = detail.Service?.Description,
                        JobHours = detail.JobHours,
                        Comments = detail.Comments,
                        CouponID = (couponID != -1) ? couponID : null,
                        EmployeeID = 1
                    };
                    var detailID = Data.InsertJobDetail(newJobDetail);
                }

                await JS.InvokeAsync<string>("alert", "Success!");
            }
            else
                await JS.InvokeAsync<string>("alert", "There are no items in job, please check values.");
        }
        catch (DbUpdateException)
        {
            await JS.InvokeAsync<string>("alert", "Error registering job, please check all fields and or database connection.");
        }

    }


    private void Reset()
    {
        serviceName = "";
        selectedJobId = -1;
    }
}
