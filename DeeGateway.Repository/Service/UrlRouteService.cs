using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using DeeGateway.Repository.Model;
using System.Threading.Tasks;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.CustomModel;

namespace DeeGateway.Repository.Service
{
    public class UrlRouteService
    {
        private SqlSugarClient _db;

        public UrlRouteService()
        {
            this._db = DBInstance.GetInstance();
        }

        //public Task<List<urlroute>> Get(int pageIndex, int pageSize)
        //{
        //    return this._db.Queryable<urlroute>().OrderBy(s => s.id, OrderByType.Asc).ToPageListAsync(pageIndex, pageSize);
        //}
        //public Task<List<urlroute>> Get(Enums.IsEnable enable, int pageIndex, int pageSize)
        //{
        //    return this._db.Queryable<urlroute>()
        //        .Where(s => s.enable == Convert.ToInt32(enable))
        //        .OrderBy(s => s.id, OrderByType.Asc)
        //        .ToPageListAsync(pageIndex, pageSize);
        //}
        public Task<List<urlroute>> ListAllUrlRoute(string sortField, string orderType)
        {
            return this._db.Queryable<urlroute>().OrderBy(sortField + " " + orderType).ToListAsync();
        }

        public Task<List<urlroute_ext>> ListUrlRoute(int page, int limit, string sortField, string orderType, string url, string enable, RefAsync<int> totalCount)
        {
            return this._db.Queryable<urlroute, urlroute_server>((u, us) =>
                new object[] { JoinType.Left, u.id == us.urlroute_id && us.enable==1 })
                .WhereIF(!string.IsNullOrEmpty(url), (u, us) => SqlFunc.Contains(u.url, url))
                .WhereIF(!string.IsNullOrEmpty(enable), (u, us) => u.enable == Convert.ToInt32(enable))
               
                .GroupBy((u, us) => new { u.id, u.url, u.hash_pattern, u.enable })
                .Select((u, us) => new urlroute_ext { id = u.id, urlroute_name = u.urlroute_name, url = u.url, hash_pattern = u.hash_pattern, enable = u.enable, active_server = SqlFunc.AggregateCount(us.id) })
                 .OrderBy(sortField + " " + orderType)
                  .ToPageListAsync(page, limit, totalCount);
        }

        public Task<urlroute> UrlRouteInfo(int id)
        {
            return this._db.Queryable<urlroute>()
                .Where(s => s.id == id)
                .FirstAsync();
        }

        public Task<int> UrlRouteUpdate(urlroute UrlRoute)
        {
            return this._db.Updateable<urlroute>(UrlRoute).ExecuteCommandAsync();
        }

        public Task<int> UrlRouteInsert(urlroute UrlRoute)
        {
            return this._db.Insertable<urlroute>(UrlRoute).ExecuteCommandAsync();
        }

        public Task<int> UrlRouteDelete(int[] ids)
        {
            return this._db.Deleteable<urlroute>().In(ids).ExecuteCommandAsync();
        }

        public Task<List<urlroute>> UrlRouteInfos(int[] ids)
        {
            return this._db.Queryable<urlroute>().In(ids).ToListAsync();
        }
    }
}
