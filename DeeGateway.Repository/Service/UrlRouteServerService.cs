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
    public class UrlRouteServerService
    {
        private SqlSugarClient _db;

        public UrlRouteServerService()
        {
            this._db = DBInstance.GetInstance();
        }

        //public Task<List<urlroute_server>> Get()
        //{
        //    return this._db.Queryable<urlroute_server>().OrderBy(s => s.id, OrderByType.Asc).ToListAsync();//.ToPageListAsync(pageIndex, pageSize);
        //}
        //public Task<List<urlroute_server>> Get(Enums.IsEnable enable, int pageIndex, int pageSize)
        //{
        //    return this._db.Queryable<urlroute_server>()
        //        .Where(s => s.enable == Convert.ToInt32(enable))
        //        .OrderBy(s => s.id, OrderByType.Asc)
        //        .ToPageListAsync(pageIndex, pageSize);
        //}

        //public Task<List<urlroute_server_ext>> GetServer(int pageIndex, int pageSize)
        //{
        //    return this._db.Queryable<urlroute_server, server, urlroute>((us, s, u) =>
        //       new object[] { JoinType.Left, us.server_id == s.id 
        //                      ,JoinType.Left,us.urlroute_id == u.id  })
        //      .Select<urlroute_server_ext>()
        //      .OrderBy(s => s.id, OrderByType.Asc)
        //      .ToPageListAsync(pageIndex, pageSize);
        //}
        public Task<List<urlroute_server_ext>> GetServer(Enums.IsEnable enable)
        {
            return this._db.Queryable<urlroute_server, server,urlroute>((us,s,u) =>
               new object[] { JoinType.Left, us.server_id == s.id 
                             ,JoinType.Left,us.urlroute_id == u.id  })              
               .Where((us, s, u) => us.enable == Convert.ToInt32(enable) && u.enable == Convert.ToInt32(enable))
                .Select<urlroute_server_ext>()
               .OrderBy(us => us.urlroute_id, OrderByType.Asc)
               .ToListAsync();
        }
        /// <summary>
        /// 获取urlroute_server的行数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<int> GetUsedServer(int[] ids)
        {
            return this._db.Queryable<urlroute_server>()
                .In(s=>s.server_id,ids)
                .CountAsync();
        }

        /// <summary>
        /// 获取urlroute_server的行数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<int> GetUsedUrlRoute(int[] ids)
        {
            return this._db.Queryable<urlroute_server>()
                .In(s => s.urlroute_id, ids)
                .CountAsync();
        }


        public Task<List<urlroute_server_ext>> ListUrlRouteServer(int page, int limit, string sortField, string orderType, string enable, int urlroute_id, RefAsync<int> totalCount)
        {
            return this._db.Queryable<urlroute_server, server, urlroute>((us, s, u) =>
               new object[] { JoinType.Left, us.server_id == s.id
                             ,JoinType.Left,us.urlroute_id == u.id  })
                .WhereIF(!string.IsNullOrEmpty(enable), (us, s, u) => us.enable == Convert.ToInt32(enable))
                .Where((us, s, u) =>us.urlroute_id == urlroute_id)
                .Select<urlroute_server_ext>()
                .OrderBy(sortField + " " + orderType)
                .ToPageListAsync(page, limit, totalCount);
        }

        public Task<urlroute_server_ext> UrlRouteServerInfo(int id)
        {
            return this._db.Queryable<urlroute_server, server, urlroute>((us, s, u) =>
              new object[] { JoinType.Left, us.server_id == s.id
                             ,JoinType.Left,us.urlroute_id == u.id  })
               .Where((us, s, u) => us.id == id)
               .Select<urlroute_server_ext>()
               .FirstAsync();
        }

        public Task<int> UrlRouteServerUpdate(urlroute_server UrlRouteServer)
        {
            return this._db.Updateable<urlroute_server>(UrlRouteServer).ExecuteCommandAsync();
        }

        public Task<int> UrlRouteServerInsert(urlroute_server UrlRouteServer)
        {
            return this._db.Insertable<urlroute_server>(UrlRouteServer).ExecuteCommandAsync();
        }

        public Task<int> UrlRouteServerDelete(int[] ids)
        {
            return this._db.Deleteable<urlroute_server>().In(ids).ExecuteCommandAsync();
        }

    }
}
