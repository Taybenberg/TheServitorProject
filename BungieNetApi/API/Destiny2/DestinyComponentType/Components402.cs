using System.Collections.Generic;
using System.Runtime.Serialization;

namespace API.Destiny2.DestinyComponentType.Components402
{

    public class Rootobject
    {
        public Response Response { get; set; }

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

    public class Response
    {
        public Sales sales { get; set; }
    }

    public class Sales
    {
        public Dictionary<string, Vendor> data { get; set; }

        [IgnoreDataMember]
        public int privacy { get; set; }
    }

    public class Vendor
    {
        public Dictionary<string, Item> saleItems { get; set; }
    }

    public class Item
    {
        [IgnoreDataMember]
        public int vendorItemIndex { get; set; }

        public long itemHash { get; set; }

        [IgnoreDataMember]
        public int quantity { get; set; }
        [IgnoreDataMember]
        public Cost[] costs { get; set; }
    }

    public class Cost
    {
        public int itemHash { get; set; }
        public int quantity { get; set; }
    }

    public class Messagedata
    {
    }

}
