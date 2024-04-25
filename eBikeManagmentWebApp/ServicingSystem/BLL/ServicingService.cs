using ServicingSystem.DAL;
using ServicingSystem.Entities;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServicingSystem.BLL;
public class ServicingService
{
    private readonly ServicingDataContext Repository;
    public ServicingService(ServicingDataContext WFS_2590Context)
    {
        Repository = WFS_2590Context;
    }

    public void DeleteJobDetailPart(JobDetailPart item)
    {
        Repository.JobDetailParts.Remove(item);
        Repository.SaveChanges();
    }

    public List<Customer> GetAllCustomers()
    {
        return Repository.Customers.ToList();
    }

    public List<Job> GetAllJobs()
    {
        return Repository.Jobs.ToList();    
    }

    public List<Job> GetAllOpenJobs()
    {
        return Repository.Jobs.Where(j => j.JobDateDone == null).ToList();
    }

    public List<Part> GetAllParts()
    {
       return Repository.Parts.ToList();
    }

    public Coupon? GetCoupon(string? couponCode)
    {
        if (!string.IsNullOrEmpty(couponCode))
        {
            var coupon = Repository.Coupons.Where(c => c.CouponIDValue == couponCode);
            if (coupon.Count() > 0)
            {
                return coupon.First();
            }
            else return null;
        }
        else return null;
    }
    public List<CustomerVehicle> GetCustomerVehicles()
    {
        return Repository.CustomerVehicles.ToList();
    }

    public Job? GetJob(int jobID)
    {
        return Repository.Jobs.Where(j => j.JobID == jobID).First();
    }

    public List<JobDetail> GetJobDetails(int jobID)
    {
        return Repository.JobDetails.Where(j => j.JobID == jobID).ToList();
    }

    public List<JobDetailPart> GetJobParts(int jobDetailID)
    {
        return Repository.JobDetailParts.Where(jp => jp.JobDetailID == jobDetailID).ToList();
    }

    public Part GetPart(int selectedPartId)
    {
        return Repository.Parts.Where(p => p.PartID == selectedPartId).First<Part>();
    }

    public StandardJob? GetStandardJob(int standardJobId)
    {
        return Repository.StandardJobs.Where(sj => sj.StandardJobID == standardJobId).First();
    }

    public List<StandardJob> GetStandardJobs()
    {
        return Repository.StandardJobs.ToList();
    }

    public int InsertJob(Job newJob)
    {
        Repository.Jobs.Add(newJob);
        Repository.SaveChanges();
        return Repository.Jobs.Max(j => j.JobID);
    }

    public int InsertJobDetail(JobDetail newJobDetail)
    {
        Repository.JobDetails.Add(newJobDetail);
        Repository.SaveChanges();
        return Repository.JobDetails.Max(j => j.JobDetailID);
    }

    public void InsertJobDetailPart(JobDetailPart newItem)
    {
        Repository.JobDetailParts.Add(newItem);
        Repository.SaveChanges();
    }

    public void UpdateJob(Job? selectedJob)
    {
        Repository.Jobs.Update(selectedJob);
    }
}
