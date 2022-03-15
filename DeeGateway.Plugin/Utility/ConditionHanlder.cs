using BeetleX.FastHttpApi;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.PluginModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Bumblebee.Events;

namespace DeeGateway.Plugin.Utility
{
    public class ConditionHanlder
    {
        /**
        public static T IsMatch<T>( List<T> RuleList, EventAgentRequestingArgs e ) where T:base_rule
        {
            var Request = e.Request;
            var Server = e.Server;
            bool isMatch = false;
            foreach (var rule in RuleList)
            {
                foreach (Condition h in rule.condition_value)
                {
                    switch (h.type)
                    {
                        case Enums.ConditionValueType.URI:
                            if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                            {
                                isMatch = Regex.IsMatch(Request.Url, h.value);
                            }
                            else
                            {
                                isMatch = !Regex.IsMatch(Request.Url, h.value);
                            }
                            break;
                        case Enums.ConditionValueType.IP:
                            isMatch = Request.RemoteIPAddress == h.value;
                            break;
                        case Enums.ConditionValueType.QUERY:

                            break;
                        case Enums.ConditionValueType.HEADER:
                            isMatch = Request.Header[h.key] == h.value;
                            break;
                        case Enums.ConditionValueType.BODY:
                            var bodyValue = e.Request.Data["body"];
                            if (!string.IsNullOrEmpty(bodyValue))
                            {
                                switch ((Enums.ConditionValueHandle)h.handle)
                                {
                                    case Enums.ConditionValueHandle.MATCH:
                                        isMatch = Regex.IsMatch(bodyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.NOT_MATCH:
                                        isMatch = !Regex.IsMatch(bodyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.CONTAINS:
                                        isMatch = bodyValue.Contains(h.value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Enums.ConditionValueType.SERVER_URI:
                            if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                            {
                                isMatch = Regex.IsMatch(Server.Uri.ToString(), h.value);
                            }
                            else if (h.handle == (int)Enums.ConditionValueHandle.NOT_MATCH)
                            {
                                isMatch = !Regex.IsMatch(Server.Uri.ToString(), h.value);
                            }
                            else if (h.handle == (int)Enums.ConditionValueHandle.EQUAL_TO)
                            {
                                isMatch = Server.Uri.ToString() == h.value;
                            }
                            else if (h.handle == (int)Enums.ConditionValueHandle.CONTAINS)
                            {
                                isMatch = Server.Uri.ToString().Contains( h.value);
                            }
                            break;
                        default:
                            break;
                    }

                    if (rule.condition_type == (int)Enums.ConditionType.ONE)
                    {
                        if (isMatch) break;
                    }
                    else if (rule.condition_type == (int)Enums.ConditionType.ALL)
                    {
                        if (isMatch == false) break;
                    }
                }


                if (isMatch) return rule;
            }
            return null;
        }

        public static Condition IsMatchExtractor<T>(T rule, EventAgentRequestingArgs e) where T : base_rule
        {
            var Request = e.Request;
            var Server = e.Server;
            bool isMatch = false;

            foreach (Condition h in rule.extractor)
            {
                switch (h.type)
                {
                    case Enums.ConditionValueType.URI:
                        if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                        {
                            isMatch = Regex.IsMatch(Request.Url, h.value);
                        }
                        else
                        {
                            isMatch = !Regex.IsMatch(Request.Url, h.value);
                        }
                        break;
                    case Enums.ConditionValueType.IP:
                        isMatch = Request.RemoteIPAddress == h.value;
                        break;
                    case Enums.ConditionValueType.QUERY:
                        //获取所有key
                        //h.key
                        var keyValue = e.Request.Data[h.key];
                        if (!string.IsNullOrWhiteSpace(keyValue))
                        {
                            switch ((Enums.ConditionValueHandle)h.handle)
                            {
                                case Enums.ConditionValueHandle.MATCH:
                                    isMatch = Regex.IsMatch(keyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.NOT_MATCH:
                                    isMatch = !Regex.IsMatch(keyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.EQUAL_TO:
                                    isMatch = keyValue == h.value;
                                    break;
                                case Enums.ConditionValueHandle.CONTAINS:
                                    isMatch = keyValue.Contains(h.value);
                                    break;
                                default:
                                    break;
                            }
                        }
                      
                        break;
                    case Enums.ConditionValueType.HEADER:
                        isMatch = Request.Header[h.key] == h.value;
                        break;
                    case Enums.ConditionValueType.BODY:
                        var bodyValue = e.Request.Data["body"];
                        if (!string.IsNullOrEmpty(bodyValue))
                        {
                            switch ((Enums.ConditionValueHandle)h.handle)
                            {
                                case Enums.ConditionValueHandle.MATCH:
                                    isMatch = Regex.IsMatch(bodyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.NOT_MATCH:
                                    isMatch = !Regex.IsMatch(bodyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.CONTAINS:
                                    isMatch = bodyValue.Contains(h.value);
                                    break;
                                default:
                                    break;
                            }
                        }
                        
                        break;
                    case Enums.ConditionValueType.SERVER_URI:
                        if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                        {
                            isMatch = Regex.IsMatch(Server.Uri.ToString(), h.value);
                        }
                        else if (h.handle == (int)Enums.ConditionValueHandle.NOT_MATCH)
                        {
                            isMatch = !Regex.IsMatch(Server.Uri.ToString(), h.value);
                        }
                        else if (h.handle == (int)Enums.ConditionValueHandle.EQUAL_TO)
                        {
                            isMatch = Server.Uri.ToString() == h.value;
                        }
                        else if (h.handle == (int)Enums.ConditionValueHandle.CONTAINS)
                        {
                            isMatch = Server.Uri.ToString().Contains(h.value);
                        }
                        break;
                    default:
                        break;
                }

                if (isMatch)
                {
                    return h;
                }
             
            }

            return null;
        }


    **/
     
