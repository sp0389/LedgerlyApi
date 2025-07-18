using FinanceApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infrastructure.Persistence
{
    public class Seed
    {
        public Seed(ModelBuilder mb)
        {
            InitUsers(mb);
            InitUserRoles(mb);
            InitUserToRole(mb);
        }

        private static void InitUsers(ModelBuilder mb)
        {
            mb.Entity<User>().HasData(
                new User
                {
                    Id = "ca32e0e5-46b8-4f44-9a97-0d685a2c54b2",
                    UserName = "a@a.com",
                    NormalizedUserName = "A@A.COM",
                    Email = "a@a.com",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEHsSevUsbVfCvzTrAPeOAJGAdLJXoClxNuG4OJyPozgYXexeGOqLXgnIxAZgTQTbfA==",
                    SecurityStamp = "M67EBX32EPBJDLSU75U3EA5SFKIR7MDP",
                    ConcurrencyStamp = "3e098325-ba04-4578-8bd8-231bbf8dde66",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                }
            );
        }

        private static void InitUserRoles(ModelBuilder mb)
        {
            mb.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "ae18c1e2-1261-4c99-ace3-039873bb14f2",
                    Name = "Member",
                    NormalizedName = "MEMBER",
                    ConcurrencyStamp = "fac56c34-35bd-49db-84b1-ea12e216462b",
                },
                new IdentityRole
                {
                    Id = "0933e74e-48a0-408e-8cf6-e33a4dbff132",
                    Name = "Adminstrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "d35caf6c-b859-4c1c-95c6-1611649fb285",
                }
            );
        }

        private static void InitUserToRole(ModelBuilder mb)
        {
            mb.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "0933e74e-48a0-408e-8cf6-e33a4dbff132",
                   UserId = "ca32e0e5-46b8-4f44-9a97-0d685a2c54b2"
               }
            );
        }
    }
}