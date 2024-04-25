﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SalesAndReturnSystem.Entities;

[Index("SaleID", "PartID", Name = "UQ_SaleDetails_SaleIDPartID", IsUnique = true)]
public partial class SaleDetail
{
    [Key]
    public int SaleDetailID { get; set; }

    public int SaleID { get; set; }

    public int PartID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal SellingPrice { get; set; }

    [ForeignKey("PartID")]
    [InverseProperty("SaleDetails")]
    public virtual Part Part { get; set; }

    [ForeignKey("SaleID")]
    [InverseProperty("SaleDetails")]
    public virtual Sale Sale { get; set; }
}