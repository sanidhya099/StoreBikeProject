
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ServicingSystem.BLL;
using ServicingSystem.Entities;
using ServicingSystem.ViewModels;

namespace eBikeManagmentWebApp.Components.Pages.Servicing;

public partial class ServiceJob :ComponentBase
{
    [Inject]
    public IJSRuntime JS { get; set; }
    [Inject]
    public ServicingService Data { get; set; }

    private List<StandardJob> services;
    private List<Part> parts;
    private Part selectedPart;
    private List<Customer> customers = new List<Customer>();
    private List<JobDetailPart> jobTasks = new List<JobDetailPart>();
    private List<CustomerVehicle> vins = new List<CustomerVehicle>();
    private OutstandingOrderModel selectedCustomer;
    private List<OutstandingOrderModel> vinservicing = new List<OutstandingOrderModel>();
    private List<ServiceItemModel> serviceItems = new();
    private Job? selectedJob;
    private JobDetail? selectedJobDetail;
    private bool isEditMode = false;
    private bool showService;
    private int selectedPartId;
    private decimal shopRate;
    private string? editButtonLabel = "Change";
    private int qty;
    private int serviceID;
    private decimal projectHours;
    private string? selectedStatus;
    private string? comment;
    private string? selectedVIN;
    private string? service = "";
    private string? ServiceForVINLabel = "";

    protected override void OnInitialized()
    {
        parts = Data.GetAllParts();
        GetOutstandingOrders();
    }
    private void GetOutstandingOrders()
    {
        vinservicing.Clear();
        var jobData = Data.GetAllOpenJobs();
        var combinedData = jobData.Join(Data.GetCustomerVehicles(),
        job => job.VehicleIdentification,
        vehicle => vehicle.VehicleIdentification,
        (job, vehicle) => new { ID = job.JobID, VIN = job.VehicleIdentification, Model = vehicle.Model });

        foreach (var item in combinedData)
        {
            vinservicing.Add(new OutstandingOrderModel()
            {
                Vehicle = new CustomerVehicle
                {
                    VehicleIdentification = item.VIN,
                    Model = item.Model
                },
                JobID = item.ID
            });
        }
    }
    private void UpdateStatus()
    {
        if (selectedJobDetail != null)
        {
            if (selectedStatus != "X")
                selectedJobDetail.StatusCode = selectedStatus;
        }
        if (selectedJob != null)
        {
            switch (selectedStatus)
            {
                case "D":
                    selectedJob!.JobDateDone = DateTime.Now;
                    break;
                case "S":
                    selectedJob!.JobDateStarted = DateTime.Now;
                    break;
            }
        }
    }
    private async Task Save()
    {
        try
        {
            foreach (var item in jobTasks)
            {
                Data.InsertJobDetailPart(item);
            }
            Data.UpdateJob(selectedJob);
            Reset();
        }
        catch (DbUpdateException ex)
        {
            await JS.InvokeAsync<string>("showAlert", "Database Error!Could not save job.");
        }
    }
    private void Reset()
    {
        selectedCustomer = new();
        selectedJob = null;
        selectedJobDetail = null;
        selectedStatus = "X";
        showService = false;
        jobTasks.Clear();
        GetOutstandingOrders();

    }
    private async Task ClearChanges()
    {
        SelectJob(selectedCustomer.JobID);
    }

    private async Task HandleSelectPart()
    {
        selectedPart = parts.Where(part => part.PartID == selectedPartId).First<Part>();
    }
    private async Task AddPart()
    {
        if (qty <= selectedPart.QuantityOnHand)
        {
            if (selectedJobDetail != null)
            {
                var newItem = new JobDetailPart
                {
                    PartID = selectedPart.PartID,
                    JobDetailID = selectedJobDetail.JobDetailID,
                    Part = selectedPart,
                    Quantity = (short)qty,
                    SellingPrice = Math.Round(selectedPart.SellingPrice * qty, 2)
                };
                jobTasks.Add(newItem);
            }
            else
            {
                await JS.InvokeVoidAsync("showAlert", "Database error, could not find JobDetails for selected Job.");
            }
        }
        else
        {
            await JS.InvokeVoidAsync("showAlert", $"Quantity entered exceeded qty on hand of {selectedPart.QuantityOnHand}");
        }

    }
    void UpdatePrice(JobDetailPart item)
    {
        item.SellingPrice = Math.Round(item.Quantity * parts.Where(p => p.PartID == item.PartID).First<Part>().SellingPrice, 2);
    }
    private void SelectOrder(OutstandingOrderModel vehicle)
    {
        showService = true;
        ServiceForVINLabel = vehicle.Vehicle.Model + '(' + vehicle.Vehicle.VehicleIdentification + ')';
        selectedVIN = vehicle.Vehicle.VehicleIdentification;
        selectedCustomer = vehicle;

        SelectJob(vehicle.JobID);
    }
    private void SelectJob(int JobID)
    {
        serviceItems.Clear();
        selectedJob = Data.GetJob(JobID);
        var details = Data.GetJobDetails(selectedJob.JobID);
        if (details.Count() > 0)
        {
            selectedJobDetail = details.Where(jd => jd.JobID == selectedJob.JobID).First<JobDetail>();
            jobTasks = Data.GetJobParts(selectedJobDetail.JobDetailID);

            var joinedData = details.Join(Data.GetStandardJobs(), j => j.Description, s => s.Description, (j, s) => new
            {
                QTY = j.JobHours,
                DESCRIPTION = j.Description,
                STANDARDJOBID = s.StandardJobID
            });
            foreach (var item in joinedData)
            {
                serviceItems.Add(new ServiceItemModel()
                {
                    Hours = item.QTY,
                    Description = item.DESCRIPTION,
                    StandardJobID = item.STANDARDJOBID
                });
            }

            selectedStatus = selectedJobDetail.StatusCode;
            projectHours = selectedJobDetail.JobHours;
        }
        foreach (var task in jobTasks)
        {
            task.Part = Data.GetPart(task.PartID);
        }
    }
    private void SelectDelete(JobDetailPart item)
    {
        Data.DeleteJobDetailPart(item);
        jobTasks.Remove(item);
    }
    private void SelectChange(JobDetailPart item)
    {
        isEditMode = !isEditMode;
        if (isEditMode)
            editButtonLabel = "Save";
        else
            editButtonLabel = "Change";
    }
}
