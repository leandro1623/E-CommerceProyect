using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Datos.Seed
{
    public static class Seeding
    {
        private static List<string> Roles = new List<string>()
        {
            Constantes.Admin,
            Constantes.User
        };


        public static void Aplicar(DbContext dbContext, bool _)
        {
            foreach (var role in Roles)
            {
                var rolDb = dbContext.Set<IdentityRole>().FirstOrDefault(r => r.Name == role);

                if(rolDb == null)
                {
                    dbContext.Set<IdentityRole>().Add(new IdentityRole()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                    });
                }
            }

            dbContext.SaveChanges();
        }


        public async static Task AplicarAsync(DbContext dbContext, bool _, CancellationToken cancellationToken)
        {
            foreach (var role in Roles)
            {
                var rolDb = await dbContext.Set<IdentityRole>().FirstOrDefaultAsync(r => r.Name == role);

                if (rolDb == null)
                {
                    await dbContext.Set<IdentityRole>().AddAsync(new IdentityRole()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                    });
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