        public static string[] GetExtractor<T>(T Rule, HttpRequest request) where T : base_rule
        {
            List<string> values;
            if (Rule.extractor?.Length > 0)
            {
                values = new List<string>(Rule.extractor.Length);
                foreach (Condition h in Rule.extractor)
                {
                    switch (h.type)
                    {
                        case Enums.ConditionValueType.URI:

                            var match = Regex.Match(request.Url, h.value);
                            if (null != match)
                            {
                                var length = match.Groups.Count;
                                for (int i = 1; i < length; i++)
                                {
                                    values.Add(match.Groups[i].Value);
                                }
                            }

                            break;
                        case Enums.ConditionValueType.IP:
                            values.Add(request.RemoteIPAddress);
                            break;
                        case Enums.ConditionValueType.QUERY:
                            values.Add(request.Data[h.key]);
                            break;
                        case Enums.ConditionValueType.HEADER:
                            if (h.key == "Host")
                            {
                                values.Add(request.Header["Host"]?.Split(':')[0]);
                            }
                            else
                            {
                                values.Add(request.Header[h.key]);
                            }
                            break;
                        case Enums.ConditionValueType.BODY:
                            request.Data.TryGetString("body", out var body);
                            if (!string.IsNullOrWhiteSpace(body))
                            {
                                var matchBody = Regex.Match(body, h.value);
                                if (null != matchBody)
                                {
                                    var length = matchBody.Groups.Count;
                                    for (int i = 1; i < length; i++)
                                    {
                                        values.Add(matchBody.Groups[i].Value);
                                    }
                                }
                            }


                            break;
                        //case Enums.ConditionValueType.SERVER_URI:
                        //    values.Add(e.Server.Uri.ToString());
                        //    break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                values = new List<string>();
            }
          
            return values.ToArray();
        }


        public static T IsMatch<T>(List<T> RuleList, HttpRequest request) where T : base_rule
        {
            var Request = request;
            //var Server = e.Server;
            bool isMatch = false;
            foreach (var rule in RuleList)
            {
                foreach (Condition h in rule.condition_value)
                {
                    switch (h.type)
                    {
                        case Enums.ConditionValueType.URI:
                            if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                            {
                                isMatch = Regex.IsMatch(Request.Url, h.value);
                            }
                            else
                            {
                                isMatch = !Regex.IsMatch(Request.Url, h.value);
                            }
                            break;
                        case Enums.ConditionValueType.IP:
                            isMatch = Request.RemoteIPAddress == h.value;
                            break;
                        case Enums.ConditionValueType.QUERY:
                            var keyValue = Request.Data[h.key];
                            if (!string.IsNullOrWhiteSpace(keyValue))
                            {
                                switch ((Enums.ConditionValueHandle)h.handle)
                                {
                                    case Enums.ConditionValueHandle.MATCH:
                                        isMatch = Regex.IsMatch(keyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.NOT_MATCH:
                                        isMatch = !Regex.IsMatch(keyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.EQUAL_TO:
                                        isMatch = keyValue == h.value;
                                        break;
                                    case Enums.ConditionValueHandle.CONTAINS:
                                        isMatch = keyValue.Contains(h.value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Enums.ConditionValueType.HEADER:
                            
                            if (h.key == "Host")
                            {
                                isMatch = Request.Header["Host"]?.Split(':')[0] == h.value;  
                            }
                            else
                            {
                                isMatch = Request.Header[h.key] == h.value;
                            }
                            break;
                        case Enums.ConditionValueType.BODY:
                            var bodyValue = Request.Data["body"];
                            if (!string.IsNullOrEmpty(bodyValue))
                            {
                                switch ((Enums.ConditionValueHandle)h.handle)
                                {
                                    case Enums.ConditionValueHandle.MATCH:
                                        isMatch = Regex.IsMatch(bodyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.NOT_MATCH:
                                        isMatch = !Regex.IsMatch(bodyValue, h.value);
                                        break;
                                    case Enums.ConditionValueHandle.CONTAINS:
                                        isMatch = bodyValue.Contains(h.value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        //case Enums.ConditionValueType.SERVER_URI:
                        //    if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                        //    {
                        //        isMatch = Regex.IsMatch(Server.Uri.ToString(), h.value);
                        //    }
                        //    else if (h.handle == (int)Enums.ConditionValueHandle.NOT_MATCH)
                        //    {
                        //        isMatch = !Regex.IsMatch(Server.Uri.ToString(), h.value);
                        //    }
                        //    else if (h.handle == (int)Enums.ConditionValueHandle.EQUAL_TO)
                        //    {
                        //        isMatch = Server.Uri.ToString() == h.value;
                        //    }
                        //    else if (h.handle == (int)Enums.ConditionValueHandle.CONTAINS)
                        //    {
                        //        isMatch = Server.Uri.ToString().Contains(h.value);
                        //    }
                        //    break;
                        default:
                            break;
                    }

                    if (rule.condition_type == (int)Enums.ConditionType.ONE)
                    {
                        if (isMatch) break;
                    }
                    else if (rule.condition_type == (int)Enums.ConditionType.ALL)
                    {
                        if (isMatch == false) break;
                    }
                }


                if (isMatch) return rule;
            }
            return null;
        }

        public static Condition IsMatchExtractor<T>(T rule, HttpRequest request) where T : base_rule
        {
            var Request = request;
            bool isMatch = false;

            foreach (Condition h in rule.extractor)
            {
                switch (h.type)
                {
                    case Enums.ConditionValueType.URI:
                        if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                        {
                            isMatch = Regex.IsMatch(Request.Url, h.value);
                        }
                        else
                        {
                            isMatch = !Regex.IsMatch(Request.Url, h.value);
                        }
                        break;
                    case Enums.ConditionValueType.IP:
                        isMatch = Request.RemoteIPAddress == h.value;
                        break;
                    case Enums.ConditionValueType.QUERY:
                        //获取所有key
                        //h.key
                        var keyValue = Request.Data[h.key];
                        if (!string.IsNullOrWhiteSpace(keyValue))
                        {
                            switch ((Enums.ConditionValueHandle)h.handle)
                            {
                                case Enums.ConditionValueHandle.MATCH:
                                    isMatch = Regex.IsMatch(keyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.NOT_MATCH:
                                    isMatch = !Regex.IsMatch(keyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.EQUAL_TO:
                                    isMatch = keyValue == h.value;
                                    break;
                                case Enums.ConditionValueHandle.CONTAINS:
                                    isMatch = keyValue.Contains(h.value);
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case Enums.ConditionValueType.HEADER:
                        if (h.key == "Host")
                        {
                            isMatch = Request.Header["Host"]?.Split(':')[0] == h.value;
                        }
                        else
                        {
                            isMatch = Request.Header[h.key] == h.value;
                        }
                        break;
                    case Enums.ConditionValueType.BODY:
                        var bodyValue = Request.Data["body"];
                        if (!string.IsNullOrEmpty(bodyValue))
                        {
                            switch ((Enums.ConditionValueHandle)h.handle)
                            {
                                case Enums.ConditionValueHandle.MATCH:
                                    isMatch = Regex.IsMatch(bodyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.NOT_MATCH:
                                    isMatch = !Regex.IsMatch(bodyValue, h.value);
                                    break;
                                case Enums.ConditionValueHandle.CONTAINS:
                                    isMatch = bodyValue.Contains(h.value);
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    //case Enums.ConditionValueType.SERVER_URI:
                    //    if (h.handle == (int)Enums.ConditionValueHandle.MATCH)
                    //    {
                    //        isMatch = Regex.IsMatch(Server.Uri.ToString(), h.value);
                    //    }
                    //    else if (h.handle == (int)Enums.ConditionValueHandle.NOT_MATCH)
                    //    {
                    //        isMatch = !Regex.IsMatch(Server.Uri.ToString(), h.value);
                    //    }
                    //    else if (h.handle == (int)Enums.ConditionValueHandle.EQUAL_TO)
                    //    {
                    //        isMatch = Server.Uri.ToString() == h.value;
                    //    }
                    //    else if (h.handle == (int)Enums.ConditionValueHandle.CONTAINS)
                    //    {
                    //        isMatch = Server.Uri.ToString().Contains(h.value);
                    //    }
                    //    break;
                    default:
                        break;
                }

                if (isMatch)
                {
                    return h;
                }

            }

            return null;
        }


    }
}
