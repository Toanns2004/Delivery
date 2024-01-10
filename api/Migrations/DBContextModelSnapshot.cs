﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Context;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("api.Entities.Bill", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("billNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("charge")
                        .HasColumnType("float");

                    b.Property<double>("cod")
                        .HasColumnType("float");

                    b.Property<DateTime>("dateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("deliveryAddId")
                        .HasColumnType("int");

                    b.Property<string>("deliveryType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("insuranceFee")
                        .HasColumnType("float");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("payer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pickupType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("shippingAddId")
                        .HasColumnType("int");

                    b.Property<double>("totalCharge")
                        .HasColumnType("float");

                    b.Property<int>("unitPriceId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("deliveryAddId");

                    b.HasIndex("shippingAddId");

                    b.HasIndex("unitPriceId");

                    b.HasIndex("userId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("api.Entities.BillDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("billId")
                        .HasColumnType("int");

                    b.Property<int>("height")
                        .HasColumnType("int");

                    b.Property<int>("length")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("value")
                        .HasColumnType("float");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.Property<int>("width")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("billId");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("api.Entities.DeliveryAddress", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<int>("wardId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.HasIndex("wardId");

                    b.ToTable("DeliveryAddresses");
                });

            modelBuilder.Entity("api.Entities.District", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("district_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("province_id")
                        .HasColumnType("int");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("province_id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("api.Entities.Employee", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("postOfficeId")
                        .HasColumnType("int");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("postOfficeId");

                    b.HasIndex("roleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("api.Entities.Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("roleId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("api.Entities.PostOffice", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("latitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("longitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("wardId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("wardId");

                    b.ToTable("PostOffices");
                });

            modelBuilder.Entity("api.Entities.Province", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("province_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("province_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("api.Entities.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("api.Entities.ShippingAddress", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<int>("wardId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.HasIndex("wardId");

                    b.ToTable("ShippingAddresses");
                });

            modelBuilder.Entity("api.Entities.Status", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("billId")
                        .HasColumnType("int");

                    b.Property<int?>("employeeId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime2");

                    b.Property<int>("typeId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("billId");

                    b.HasIndex("employeeId");

                    b.HasIndex("typeId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("api.Entities.StatusType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("StatusTypes");
                });

            modelBuilder.Entity("api.Entities.UnitPrice", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<double>("chargeRate")
                        .HasColumnType("float");

                    b.Property<string>("range")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("weightLimit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("UnitPrices");
                });

            modelBuilder.Entity("api.Entities.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api.Entities.Ward", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<int>("district_id")
                        .HasColumnType("int");

                    b.Property<string>("location_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ward_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("district_id");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("api.Entities.Bill", b =>
                {
                    b.HasOne("api.Entities.DeliveryAddress", "DeliveryAddress")
                        .WithMany()
                        .HasForeignKey("deliveryAddId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.ShippingAddress", "ShippingAddress")
                        .WithMany()
                        .HasForeignKey("shippingAddId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.UnitPrice", "UnitPrice")
                        .WithMany()
                        .HasForeignKey("unitPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryAddress");

                    b.Navigation("ShippingAddress");

                    b.Navigation("UnitPrice");

                    b.Navigation("User");
                });

            modelBuilder.Entity("api.Entities.BillDetail", b =>
                {
                    b.HasOne("api.Entities.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("billId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");
                });

            modelBuilder.Entity("api.Entities.DeliveryAddress", b =>
                {
                    b.HasOne("api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("wardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("api.Entities.District", b =>
                {
                    b.HasOne("api.Entities.Province", "Province")
                        .WithMany()
                        .HasForeignKey("province_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("api.Entities.Employee", b =>
                {
                    b.HasOne("api.Entities.PostOffice", "PostOffice")
                        .WithMany()
                        .HasForeignKey("postOfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostOffice");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("api.Entities.Permission", b =>
                {
                    b.HasOne("api.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("api.Entities.PostOffice", b =>
                {
                    b.HasOne("api.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("wardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("api.Entities.ShippingAddress", b =>
                {
                    b.HasOne("api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("wardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("api.Entities.Status", b =>
                {
                    b.HasOne("api.Entities.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("billId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("employeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.StatusType", "StatusType")
                        .WithMany()
                        .HasForeignKey("typeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("Employee");

                    b.Navigation("StatusType");
                });

            modelBuilder.Entity("api.Entities.Ward", b =>
                {
                    b.HasOne("api.Entities.District", "District")
                        .WithMany()
                        .HasForeignKey("district_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });
#pragma warning restore 612, 618
        }
    }
}
