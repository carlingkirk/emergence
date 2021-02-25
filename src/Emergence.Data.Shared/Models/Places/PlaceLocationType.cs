namespace Emergence.Data.Shared.Models.Places
{
    public enum PlaceLocationType
    {
        //
        // Summary:
        //     Unknown is set when unmapped place location types is returned.
        Uknown = 0,
        //
        // Summary:
        //     Geocode instructs the Place Autocomplete service to return only geocoding results,
        //     rather than business results. Generally, you use this request to disambiguate
        //     results where the location specified may be indeterminate.
        Geocode = 1,
        //
        // Summary:
        //     Indicates a precise street address.
        Street_Address = 2,
        //
        // Summary:
        //     Indicates a named route (such as "US 101").
        Route = 3,
        //
        // Summary:
        //     Indicates a major intersection, usually of two major roads.
        Intersection = 4,
        //
        // Summary:
        //     Indicates a political entity. Usually, this type indicates a polygon of some
        //     civil administration.
        Political = 5,
        //
        // Summary:
        //     Indicates the national political entity, and is typically the highest order type
        //     returned by the Geocoder.
        Country = 6,
        //
        // Summary:
        //     Indicates a first-order civil entity below the country level. Within the United
        //     States, these administrative levels are states. Not all nations exhibit these
        //     administrative levels.
        Administrative_Area_Level_1 = 7,
        //
        // Summary:
        //     Indicates a second-order civil entity below the country level. Within the United
        //     States, these administrative levels are counties. Not all nations exhibit these
        //     administrative levels.
        Administrative_Area_Level_2 = 8,
        //
        // Summary:
        //     Indicates a third-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_3 = 9,
        //
        // Summary:
        //     Indicates a fourth-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_4 = 10,
        //
        // Summary:
        //     Indicates a fifth-order civil entity below the country level. This type indicates
        //     a minor civil division. Not all nations exhibit these administrative levels.
        Administrative_Area_Level_5 = 11,
        //
        // Summary:
        //     Indicates a commonly-used alternative name for the entity.
        Colloquial_Area = 12,
        //
        // Summary:
        //     Indicates an incorporated city or town political entity.
        Locality = 13,
        //
        // Summary:
        //     locality.
        Sublocality = 14,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality.
        Sublocality_Level_1 = 15,
        //
        // Summary:
        //     indicates an second-order civil entity below a locality.
        Sublocality_Level_2 = 16,
        //
        // Summary:
        //     indicates an third-order civil entity below a locality.
        Sublocality_Level_3 = 17,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality.
        Sublocality_Level_4 = 18,
        //
        // Summary:
        //     indicates an first-order civil entity below a locality.
        Sublocality_Level_5 = 19,
        //
        // Summary:
        //     Indicates a named neighborhood
        Neighborhood = 20,
        //
        // Summary:
        //     Indicates a named location, usually a building or collection of buildings with
        //     a common name.
        Premise = 21,
        //
        // Summary:
        //     Indicates a first-order entity below a named location, usually a singular building
        //     within a collection of buildings with a common name.
        Subpremise = 22,
        //
        // Summary:
        //     Indicates a postal code as used to address postal mail within the country.
        Postal_Code = 23,
        //
        // Summary:
        //     Indicates a postal code prefix.
        Postal_Code_Prefix = 24,
        //
        // Summary:
        //     Indicates a postal code suffix.
        Postal_Code_Suffix = 25,
        //
        // Summary:
        //     Indicates a prominent natural feature.
        Natural_Feature = 26,
        //
        // Summary:
        //     Indicates a named point of interest. Typically, these "POI"s are prominent local
        //     entities that don't easily fit in another category such as "Empire State Building"
        //     or "Statue of Liberty."
        Point_Of_Interest = 27,
        //
        // Summary:
        //     Indicates the floor of a building address.
        Floor = 28,
        //
        // Summary:
        //     post_box indicates a specific postal box.
        Post_Box = 29,
        //
        // Summary:
        //     postal_town indicates a grouping of geographic areas, such as locality and sublocality,
        //     used for mailing addresses in some countries.
        Postal_Town = 30,
        //
        // Summary:
        //     room indicates the room of a building address.
        Room = 31,
        //
        // Summary:
        //     street_number indicates the precise street number.
        Street_Number = 32,
        //
        // Summary:
        //     Indicate the location of a public transit stop.
        Transit_Station = 33,
        //
        // Summary:
        //     Accounting.
        Accounting = 34,
        //
        // Summary:
        //     Airport.
        Airport = 35,
        //
        // Summary:
        //     Amusement Park.
        Amusement_Park = 36,
        //
        // Summary:
        //     Aquarium.
        Aquarium = 37,
        //
        // Summary:
        //     Art Gallery.
        Art_Gallery = 38,
        //
        // Summary:
        //     Atm.
        Atm = 39,
        //
        // Summary:
        //     Bakery.
        Bakery = 40,
        //
        // Summary:
        //     Bank.
        Bank = 41,
        //
        // Summary:
        //     Bar.
        Bar = 42,
        //
        // Summary:
        //     Beauty Salon.
        Beauty_Salon = 43,
        //
        // Summary:
        //     Bicycle Store.
        Bicycle_Store = 44,
        //
        // Summary:
        //     Book Store.
        Book_Store = 45,
        //
        // Summary:
        //     Bowling Alley.
        Bowling_Alley = 46,
        //
        // Summary:
        //     Bus Station.
        Bus_Station = 47,
        //
        // Summary:
        //     Cafe.
        Cafe = 48,
        //
        // Summary:
        //     Campground.
        Campground = 49,
        //
        // Summary:
        //     Car Dealer.
        Car_Dealer = 50,
        //
        // Summary:
        //     Car Rental.
        Car_Rental = 51,
        //
        // Summary:
        //     Car Repair.
        Car_Repair = 52,
        //
        // Summary:
        //     Car Wash.
        Car_Wash = 53,
        //
        // Summary:
        //     Casino.
        Casino = 54,
        //
        // Summary:
        //     Cemetery.
        Cemetery = 55,
        //
        // Summary:
        //     Church.
        Church = 56,
        //
        // Summary:
        //     City Hall.
        City_Hall = 57,
        //
        // Summary:
        //     Clothing Store.
        Clothing_Store = 58,
        //
        // Summary:
        //     Convenience Store.
        Convenience_Store = 59,
        //
        // Summary:
        //     Courthouse.
        Courthouse = 60,
        //
        // Summary:
        //     Dentist.
        Dentist = 61,
        //
        // Summary:
        //     Department Store.
        Department_Store = 62,
        //
        // Summary:
        //     Doctor.
        Doctor = 63,
        //
        // Summary:
        //     Electrician.
        Electrician = 64,
        //
        // Summary:
        //     Electronics Store.
        Electronics_Store = 65,
        //
        // Summary:
        //     Embassy.
        Embassy = 66,
        //
        // Summary:
        //     Establishment.
        Establishment = 67,
        //
        // Summary:
        //     Finance.
        Finance = 68,
        //
        // Summary:
        //     Fire Station.
        Fire_Station = 69,
        //
        // Summary:
        //     Florist.
        Florist = 70,
        //
        // Summary:
        //     Food.
        Food = 71,
        //
        // Summary:
        //     Funeral Home.
        Funeral_Home = 72,
        //
        // Summary:
        //     Furniture Store.
        Furniture_Store = 73,
        //
        // Summary:
        //     Gas Station.
        Gas_Station = 74,
        //
        // Summary:
        //     General Contractor.
        General_Contractor = 75,
        //
        // Summary:
        //     Supermarket.
        Supermarket = 76,
        //
        // Summary:
        //     Grocery Or Supermarket.
        Grocery_Or_Supermarket = 77,
        //
        // Summary:
        //     Gym.
        Gym = 78,
        //
        // Summary:
        //     Hair Care.
        Hair_Care = 79,
        //
        // Summary:
        //     Hardware Store.
        Hardware_Store = 80,
        //
        // Summary:
        //     Health.
        Health = 81,
        //
        // Summary:
        //     Hindu Temple.
        Hindu_Temple = 82,
        //
        // Summary:
        //     Home Goods Store.
        Home_Goods_Store = 83,
        //
        // Summary:
        //     Hospital.
        Hospital = 84,
        //
        // Summary:
        //     Insurance Agency.
        Insurance_Agency = 85,
        //
        // Summary:
        //     Jewelry Store.
        Jewelry_Store = 86,
        //
        // Summary:
        //     Laundry.
        Laundry = 87,
        //
        // Summary:
        //     Lawyer.
        Lawyer = 88,
        //
        // Summary:
        //     Library.
        Library = 89,
        //
        // Summary:
        //     Liquor Store.
        Liquor_Store = 90,
        //
        // Summary:
        //     Local Government Office.
        Local_Government_Office = 91,
        //
        // Summary:
        //     Locksmith.
        Locksmith = 92,
        //
        // Summary:
        //     Lodging.
        Lodging = 93,
        //
        // Summary:
        //     Meal Delivery.
        Meal_Delivery = 94,
        Meal_Takeaway = 95,
        //
        // Summary:
        //     Mosque.
        Mosque = 96,
        //
        // Summary:
        //     Movie Rental.
        Movie_Rental = 97,
        //
        // Summary:
        //     Movie Theater.
        Movie_Theater = 98,
        //
        // Summary:
        //     Moving Company.
        Moving_Company = 99,
        //
        // Summary:
        //     Museum.
        Museum = 100,
        //
        // Summary:
        //     Night Club.
        Night_Club = 101,
        //
        // Summary:
        //     Painter.
        Painter = 102,
        //
        // Summary:
        //     Park.
        Park = 103,
        //
        // Summary:
        //     Parking.
        Parking = 104,
        //
        // Summary:
        //     Pet Store.
        Pet_Store = 105,
        //
        // Summary:
        //     Pharmacy.
        Pharmacy = 106,
        //
        // Summary:
        //     Physiotherapist.
        Physiotherapist = 107,
        //
        // Summary:
        //     Place Of Worship.
        Place_Of_Worship = 108,
        //
        // Summary:
        //     Plumber.
        Plumber = 109,
        //
        // Summary:
        //     Police.
        Police = 110,
        //
        // Summary:
        //     Post Office.
        PostOffice = 111,
        //
        // Summary:
        //     Real Estate Agency.
        Real_Estate_Agency = 112,
        //
        // Summary:
        //     Restaurant.
        Restaurant = 113,
        //
        // Summary:
        //     Roofing Contractor.
        Roofing_Contractor = 114,
        //
        // Summary:
        //     Rv Park.
        Rv_Park = 115,
        //
        // Summary:
        //     School.
        School = 116,
        Shoe_Store = 117,
        //
        // Summary:
        //     Shopping Mall.
        Shopping_Mall = 118,
        //
        // Summary:
        //     Spa.
        Spa = 119,
        //
        // Summary:
        //     Stadium.
        Stadium = 120,
        //
        // Summary:
        //     Storage.
        Storage = 121,
        //
        // Summary:
        //     Store-
        Store = 122,
        //
        // Summary:
        //     Subway Station.
        Subway_Station = 123,
        //
        // Summary:
        //     Synagogue.
        Synagogue = 124,
        //
        // Summary:
        //     Tourist Attracton.
        Tourist_Attracton = 125,
        //
        // Summary:
        //     Taxi Stand.
        Taxi_Stand = 126,
        //
        // Summary:
        //     Train Station.
        Train_Station = 127,
        //
        // Summary:
        //     Travel Agency.
        Travel_Agency = 128,
        //
        // Summary:
        //     University.
        University = 129,
        //
        // Summary:
        //     VeterinaryCare.
        Veterinary_Care = 130,
        //
        // Summary:
        //     Zoo.
        Zoo = 131
    }
}
