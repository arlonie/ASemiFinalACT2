using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASemiFinalACT2
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<ORDERS>().Wait();
        }
        //ADD and UPDATE records
        public Task<int> Save(ORDERS orders)
        {
            if (orders.SONum != 0)
            {
                return db.UpdateAsync(orders);
            }
            else
            {
                return db.InsertAsync(orders);
            }
        }

        //DELETE
        public Task<int> Delete(ORDERS orders)
        {
            return db.DeleteAsync(orders);
        }

        // DISPLAY ALL or READ ALL 
        public Task<List<ORDERS>> DisplayAll()
        {
            return db.Table<ORDERS>().ToListAsync();
        }

        //SEARCH (specific)
        public Task<ORDERS> Search(int soNum)
        {
            return db.Table<ORDERS>().Where(i => i.SONum == soNum).FirstOrDefaultAsync();    
        }

    }
}
