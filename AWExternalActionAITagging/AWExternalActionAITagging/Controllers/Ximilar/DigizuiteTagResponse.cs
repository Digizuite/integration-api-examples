using System.Collections.Generic;
using JetBrains.Annotations;

namespace AWExternalActionAITagging.Controllers.Ximilar
{
    public class DigizuiteTagResponse
    {
        public IEnumerable<DetectTagObject> tags;
        public IEnumerable<DetectTagObject> subTags;
        public IEnumerable<DetectSubObject> objects;
        public string status;
        public string errorMessage;
    }
    
    public class DetectTagObject
    {
        public float prob;
        public string name;
        public string id;
        [CanBeNull] public string type;
    }

    public class DetectSubObject
    {
        public float prob;
        public string name;
        public string id;
        public IEnumerable<DetectTagObject> subObjects;
    }
}