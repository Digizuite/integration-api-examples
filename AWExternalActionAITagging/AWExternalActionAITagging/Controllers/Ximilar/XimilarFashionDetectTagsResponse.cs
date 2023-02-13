using System.Collections.Generic;

namespace AWExternalActionAITagging.Controllers.Ximilar
{
    public class XimilarFashionDetectTagsResponse
    {
        public IEnumerable<XimilarFashionDetectTagObjects> records;
    }
    

    public class XimilarFashionDetectTagObjects
    {
        public string _url;
        public object _status;
        public string _id;
        public XimilarFashionDetectObject _tags;
        public IEnumerable<string> _tags_simple;
    }
    
    public class XimilarFashionDetectObject
    {
        public IEnumerable<XimilarFashionDetectTagObject> Category;
        public IEnumerable<XimilarFashionDetectTagObject> Color;
        public IEnumerable<XimilarFashionDetectTagObject> Style;
        public IEnumerable<XimilarFashionDetectTagObject> Length;
        public IEnumerable<XimilarFashionDetectTagObject> Subcategory;
        public IEnumerable<XimilarFashionDetectTagObject> Design;
        public IEnumerable<XimilarFashionDetectTagObject> Closure;
        public IEnumerable<XimilarFashionDetectTagObject> Fastening;
        public IEnumerable<XimilarFashionDetectTagObject> Type;
        

    }
    
    public class XimilarFashionDetectTagObject
    {
        public float prob;
        public string name;
        public string id;
        public string type;
    }
}