using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using DeeGateway.Repository.Model;
using System.Threading.Tasks;
using DeeGateway.Repository.Constant;

namespace DeeGateway.Repository.Service
{
    public class ServerService
    {
        private SqlSugarClient _db;

        public ServerService()
        {
            this._db = DBInstance.GetInstance();
        }

        //public Task<List<server>> Get(int pageIndex,int pageSize)
        //{
        //    return this._db.Queryable<server>().OrderBy(s => s.id, OrderByType.Asc).ToPageListAsync(pageIndex, pageSize);
        //}
        public Task<List<server>> GetList(Enums.IsEnable enable)
        {
            return this._db.Queryable<server>()
                .Where(s => s.enable == Convert.ToInt32(enable))
                .OrderBy(s => s.id, OrderByType.Asc)
                .ToListAsync();
        }

        public Task<List<server>> ListServer(int page, int limit, string sortField, string orderType,string uri,string enable, RefAsync<int> totalCount)
        {
            return this._db.Queryable<server>()
                .WhereIF(!string.IsNullOrEmpty(uri),s=>SqlFunc.Contains(s.uri,uri))
                .WhereIF(!string.IsNullOrEmpty(enable), s => s.enable == Convert.ToInt32(enable))
                .OrderBy(sortField + " " + orderType)

                .ToPageListAsync(page, limit, totalCount);
        }

        public Task<server> ServerInfo(int id)
        {
            return this._db.Queryable<server>()
                .Where(s => s.id == id)
                .FirstAsync();
        }

        public Task<int> ServerUpdate(server Server)
        {
            return this._db.Updateable<server>(Server).ExecuteCommandAsync();
        }

        public Task<int> ServerInsert(server Server)
        {
            return this._db.Insertable<server>(Server).ExecuteCommandAsync();
        }

        public Task<int> ServerDelete(int[] ids)
        {
            return this._db.Deleteable<server>().In(ids).ExecuteCommandAsync();
        }

        public Task<List<server>> ServerInfo(int[] ids)
        {
            return this._db.Queryable<server>().In(ids).ToListAsync();
        }
    }
}
