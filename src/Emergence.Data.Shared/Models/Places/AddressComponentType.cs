namespace Emergence.Data.Shared.Models.Places
{
    public enum AddressComponentType
    {
        Uknown = 0,
        //
        // Summary:
        //     Indicates a precise street address.
        Street_Address = 1,
        //
        // Summary:
        //     Indicates a named route (such as "US 101").
        Route = 2,
        //
        // Summary:
        //     Indicates a major intersection, usually of two major roads.
        Intersection = 3,
        //
        // Summary:
        //     Indicates a political entity. Usually, this type indicates a polygon of some
        //     civil administration.
        Political = 4,
        //
        // Summary:
        //     Indicates the national political entity, and is typically the highest order type
        //     returned by the Geocoder.
        Country = 5,
        //
        // Summary:
        //     Indicates a first-order civil entity below the country level. Within the United
        //     States, these administrative levels are states. Not all nations exhibit these
        //     administrative levels.
        Administrative_Area_Level_1 = 6,
        //
        // Summary:
        //     Indicates a second-order civil entity below the country level. Within the United
        //     States, these administrative levels are counties. Not all nations exhibit these
        //     administrative levels.
        Administrative_Area_Level_2 = 7,
        //
        // Summary:
        //     Indicates a third-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_3 = 8,
        //
        // Summary:
        //     Indicates a fourth-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_4 = 9,
        //
        // Summary:
        //     Indicates a fifth-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_5 = 10,
        //
        // Summary:
        //     Indicates a commonly-used alternative name for the entity.
        Colloquial_Area = 11,
        //
        // Summary:
        //     Indicates an incorporated city or town political entity.
        Locality = 12,
        //
        // Summary:
        //     Ward indicates a specific type of Japanese locality, to facilitate distinction
        //     between multiple locality components within a Japanese address.
        Ward = 13,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality
        Sublocality = 14,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality
        Sublocality_Level_1 = 15,
        //
        // Summary:
        //     indicates an second-order civil entity below a locality
        SublocalityLevel2 = 16,
        //
        // Summary:
        //     indicates an third-order civil entity below a locality
        Sublocality_Level_3 = 17,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality
        Sublocality_Level_4 = 18,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality
        Sublocality_Level_5 = 19,
        //
        // Summary:
        //     Indicates a named neighborhood
        Neighborhood = 20,
        //
        // Summary:
        //     Indicates a named location, usually a building or collection of buildings with
        //     a common name
        Premise = 21,
        //
        // Summary:
        //     Indicates a first-order entity below a named location, usually a singular building
        //     within a collection of buildings with a common name
        Subpremise = 22,
        //
        // Summary:
        //     Indicates a postal code as used to address postal mail within the country.
        Postal_Code = 23,
        //
        // Summary:
        //     Indicates a prominent natural feature.
        Natural_Feature = 24,
        //
        // Summary:
        //     Indicates an airport.
        Airport = 25,
        //
        // Summary:
        //     Indicates a named park.
        Park = 26,
        //
        // Summary:
        //     Indicates the floor of a building address.
        Floor = 27,
        //
        // Summary:
        //     Establishment typically indicates a place that has not yet been categorized.
        Establishment = 28,
        //
        // Summary:
        //     Indicates a named point of interest. Typically, these "POI"s are prominent local
        //     entities that don't easily fit in another category such as "Empire State Building"
        //     or "Statue of Liberty."
        Point_Of_Interest = 29,
        //
        // Summary:
        //     Parking indicates a parking lot or parking structure.
        Parking = 30,
        //
        // Summary:
        //     post_box indicates a specific postal box.
        Post_Box = 31,
        //
        // Summary:
        //     postal_town indicates a grouping of geographic areas, such as locality and sublocality,
        //     used for mailing addresses in some countries.
        Postal_Town = 32,
        //
        // Summary:
        //     room indicates the room of a building address.
        Room = 33,
        //
        // Summary:
        //     street_number indicates the precise street number.
        Street_Number = 34,
        //
        // Summary:
        //     Indicate the location of a bus stop.
        Bus_Station = 35,
        //
        // Summary:
        //     Indicate the location of a train stop.
        Train_Station = 36,
        //
        // Summary:
        //     Indicate the location of a public transit stop.
        Transit_Station = 37,
        //
        // Summary:
        //     Geocode.
        Geocode = 38,
        //
        // Summary:
        //     Postal code prefix.
        Postal_Code_Prefix = 39,
        //
        // Summary:
        //     Postal code suffix.
        Postal_Code_Suffix = 40
    }
}
