using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class ApiContextSeed
{
    public static async Task SeedAsync(ApiContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            /* var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); */

            if (!context.TypeFiles.Any())
            {
                /* Console.WriteLine("ruta:"+ruta); */
                 using (var readerTypeFile = new StreamReader("../Persistence/Data/Csvs/Typefile.csv"))
                {
                    using (var csv = new CsvReader(readerTypeFile, CultureInfo.InvariantCulture))
                    {
                        var list = csv.GetRecords<TypeFile>();
                        context.TypeFiles.AddRange(list);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApiContext>();
            logger.LogError(ex.Message);
        }
    }
}
