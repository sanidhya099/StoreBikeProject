﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicingSystem.Entities;

public partial class Job
{
    [Key]
    public int JobID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime JobDateIn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JobDateStarted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JobDateDone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JobDateOut { get; set; }

    public int EmployeeID { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal ShopRate { get; set; }

    [Required]
    [StringLength(13)]
    public string VehicleIdentification { get; set; }

    [ForeignKey("EmployeeID")]
    [InverseProperty("Jobs")]
    public virtual Employee Employee { get; set; }

    [InverseProperty("Job")]
    public virtual ICollection<JobDetail> JobDetails { get; set; } = new List<JobDetail>();

    [ForeignKey("VehicleIdentification")]
    [InverseProperty("Jobs")]
    public virtual CustomerVehicle VehicleIdentificationNavigation { get; set; }
}