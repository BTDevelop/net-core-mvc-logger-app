using ElasticLoggerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticLoggerApp.ElasticSearch;

namespace ElasticLoggerApp.Data
{
    public class ElasticContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=Server_Name;Initial Catalog=Database_Name;Persist Security Info=False;User ID=******;Password=******;Connection Timeout=30;");
            optionsBuilder.UseSqlServer("Server=94.73.170.5;Initial Catalog=u7939276_db4E1;Persist Security Info=False;User ID=u7939276_user4E1;Password=NFtm71Z5;Connection Timeout=30;");
        }

        //SaveChanges() ile dbcontext içerinde değişen tablolar bulunur ve değiştiği alanların ilk ve son halleri kaydedilir.
        public override int SaveChanges()
        {
            try
            {
                var modifiedUnit = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
                var logTime = System.DateTime.UtcNow;

                foreach (var units in modifiedUnit)
                {
                    var entityName = units.Entity.GetType().Name;
                    var primaryKeyColName = units.OriginalValues.Properties.FirstOrDefault(prop => prop.IsPrimaryKey() == true).Name;

                    foreach (IProperty prop in units.OriginalValues.Properties)
                    {
                        var originVal = units.OriginalValues[prop.Name].ToString();
                        var currVal = units.CurrentValues[prop.Name].ToString();

                        if (originVal != currVal)  //değişen var mı?
                        {
                            ModifyLog log = new ModifyLog()
                            {
                                EntityName = entityName,
                                PrimaryKeyColNameVal = int.Parse(units.OriginalValues[primaryKeyColName].ToString()),
                                PropertyName = prop.Name,
                                OriginVal = originVal,
                                NewVal = currVal,
                                LogDate = logTime,
                                State = EnumLogState.Update
                            };
                            //log elasticsearh'e aktarılır.
                            ElasticSearchStrike.CheckExistsAndInsert(log);
                        }
                    }

                }
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return 0;
            }
        }

    }
}
