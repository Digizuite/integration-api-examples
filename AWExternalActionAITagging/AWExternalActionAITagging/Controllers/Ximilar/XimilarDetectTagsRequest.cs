using System.Collections.Generic;

namespace AWExternalActionAITagging.Controllers.Ximilar
{
    public class XimilarDetectTagsRequest
    {
        public IEnumerable<XimilarDetectTagsUrl> records;
        public string task;

        public XimilarDetectTagsRequest(IEnumerable<XimilarDetectTagsUrl> urls, string taskId)
        {
            records = urls;
            task = taskId;
        }
        
        public XimilarDetectTagsRequest(IEnumerable<XimilarDetectTagsUrl> urls)
        {
            records = urls;
        }
    }

    public class XimilarDetectTagsUrl
    {
        public string _url;

        public XimilarDetectTagsUrl(string _url)
        {
            this._url = _url;
        }
    }
}