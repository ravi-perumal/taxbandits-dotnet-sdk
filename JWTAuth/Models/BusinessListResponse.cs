using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JWTAuth.Models
{
    [DataContract]
    public class BusinessListResponse : BaseResponseStatus
    {
        /// <summary>
        /// Business Details of all the businesses
        /// </summary>
        [DataMember(Order = 1)]
        public List<Business> Businesses { get; set; }
        /// <summary>
        /// Page number
        /// </summary>
        [DataMember(Order = 2)]
        public int Page { get; set; }
        /// <summary>
        /// Total number of Businesses
        /// </summary>
        [DataMember(Order = 3)]
        public int TotalRecords { get; set; }
        /// <summary>
        /// Total number of pages
        /// </summary>
        [DataMember(Order = 4)]
        public int TotalPages { get; set; }
        /// <summary>
        /// Range: 10, 25, 50, 100
        /// </summary>
        [DataMember(Order = 5)]
        public int PageSize { get; set; }

    }
}
