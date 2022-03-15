using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using DeeGateway.Repository.Model;
using System.Threading.Tasks;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.CustomModel;
using Newtonsoft.Json;
using DeeGateway.Repository.PluginModel;
using System.Linq;

namespace DeeGateway.Repository.Service
{
    public class PluginService
    {
        private SqlSugarClient _db;

        public PluginService()
        {
            this._db = DBInstance.GetInstance();
        }

        public Task<List<plugin_rule>> GetRule<T>(Enums.IsEnable enable, int pageIndex, int pageSize) where T : plugin_rule
        {
            RefAsync<int> totalCount = 0;
            return GetRule<T>(enable, pageIndex, pageSize, totalCount);
        }

        public Task<List<plugin_rule>> GetRule<T>(Enums.IsEnable enable, int pageIndex, int pageSize, RefAsync<int> totalCount) where T : plugin_rule
        {

            return GetRule<T>(Convert.ToInt32(enable), pageIndex, pageSize, totalCount);
        }
        public Task<List<plugin_rule>> GetRule<T>(int enable, int pageIndex, int pageSize, RefAsync<int> totalCount, int urlroute_id = 0) where T : plugin_rule
        {
            return this._db.Queryable<T, urlroute>((j, u) =>
                new object[] { JoinType.Left, j.urlroute_id == u.id })
               .WhereIF(enable >= 0, (j, u) => j.enable == enable)
               .WhereIF(urlroute_id > 0, (j, u) => j.urlroute_id == urlroute_id)
               .Select((j, u) => new plugin_rule
               {
                   id = j.id,
                   condition_type = j.condition_type,
                   enable = j.enable,
                   condition_value = j.condition_value.ToString(),
                   extractor = j.extractor.ToString(),
                   handle = j.handle.ToString(),
                   urlroute_id = j.urlroute_id,
                   url = u.url,
                   urlroute_name = u.urlroute_name,
                   title = j.title
               }
                )
              .ToPageListAsync(pageIndex, pageSize, totalCount);

        }

        public Task<plugin_rule> GetRule<T>(int id) where T : plugin_rule
        {
            return this._db.Queryable<T, urlroute>((j, u) =>
                new object[] { JoinType.Left, j.urlroute_id == u.id })
               .Where((j, u) => j.id == id)
               .Select((j, u) => new plugin_rule
               {
                   id = j.id,
                   condition_type = j.condition_type,
                   enable = j.enable,
                   condition_value = j.condition_value.ToString(),
                   extractor = j.extractor.ToString(),
                   handle = j.handle.ToString(),
                   urlroute_id = j.urlroute_id,
                   url = u.url,
                   urlroute_name = u.urlroute_name,
                   title = j.title

               }
                )
               .FirstAsync();
            ;
        }

        #region 编辑
        public int UpdateRule<T>(T value) where T : plugin_rule, new()
        {
            try
            {
                value.condition_value = value.condition_value.Replace("[],", "").Replace("[]", "");
                value.extractor = value.extractor.Replace("[],", "").Replace("[]", "");
                if (string.IsNullOrWhiteSpace(value.condition_value))
                {
                    value.condition_value = "[]";
                }
                if (string.IsNullOrWhiteSpace(value.extractor))
                {
                    value.extractor = "[]";
                }

                var condition_value = Newtonsoft.Json.JsonConvert.DeserializeObject<Condition[]>(value.condition_value);
                var extractor = Newtonsoft.Json.JsonConvert.DeserializeObject<Condition[]>(value.extractor);

                condition_value = (from a in condition_value orderby a.order ascending select a).ToArray();
                extractor = (from a in extractor orderby a.order ascending select a).ToArray();

                value.condition_value = Newtonsoft.Json.JsonConvert.SerializeObject(condition_value);
                value.extractor = Newtonsoft.Json.JsonConvert.SerializeObject(extractor);
                return this._db.Updateable<T>(value).ExecuteCommand();
            }
            catch
            {
                return 0;
            }
        }
        public int DeleteRule<T>(int[] ids) where T : plugin_rule, new()
        {
            try
            {
                return this._db.Deleteable<T>().In(ids).ExecuteCommand();
            }
            catch
            {
                return 0;
            }

        }
        public int InsertRule<T>(T value) where T : plugin_rule, new()
        {
            try
            {
                value.condition_value = value.condition_value.Replace("[],", "").Replace("[]", "");
                value.extractor = value.extractor.Replace("[],", "").Replace("[]", "");
                if (string.IsNullOrWhiteSpace(value.condition_value))
                {
                    value.condition_value = "[]";
                }
                if (string.IsNullOrWhiteSpace(value.extractor))
                {
                    value.extractor = "[]";
                }
                var condition_value = Newtonsoft.Json.JsonConvert.DeserializeObject<Condition[]>(value.condition_value);
                var extractor = Newtonsoft.Json.JsonConvert.DeserializeObject<Condition[]>(value.extractor);

                condition_value = (from a in condition_value orderby a.order ascending select a).ToArray();
                extractor = (from a in extractor orderby a.order ascending select a).ToArray();

                value.condition_value = Newtonsoft.Json.JsonConvert.SerializeObject(condition_value);
                value.extractor = Newtonsoft.Json.JsonConvert.SerializeObject(extractor);

                return this._db.Insertable<T>(value).ExecuteCommand();
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region Plugin
        public Task<List<plugins>> GetPluginsAll()
        {
            RefAsync<int> totalCount = new RefAsync<int>();
            return GetPlugins(1, 9999, "", totalCount);
        }
        public Task<List<plugins>> GetPlugins(int pageIndex, int pageSize, string enabled, RefAsync<int> totalCount)
        {
            if (string.IsNullOrWhiteSpace(enabled))
            {
                enabled = string.Empty;
            }
            else
            {
                enabled = enabled.ToUpper();
            }
            return this._db.Queryable<plugins>()
                 .WhereIF(enabled == "TRUE", p => p.enable == 1)
                 .WhereIF(enabled == "FALSE", p => p.enable == 0)
                 .OrderBy(p => p.level, OrderByType.Desc)
                 .ToPageListAsync(pageIndex, pageSize, totalCount);
        }
        public Task<plugins> PluginInfo(int id)
        {
            return this._db.Queryable<plugins>()
                .Where(s => s.id == id)
                .FirstAsync();
        }
        public Task<plugins> PluginInfo(string name)
        {
            return this._db.Queryable<plugins>()
                .Where(s => s.plugin_name == name)
                .FirstAsync();
        }

        public Task<int> PluginUpdate(plugins Plugin)
        {
            try
            {
                return this._db.Updateable<plugins>(Plugin).ExecuteCommandAsync();
            }
            catch
            {
                return Task.FromResult<int>(0);
            }
        }

        public Task<int> PluginInsert(plugins Plugin)
        {
            try
            {
                return this._db.Insertable<plugins>(Plugin).ExecuteCommandAsync();
            }
            catch
            {
                return Task.FromResult<int>(0);
            }
        }

        public Task<int> PluginDelete(int[] ids)
        {
            try
            {
                return this._db.Deleteable<plugins>().In(ids).ExecuteCommandAsync();
            }
            catch
            {
                return Task.FromResult<int>(0);
            }
        }

        #endregion
    }
}
