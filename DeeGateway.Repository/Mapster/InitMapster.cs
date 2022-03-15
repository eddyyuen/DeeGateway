using DeeGateway.Repository.CustomModel;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.PluginModel;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace DeeGateway.Repository.Mapster
{
    public class InitMapster
    {
        public static void Init()
        {
            TypeAdapterConfig<plugin_rule, jwt_auth_rule_ext>
                .NewConfig()
                .Map(dest => dest.condition_value, src => JsonConvert.DeserializeObject<List<Condition>>(src.condition_value))
                .Map(dest => dest.extractor, src => JsonConvert.DeserializeObject<List<Condition>>(src.extractor))
                .Map(dest => dest.handle, src => JsonConvert.DeserializeObject<JwtAuthHandle>(src.handle));

            TypeAdapterConfig<plugin_rule, rewrite_rule_ext>
               .NewConfig()
               .Map(dest => dest.condition_value, src => JsonConvert.DeserializeObject<List<Condition>>(src.condition_value))
               .Map(dest => dest.extractor, src => JsonConvert.DeserializeObject<List<Condition>>(src.extractor))
               .Map(dest => dest.handle, src => JsonConvert.DeserializeObject<RewriteHandle>(src.handle));

            TypeAdapterConfig<plugin_rule, header_rule_ext>
              .NewConfig()
              .Map(dest => dest.condition_value, src => JsonConvert.DeserializeObject< List<Condition>>(src.condition_value))
              .Map(dest => dest.extractor, src => JsonConvert.DeserializeObject<List<Condition>>(src.extractor))
              .Map(dest => dest.handle, src => JsonConvert.DeserializeObject<List<HeaderHandle>>(src.handle));

            TypeAdapterConfig<plugin_rule, ipv4_ratelimit_rule_ext>
             .NewConfig()
             .Map(dest => dest.condition_value, src => JsonConvert.DeserializeObject<List<Condition>>(src.condition_value))
             .Map(dest => dest.extractor, src => JsonConvert.DeserializeObject<List<Condition>>(src.extractor))
             .Map(dest => dest.handle, src => JsonConvert.DeserializeObject<List<IPRateLimitHandle>>(src.handle));

            TypeAdapterConfig<plugin_rule, url_ratelimit_rule_ext>
             .NewConfig()
             .Map(dest => dest.condition_value, src => JsonConvert.DeserializeObject<List<Condition>>(src.condition_value))
             .Map(dest => dest.extractor, src => JsonConvert.DeserializeObject<List<Condition>>(src.extractor))
             .Map(dest => dest.handle, src => JsonConvert.DeserializeObject<List<UrlRateLimitHandle>>(src.handle));
        }
    }
}
