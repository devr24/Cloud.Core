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
        /// <summary>Address in a single displayable format.</summary>
        string DisplayAddress { get; set; }

        /// <summary>Main address.</summary>
        string AddressLine1 { get; set; }

        /// <summary>Array of address parts formatted by address line number.</summary>
        List<string> FormattedAddress { get; set; }

        /// <summary>Thoroughfare for the address.</summary>
        string Thoroughfare { get; set; }

        /// <summary>Building number.</summary>
        string BuildingNumber { get; set; }

        /// <summary>Building name.</summary>
        string BuildingName { get; set; }

        /// <summary>Sub building number.</summary>
        string SubBuildingNumber { get; set; }

        /// <summary>Sub building name.</summary>
        string SubBuildingName { get; set; }

        /// <summary>First line of address.</summary>
        string Line1 { get; set; }

        /// <summary>Second line of address.</summary>
        string Line2 { get; set; }

        /// <summary>Third line of address.</summary>
        string Line3 { get; set; }

        /// <summary>Fourth line of address.</summary>
        string Line4 { get; set; }

        /// <summary>Address locality.</summary>
        string Locality { get; set; }

        /// <summary>Town or city the address resides within.</summary>
        string TownOrCity { get; set; }

        /// <summary>The county the address resides within.</summary>
        string County { get; set; }

        /// <summary>District the address resides within.</summary>
        string District { get; set; }

        /// <summary>Country the address resides within.</summary>
        string Country { get; set; }
    }
}
