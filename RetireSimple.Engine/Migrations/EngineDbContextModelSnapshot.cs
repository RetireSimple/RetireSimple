﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RetireSimple.Engine.Data;

#nullable disable

namespace RetireSimple.Engine.Migrations
{
    [DbContext(typeof(EngineDbContext))]
    partial class EngineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("RetireSimple.Engine.Data.Expense.ExpenseBase", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0.0);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SourceInvestmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ExpenseId");

                    b.HasIndex("SourceInvestmentId");

                    b.ToTable("Expenses", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("ExpenseBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.InvestmentBase", b =>
                {
                    b.Property<int>("InvestmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnalysisOptionsOverrides")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AnalysisType")
                        .HasColumnType("TEXT")
                        .HasColumnName("AnalysisType");

                    b.Property<string>("InvestmentData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InvestmentName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.Property<string>("InvestmentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("InvestmentVehicleBaseInvestmentVehicleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2(7)");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("InvestmentId");

                    b.HasIndex("InvestmentVehicleBaseInvestmentVehicleId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Investments");

                    b.HasDiscriminator<string>("InvestmentType").HasValue("InvestmentBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentModel", b =>
                {
                    b.Property<int>("InvestmentModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvgModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InvestmentId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaxModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MinModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("InvestmentModelId");

                    b.HasIndex("InvestmentId")
                        .IsUnique();

                    b.ToTable("InvestmentModel", (string)null);
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentTransfer", b =>
                {
                    b.Property<int>("InvestmentTransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0.0);

                    b.Property<int>("DestinationInvestmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceInvestmentId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TransferDate")
                        .HasColumnType("REAL");

                    b.HasKey("InvestmentTransferId");

                    b.HasIndex("DestinationInvestmentId");

                    b.HasIndex("SourceInvestmentId");

                    b.ToTable("InvestmentTransfers", (string)null);
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase", b =>
                {
                    b.Property<int>("InvestmentVehicleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnalysisOptionsOverrides")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CashHoldings")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0.0m);

                    b.Property<int?>("InvestmentVehicleModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InvestmentVehicleName")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvestmentVehicleType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("InvestmentVehicleId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("InvestmentVehicle");

                    b.HasDiscriminator<string>("InvestmentVehicleType").HasValue("InvestmentVehicleBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicleModel", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvgModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InvestmentVehicleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaxModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MinModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxDeductedAvgModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxDeductedMaxModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxDeductedMinModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ModelId");

                    b.HasIndex("InvestmentVehicleId")
                        .IsUnique();

                    b.ToTable("InvestmentVehicleModel");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.PortfolioModel", b =>
                {
                    b.Property<int>("PortfolioModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvgModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaxModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MinModelData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PortfolioModelId");

                    b.HasIndex("PortfolioId")
                        .IsUnique();

                    b.ToTable("PortfolioModel", (string)null);
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.User.Portfolio", b =>
                {
                    b.Property<int>("PortfolioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("PortfolioName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PortfolioId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Portfolio");

                    b.HasData(
                        new
                        {
                            PortfolioId = 1,
                            PortfolioName = "Default",
                            ProfileId = 1
                        });
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.User.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProfileId");

                    b.ToTable("Profile");

                    b.HasData(
                        new
                        {
                            ProfileId = 1,
                            Age = 65,
                            Name = "Default",
                            Status = true
                        });
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Expense.OneTimeExpense", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Expense.ExpenseBase");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("OneTime");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Expense.RecurringExpense", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Expense.ExpenseBase");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Frequency")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Recurring");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.AnnuityInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("AnnuityInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.BondInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("BondInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.CashInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("CashInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.FixedInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("FixedInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.PensionInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("PensionInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.SocialSecurityInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("SocialSecurityInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.StockInvestment", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.Investment.InvestmentBase");

                    b.ToTable("Investments");

                    b.HasDiscriminator().HasValue("StockInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.Vehicle401k", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase");

                    b.HasDiscriminator().HasValue("401k");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.Vehicle403b", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase");

                    b.HasDiscriminator().HasValue("403b");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.Vehicle457", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase");

                    b.HasDiscriminator().HasValue("457");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.VehicleIRA", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase");

                    b.HasDiscriminator().HasValue("IRA");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.VehicleRothIRA", b =>
                {
                    b.HasBaseType("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase");

                    b.HasDiscriminator().HasValue("RothIRA");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Expense.ExpenseBase", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.Investment.InvestmentBase", "SourceInvestment")
                        .WithMany("Expenses")
                        .HasForeignKey("SourceInvestmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.InvestmentBase", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase", null)
                        .WithMany("Investments")
                        .HasForeignKey("InvestmentVehicleBaseInvestmentVehicleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RetireSimple.Engine.Data.User.Portfolio", null)
                        .WithMany("Investments")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentModel", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.Investment.InvestmentBase", "Investment")
                        .WithOne("InvestmentModel")
                        .HasForeignKey("RetireSimple.Engine.Data.InvestmentModel", "InvestmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Investment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentTransfer", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.Investment.InvestmentBase", "DestinationInvestment")
                        .WithMany("TransfersTo")
                        .HasForeignKey("DestinationInvestmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RetireSimple.Engine.Data.Investment.InvestmentBase", "SourceInvestment")
                        .WithMany("TransfersFrom")
                        .HasForeignKey("SourceInvestmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationInvestment");

                    b.Navigation("SourceInvestment");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.User.Portfolio", null)
                        .WithMany("InvestmentVehicles")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicleModel", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase", null)
                        .WithOne("InvestmentVehicleModel")
                        .HasForeignKey("RetireSimple.Engine.Data.InvestmentVehicleModel", "InvestmentVehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.PortfolioModel", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.User.Portfolio", "Portfolio")
                        .WithOne("PortfolioModel")
                        .HasForeignKey("RetireSimple.Engine.Data.PortfolioModel", "PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.User.Portfolio", b =>
                {
                    b.HasOne("RetireSimple.Engine.Data.User.Profile", "Profile")
                        .WithMany("Portfolios")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.Investment.InvestmentBase", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("InvestmentModel");

                    b.Navigation("TransfersFrom");

                    b.Navigation("TransfersTo");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.InvestmentVehicle.InvestmentVehicleBase", b =>
                {
                    b.Navigation("InvestmentVehicleModel");

                    b.Navigation("Investments");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.User.Portfolio", b =>
                {
                    b.Navigation("InvestmentVehicles");

                    b.Navigation("Investments");

                    b.Navigation("PortfolioModel");
                });

            modelBuilder.Entity("RetireSimple.Engine.Data.User.Profile", b =>
                {
                    b.Navigation("Portfolios");
                });
#pragma warning restore 612, 618
        }
    }
}
