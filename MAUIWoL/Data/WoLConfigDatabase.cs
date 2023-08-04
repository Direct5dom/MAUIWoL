using SQLite;
using MAUIWoL.Models;

namespace MAUIWoL.Data;

public class WoLConfigDatabase
{
    SQLiteAsyncConnection Database;
    public WoLConfigDatabase()
    {
    }
    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<WoLConfig>();
    }

    public async Task<List<WoLConfig>> GetItemsAsync()
    {
        await Init();
        return await Database.Table<WoLConfig>().ToListAsync();
    }

    //public async Task<List<WoLConfig>> GetItemsNotDoneAsync()
    //{
    //    await Init();
    //    return await Database.Table<WoLConfig>().Where(t => t.Done).ToListAsync();
        
    //    // SQL queries are also possible
    //    //return await Database.QueryAsync<WoLConfig>("SELECT * FROM [WoLConfig] WHERE [Done] = 0");
    //}

    public async Task<WoLConfig> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<WoLConfig>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(WoLConfig item)
    {
        await Init();
        if (item.ID != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(WoLConfig item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}