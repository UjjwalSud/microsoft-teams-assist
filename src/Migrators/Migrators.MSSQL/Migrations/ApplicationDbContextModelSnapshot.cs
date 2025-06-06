﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.Email.EmailLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bcc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailSentMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailSmtpUsed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmailType")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Headers")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailSent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReplyTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReplyToName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmailLog", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFieldOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FKFormPageFieldPKId")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FKFormPageFieldPKId");

                    b.ToTable("FormPageFieldOptions", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFields", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Columns")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomRegularExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DateTimeType")
                        .HasColumnType("int");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("DisabledExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("FKFormPageFieldPKId")
                        .HasColumnType("int");

                    b.Property<int>("FKFormPagePKId")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileUploadCustomExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUploadCustomValidationExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileUploadValidFileType")
                        .HasColumnType("int");

                    b.Property<int>("FormPageFieldType")
                        .HasColumnType("int");

                    b.Property<string>("HideExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabelExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaximumExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaximumLength")
                        .HasColumnType("int");

                    b.Property<string>("MinimumExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MinimumLength")
                        .HasColumnType("int");

                    b.Property<int?>("NumberDecimalDigits")
                        .HasColumnType("int");

                    b.Property<string>("Prefix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrefixExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SelectDefaultValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suffix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SuffixExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<int?>("ValidationPattern")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FKFormPageFieldPKId");

                    b.HasIndex("FKFormPagePKId");

                    b.ToTable("FormPageFields", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FKFormStructurePKId")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IntroScript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IntroText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FKFormStructurePKId");

                    b.ToTable("FormPages", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormStructures", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompletionMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FormStatus")
                        .HasColumnType("int");

                    b.Property<int>("FormStructureType")
                        .HasColumnType("int");

                    b.Property<string>("IntroScript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IntroductionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("UrlIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FormStructures", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.LookUp.LookUpCodeValues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FKLookUpCodePKId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LookUpValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FKLookUpCodePKId");

                    b.ToTable("LookUpCodeValues", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.LookUp.LookUpCodes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("LookUpCodeType")
                        .HasColumnType("int");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LookUpCodes", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.Setting.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FKDeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FKLastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OriginalValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SettingType")
                        .HasColumnType("int");

                    b.Property<string>("SettingValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Settings", "dbo");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFieldOptions", b =>
                {
                    b.HasOne("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFields", "FormPageField")
                        .WithMany("FormPageFieldOptions")
                        .HasForeignKey("FKFormPageFieldPKId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FormPageField");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFields", b =>
                {
                    b.HasOne("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFields", "FormPageField")
                        .WithMany()
                        .HasForeignKey("FKFormPageFieldPKId");

                    b.HasOne("Microsoft.Teams.Assist.Domain.FormDesigner.FormPages", "FormPage")
                        .WithMany("FormPageField")
                        .HasForeignKey("FKFormPagePKId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FormPage");

                    b.Navigation("FormPageField");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPages", b =>
                {
                    b.HasOne("Microsoft.Teams.Assist.Domain.FormDesigner.FormStructures", "FormStructure")
                        .WithMany("FormPages")
                        .HasForeignKey("FKFormStructurePKId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FormStructure");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.LookUp.LookUpCodeValues", b =>
                {
                    b.HasOne("Microsoft.Teams.Assist.Domain.LookUp.LookUpCodes", "LookUpCode")
                        .WithMany("LookUpCodeValues")
                        .HasForeignKey("FKLookUpCodePKId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LookUpCode");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPageFields", b =>
                {
                    b.Navigation("FormPageFieldOptions");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormPages", b =>
                {
                    b.Navigation("FormPageField");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.FormDesigner.FormStructures", b =>
                {
                    b.Navigation("FormPages");
                });

            modelBuilder.Entity("Microsoft.Teams.Assist.Domain.LookUp.LookUpCodes", b =>
                {
                    b.Navigation("LookUpCodeValues");
                });
#pragma warning restore 612, 618
        }
    }
}
