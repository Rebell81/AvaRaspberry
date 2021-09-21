﻿using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Reflection;

namespace SynologyClient
{
    public class FuncProcessor<TResponse> where TResponse : BaseSynologyResponse, new()
    {
        private readonly dynamic _args;
        private readonly dynamic _optionalArgs;
        private readonly string _scriptPath;
        private readonly string _sid;
        public SynologyRestRequest RestRequest;



        public FuncProcessor(string scriptPath, string sid, dynamic args, dynamic optionalArgs = null)
        {
            if (string.IsNullOrWhiteSpace(scriptPath))
                throw new ArgumentNullException("scriptPath");
            if (string.IsNullOrWhiteSpace(sid))
                throw new ArgumentNullException("sid");
            if (args == null)
                throw new ArgumentNullException("args");


            _scriptPath = scriptPath;
            _sid = sid;
            _args = args;
            _optionalArgs = optionalArgs;
        }

        public TResponse Run()
        {
            try
            {
                RestRequest = new SynologyRestRequest();

                AddParametersFromObjectProperties(_args, RestRequest);

                if (_optionalArgs != null)
                    AddParametersFromObjectProperties(_optionalArgs, RestRequest);

                RestRequest.AddParameter("_sid", _sid);

                IRestClient client = new RestClient(config.Conf.ApiBaseAddressAndPathNoTrailingSlash + _scriptPath);
                client.UseNewtonsoftJson();

                var response = client.Execute<TResponse>(RestRequest);


                return response.Data;
            }
            catch (Exception e)
            {
                throw new SynologyClientException(e.Message, e);
            }
        }

        private void AddParametersFromObjectProperties(object src, IRestRequest req)
        {
            var props = src.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
                req.AddParameter(prop.Name, prop.GetValue(src, null));
        }
    }
}