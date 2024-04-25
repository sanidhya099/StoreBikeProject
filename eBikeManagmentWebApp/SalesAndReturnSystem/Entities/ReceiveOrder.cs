﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SalesAndReturnSystem.Entities;

public partial class ReceiveOrder
{
    [Key]
    public int ReceiveOrderID { get; set; }

    public int PurchaseOrderID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceiveDate { get; set; }

    public int EmployeeID { get; set; }

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("ReceiveOrders")]
    public virtual PurchaseOrder PurchaseOrder { get; set; }

    [InverseProperty("ReceiveOrder")]
    public virtual ICollection<ReceiveOrderDetail> ReceiveOrderDetails { get; set; } = new List<ReceiveOrderDetail>();

    [InverseProperty("ReceiveOrder")]
    public virtual ICollection<ReturnedOrderDetail> ReturnedOrderDetails { get; set; } = new List<ReturnedOrderDetail>();
}