using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccessLibrary.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Enrollee> Enrollees { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Financing> Financing { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<StudyPeriod> StudyPeriods { get; set; }
        public DbSet<SpecialityName> SpecialityNames { get; set; }
        public DbSet<Enrollee_Specialities> Enrollees_Specialities { get; set; }

        public AppDbContext()
        {
            //Database.EnsureCreated();
            //Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=EnrolleeDb;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EducationConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialityConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialityNamesConfiguration());
            modelBuilder.ApplyConfiguration(new StudyPeriodConfiguration());
            modelBuilder.ApplyConfiguration(new FinancingConfiguration());


            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");
        }
    }

    #region Education configuration

    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasData(new Education { EducationId = 1, EducationName = "Основное общее образование" });
            builder.HasData(new Education { EducationId = 2, EducationName = "Среднее общее образование" });
        }
    }

    #endregion

    #region Status configuration

    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasData(new Status { StatusId = 1, StatusName = "Принято" });
            builder.HasData(new Status { StatusId = 2, StatusName = "Подано" });
            builder.HasData(new Status { StatusId = 3, StatusName = "Отказано" });
            builder.HasData(new Status { StatusId = 4, StatusName = "Ожидается" });
        }
    }

    #endregion

    #region Role configuration

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new Role() { RoleId = 1, RoleName = "Абитуриент" });
            builder.HasData(new Role() { RoleId = 2, RoleName = "Администратор" });
        }
    }

    #endregion

    #region Speciality configuration

    public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.HasData(new Speciality { SpecialityId = 1, SpecialityCode = "15.01", NameId = 1, StudyPeriodId = 1, MinimumEducationLevel = 1, FinancingId = 1 });
            builder.HasData(new Speciality { SpecialityId = 2, SpecialityCode = "15.02", NameId = 1, StudyPeriodId = 1, MinimumEducationLevel = 1, FinancingId = 2 });

            builder.HasData(new Speciality { SpecialityId = 3, SpecialityCode = "16.01", NameId = 2, StudyPeriodId = 2, MinimumEducationLevel = 1, FinancingId = 1 });
            builder.HasData(new Speciality { SpecialityId = 4, SpecialityCode = "16.02", NameId = 2, StudyPeriodId = 2, MinimumEducationLevel = 1, FinancingId = 2 });

            builder.HasData(new Speciality { SpecialityId = 5, SpecialityCode = "17.01", NameId = 3, StudyPeriodId = 3, MinimumEducationLevel = 1, FinancingId = 1 });
            builder.HasData(new Speciality { SpecialityId = 6, SpecialityCode = "17.02", NameId = 3, StudyPeriodId = 3, MinimumEducationLevel = 1, FinancingId = 2 });

            builder.HasData(new Speciality { SpecialityId = 7, SpecialityCode = "18.01", NameId = 4, StudyPeriodId = 4, MinimumEducationLevel = 2, FinancingId = 1 });
            builder.HasData(new Speciality { SpecialityId = 8, SpecialityCode = "18.02", NameId = 4, StudyPeriodId = 4, MinimumEducationLevel = 2, FinancingId = 2 });

            builder.HasData(new Speciality { SpecialityId = 9, SpecialityCode = "19.01", NameId = 5, StudyPeriodId = 4, MinimumEducationLevel = 2, FinancingId = 2 });
        }
    }

    #endregion

    #region Speciality names configuration

    public class SpecialityNamesConfiguration : IEntityTypeConfiguration<SpecialityName>
    {
        public void Configure(EntityTypeBuilder<SpecialityName> builder)
        {
            builder.HasData(new SpecialityName() { Id = 1, Name = "Поварское дело"});
            builder.HasData(new SpecialityName() { Id = 2, Name = "Программирование"});
            builder.HasData(new SpecialityName() { Id = 3, Name = "Туризм на базе 9 классов"});
            builder.HasData(new SpecialityName() { Id = 4, Name = "Туризм на базе 11 классов"});
            builder.HasData(new SpecialityName() { Id = 5, Name = "Системное администрирование на базе 11 классов"});
        }
    }

    #endregion

    #region Study period configuration

    public class StudyPeriodConfiguration : IEntityTypeConfiguration<StudyPeriod>
    {
        public void Configure(EntityTypeBuilder<StudyPeriod> builder)
        {
            builder.HasData(new StudyPeriod() { Id = 1, Period = "1 год 10 мес." });
            builder.HasData(new StudyPeriod() { Id = 2, Period = "3 года 11 мес." });
            builder.HasData(new StudyPeriod() { Id = 3, Period = "2 года 5 мес." });
            builder.HasData(new StudyPeriod() { Id = 4, Period = "1 год 5 мес." });
        }
    }

    #endregion

    #region Financing configuration

    public class FinancingConfiguration : IEntityTypeConfiguration<Financing>
    {
        public void Configure(EntityTypeBuilder<Financing> builder)
        {
            builder.HasData(new Financing() { FinancingId = 1, FinancingName = "Бюджетное финансирование" });
            builder.HasData(new Financing() { FinancingId = 2, FinancingName = "Коммерческое финансирование" });
        }
    }

    #endregion

}