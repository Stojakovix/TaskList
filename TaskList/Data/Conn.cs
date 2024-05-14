using SQLite;
using TaskList.Model;
using System.Diagnostics;

namespace TaskList.Data
{
    
    public class Conn
    {
        SQLiteAsyncConnection db;

        public Conn()
        {
            Debug.WriteLine(Constants.DatabasePath);
        }
        async Task Init()
        {
            try
            {
                if (db != null)
                    return;

                db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                var result = await db.CreateTableAsync<TaskItem>();
                Debug.WriteLine($"{Constants.DatabasePath}" + " database path to the left");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<List<TaskItem>> GetItemsAsync()
        {
            await Init();
            return await db.Table<TaskItem>().ToListAsync();
        }
        public async Task<List<TaskItem>> GetNotDone()
        {
            await Init();
            return await db.Table<TaskItem>().Where(t => t.IsCompleted).ToListAsync();
        }
        public async Task<List<TaskItem>>GetDone()
        {
            await Init();
            return await db.Table<TaskItem>().Where(t =>t.IsCompleted!).ToListAsync();
        }
        public async Task<TaskItem> GetItemAsync(int id)
        {
            await Init();
            
            return await db.Table<TaskItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveItemAsync(TaskItem item)
        {
            await Init();
            if (item.Id != 0)
                return await db.UpdateAsync(item);
            else
                return await db.InsertAsync(item);
        }
        public async Task<int> DeleteItemAsync(TaskItem item)
        {
            await Init();
            return await db.DeleteAsync(item);
        }

        public async Task<List<TaskItem>> FindItems(string query)
        {
            await Init();
            List<TaskItem> foundItems = await db.QueryAsync<TaskItem>(
                "SELECT * FROM TaskItem WHERE Name LIKE '%' || ? || '%' OR Description LIKE '%' || ? || '%' OR DateTime LIKE '%' || ? || '%'", query, query, query);
            Debug.WriteLine(foundItems.Count);
            return foundItems;
            // Zanimljiv potencijalni problem, kako prikazati sve koji imaju 1 na  "DONE" taskovi, a ne promjeniti im visibility i/ili ga vratiti nazad na kraju querija, a da bude u istom ListViewu
        }
    }
}
