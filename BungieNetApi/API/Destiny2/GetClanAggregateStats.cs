using System.Runtime.Serialization;

namespace API.Destiny2.GetClanAggregateStats
{
    public class Rootobject
    {
        public Response[] Response { get; set; }

        [IgnoreDataMember]
        public int ErrorCode { get; set; }
        [IgnoreDataMember]
        public int ThrottleSeconds { get; set; }
        [IgnoreDataMember]
        public string ErrorStatus { get; set; }
        [IgnoreDataMember]
        public string Message { get; set; }
        [IgnoreDataMember]
        public Messagedata MessageData { get; set; }
    }

    public class Messagedata
    {
    }

    public class Response
    {
        [IgnoreDataMember]
        public int mode { get; set; }

        public string statId { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public Basic basic { get; set; }
    }

    public class Basic
    {
        [IgnoreDataMember]
        public float value { get; set; }
        public string displayValue { get; set; }
    }
}
