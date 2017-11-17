using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number
{
    public string No { get; set; }
    public bool Preferred { get; set; }
    public int Type { get; set; }
}

public class OfficeAddress
{
    public string AddressLine1 { get; set; }
    public object AddressLine2 { get; set; }
    public object AddressLine3 { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CountryDescription { get; set; }
    public object Location { get; set; }
    public string Municipality { get; set; }
    public string PostalCode { get; set; }
    public string StateDescription { get; set; }
}

public class DpnProfile
{
    public string About { get; set; }
    public string ActiveDirectoryId { get; set; }
    public object ActivityStream { get; set; }
    public List<string> AskAbout { get; set; }
    public object AssistantName { get; set; }
    public object AssistantPhone { get; set; }
    public object Birthday { get; set; }
    public bool CanBeAddedToMyColleagues { get; set; }
    public List<string> Capabilities { get; set; }
    public List<object> Certifications { get; set; }
    public string City { get; set; }
    public object Colleagues { get; set; }
    public string Country { get; set; }
    public bool Deleted { get; set; }
    public List<object> Designations { get; set; }
    public string Email { get; set; }
    public string Entity { get; set; }
    public object FavoriteActors { get; set; }
    public object FavoriteAuthors { get; set; }
    public object FavoriteBooks { get; set; }
    public object FavoriteMovies { get; set; }
    public object FavoriteMusic { get; set; }
    public object FavoritePlacesToEat { get; set; }
    public object FavoritePlacesToVisit { get; set; }
    public object FavoriteQuotes { get; set; }
    public object FavoriteSports { get; set; }
    public object FavoriteTelevisionShows { get; set; }
    public string FirstName { get; set; }
    public string Function { get; set; }
    public List<object> Hobbies { get; set; }
    public object HomeTown { get; set; }
    public string Id { get; set; }
    public List<object> Interests { get; set; }
    public List<string> Keywords { get; set; }
    public object LabyrinthGameResults { get; set; }
    public List<object> LanguagesSpoken { get; set; }
    public object LastLocation { get; set; }
    public string LastLocationDate { get; set; }
    public string LastName { get; set; }
    public string Level { get; set; }
    public object LocationAddress { get; set; }
    public object Locations { get; set; }
    public string MemberFirm { get; set; }
    public List<string> Names { get; set; }
    public object Notes { get; set; }
    public List<Number> Numbers { get; set; }
    public string Office { get; set; }
    public OfficeAddress OfficeAddress { get; set; }
    public object OtherName { get; set; }
    public object PersonLocation { get; set; }
    public string PictureBase64 { get; set; }
    public string PictureUrl { get; set; }
    public object PictureURL_AzureAD { get; set; }
    public object PictureURL_BasicNTLM { get; set; }
    public object PictureURL_OAuth { get; set; }
    public string PreferredFirstName { get; set; }
    public string PreferredPhone { get; set; }
    public string PrimaryIndustry { get; set; }
    public string PrimaryMarket { get; set; }
    public object Role { get; set; }
    public string SecondaryIndustry { get; set; }
    public string SecondaryMarket { get; set; }
    public List<object> Skills { get; set; }
    public string Title { get; set; }
    public string Username { get; set; }
    public object Vendor { get; set; }
    public object VendorModule { get; set; }
    public List<object> VolunteerActivities { get; set; }
}

public class User
{
    public string _id { get; set; }
    public string updatedAt { get; set; }
    public string createdAt { get; set; }
    public string email { get; set; }
    public int __v { get; set; }
    public List<object> scores { get; set; }
    public List<string> logins { get; set; }
}

public class EmployeeData
{
    public DpnProfile dpn_profile { get; set; }
    public User user { get; set; }
}
