using System;

namespace sprd_net_api.exception
{
    public class ApiException : Exception
    {
        public string Reason { get; set; }
        public string ResultContent { get; set; }

        public ApiException(string reason, string resultContent)
        {
            Reason = reason;
            ResultContent = resultContent;
        }
    }
}