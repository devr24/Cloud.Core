namespace Cloud.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IAddressLookup
    /// </summary>
    public interface IAddressLookup
    {
        /// <summary>Searches for addresses using a postcode only.</summary>
        /// <param name="postCode">The postcode.</param>
        /// <returns>Task&lt;AddressResult&gt;.</returns>
        Task<IAddressResult> SearchAddress(string postCode);
        
        /// <summary>Searches for addresses using both the postcode and the house number.</summary>
        /// <param name="postCode">The postcode.</param>
        /// <param name="houseNo">The house no.</param>
        /// <returns>Task&lt;AddressResult&gt;.</returns>
        Task<IAddressResult> SearchAddress(string postCode, string houseNo);
    }

    /// <summary>Interface IAddressResult.</summary>
    public interface IAddressResult
    {
        /// <summary>Postcode used when searching.</summary>
        string Postcode { get; set; }

        /// <summary>House number used when searching.</summary>
        string HouseNumber { get; set; }

        /// <summary>Latitude of the address area.</summary>
        double Latitude {get; set; }

        /// <summary>Lontitude of the address area.</summary>
        double Longitude { get; set; }

        /// <summary>Whether the [address was found] or not.</summary>
        bool AddressFound { get; set; }

        /// <summary>Number of addresses found.</summary>
        int AddressCount { get; set; }

        /// <summary>Addresses found.</summary>
        List<IAddress> Addresses { get; set; }
    }

    /// <summary>Interface IAddress containing a single addresses details.</summary>
    public interface IAddress
    {
        string DisplayAddress { get; set; }

        string AddressLine1 { get; set; }

        List<string> FormattedAddress { get; set; }

        string Thoroughfare { get; set; }

        string BuildingName { get; set; }

        string SubBuildingName { get; set; }

        string SubBuildingNumber { get; set; }

        string BuildingNumber { get; set; }

        string Line1 { get; set; }

        string Line2 { get; set; }

        string Line3 { get; set; }

        string Line4 { get; set; }

        string Locality { get; set; }

        string TownOrCity { get; set; }

        string County { get; set; }

        string District { get; set; }

        string Country { get; set; }
    }
}
