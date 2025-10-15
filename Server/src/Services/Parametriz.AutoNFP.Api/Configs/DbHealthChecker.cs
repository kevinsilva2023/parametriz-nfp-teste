using Microsoft.EntityFrameworkCore;

namespace Parametriz.AutoNFP.Api.Configs
{
    public class DbHealthChecker
    {
        public static async Task TestConnection(DbContext context)
        {
            var maxAttemps = 10;
            var delay = 5000;

            for (int i = 0; i < maxAttemps; i++)
            {
                var canConnect = CanConnect(context);
                if (canConnect)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            throw new Exception("Verifique a conexão com o banco de dados.");
        }

        public static async Task WaitForTable<T>(DbContext context) where T : class
        {
            var maxAttemps = 10;
            var delay = 5000;

            for (int i = 0; i < maxAttemps; i++)
            {
                var tableExist = await CheckTable<T>(context);
                if (tableExist)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            throw new Exception("Verfique se o banco de dados existe.");
        }

        private static bool CanConnect(DbContext context)
        {
            try
            {
                context.Database.GetAppliedMigrations();   // Check the database connection
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> CheckTableExists<T>(DbContext db) where T : class
        {
            await TestConnection(db);
            return await CheckTable<T>(db);
        }

        private static async Task<bool> CheckTable<T>(DbContext db) where T : class
        {
            try
            {
                await db.Set<T>().AnyAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
