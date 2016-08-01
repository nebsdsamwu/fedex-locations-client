// This code was built using Visual Studio 2005
using System;
using System.Web.Services.Protocols;
//using LocationsSearchWebServiceClient.LocationsServiceWebReference;
using GlobalShipAddressWebServiceClient.LocationsServiceWebReference;

// Sample code to call the FedEx Locations Search Web Service
// Tested with Microsoft Visual Studio 2005 Professional Edition
namespace LocationsSearchWebServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SearchLocationsRequest request = CreateSearchLocationsRequest();
            //
            LocationsService service = new LocationsService();
			if (usePropertyFile())
            {
                service.Url = getProperty("endpoint");
            }
            //
            try
            {
                // Call the Locations web service passing in a SearchLocationsRequest and returning a SearchLocationsReply
                SearchLocationsReply reply = service.searchLocations(request);
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING)
                {
                    ShowSearchLocationsReply(reply);
                }
                ShowNotifications(reply);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Detail.InnerText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press any key to quit!");
            Console.ReadKey();
        }

        private static SearchLocationsRequest CreateSearchLocationsRequest()
        {
            // Build the SearchLocationRequest
            SearchLocationsRequest request = new SearchLocationsRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = "Nvu0MsZ4wgWPTA84"; // Replace "XXX" with the Key
            request.WebAuthenticationDetail.UserCredential.Password = "jJvWn79S9lFeuzPlYblL76hnR"; // Replace "XXX" with the Password
            request.WebAuthenticationDetail.ParentCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.ParentCredential.Key = "Nvu0MsZ4wgWPTA84"; // Replace "XXX" with the Key
            request.WebAuthenticationDetail.ParentCredential.Password = "jJvWn79S9lFeuzPlYblL76hnR"; // Replace "XXX"
            if (usePropertyFile()) //Set values from a file for testing purposes
            {
                request.WebAuthenticationDetail.UserCredential.Key = getProperty("key");
                request.WebAuthenticationDetail.UserCredential.Password = getProperty("password");
                request.WebAuthenticationDetail.ParentCredential.Key = getProperty("parentkey");
                request.WebAuthenticationDetail.ParentCredential.Password = getProperty("parentpassword");
            }
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = "510087208"; // Replace "XXX" with the client's account number
            request.ClientDetail.MeterNumber = "100298364"; // Replace "XXX" with the client's meter number
            if (usePropertyFile()) //Set values from a file for testing purposes
            {
                request.ClientDetail.AccountNumber = getProperty("accountnumber");
                request.ClientDetail.MeterNumber = getProperty("meternumber");
            }
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***SearchLocation v2 Request using VC#***"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.
            //
            request.Version = new VersionId();
            //
            request.EffectiveDate = DateTime.Now;
            request.EffectiveDateSpecified = true;
            request.LocationsSearchCriterionSpecified = true;
            //
            bool bUsePhoneNumber = false; 
            if (bUsePhoneNumber)
            {
                request.LocationsSearchCriterion = LocationsSearchCriteriaType.PHONE_NUMBER;
                //request.PhoneNumber = "9015551234"; // Search locations based on a phone number
                request.PhoneNumber = "6262719700";
            }
            else
            {
                request.LocationsSearchCriterion = LocationsSearchCriteriaType.ADDRESS;
            }
            SetAddress(request);
            //
            request.MultipleMatchesAction = MultipleMatchesActionType.RETURN_ALL;
            request.MultipleMatchesActionSpecified = true;
            SetSortDetail(request);
            SetConstraints(request);
            return request;
        }

        private static void SetAddress(SearchLocationsRequest request)
        {
            request.Address = new Address();
            request.Address.StreetLines = new string[1] { "17560 Rowland St" };
            request.Address.City = "City of Industry";
            request.Address.StateOrProvinceCode = "CA";
            request.Address.PostalCode = "91748";
            request.Address.CountryCode = "US";
            //request.Address = new Address();
            //request.Address.StreetLines = new string[1] { "Address Line 1" };
            //request.Address.City = "Collierville";
            //request.Address.StateOrProvinceCode = "TN";
            //request.Address.PostalCode = "38017";
            //request.Address.CountryCode = "US";
        }

        private static void SetSortDetail(SearchLocationsRequest request)
        {
            request.SortDetail = new LocationSortDetail();
            request.SortDetail.Criterion = LocationSortCriteriaType.DISTANCE;
            request.SortDetail.CriterionSpecified = true;
            request.SortDetail.Order = LocationSortOrderType.LOWEST_TO_HIGHEST;
            request.SortDetail.OrderSpecified = true;
        }

        private static void SetConstraints(SearchLocationsRequest request)
        {
            request.Constraints = new SearchLocationConstraints();
            request.Constraints.RadiusDistance = new Distance();
            request.Constraints.RadiusDistance.Value = new Decimal(25.0);
            request.Constraints.RadiusDistance.ValueSpecified = true;
            request.Constraints.RadiusDistance.Units = DistanceUnits.MI;
            request.Constraints.RadiusDistance.UnitsSpecified = true;
            //
            request.Constraints.ResultsFilters = new LocationSearchFilterType[1];
            request.Constraints.ResultsFilters[0] = LocationSearchFilterType.EXCLUDE_LOCATIONS_OUTSIDE_STATE_OR_PROVINCE;
            //
            // These is [AND] logic, include GROUND will reduce the results a lot.  
            request.Constraints.RequiredLocationAttributes = new LocationAttributesType[3];
            request.Constraints.RequiredLocationAttributes[0] = LocationAttributesType.SATURDAY_EXPRESS_HOLD_AT_LOCATION;
            request.Constraints.RequiredLocationAttributes[1] = LocationAttributesType.WEEKDAY_EXPRESS_HOLD_AT_LOCATION;
            //request.Constraints.RequiredLocationAttributes[2] = LocationAttributesType.WEEKDAY_GROUND_HOLD_AT_LOCATION;
            //
            request.Constraints.ResultsRequested = "20";
            //
            request.Constraints.LocationContentOptions = new LocationContentOptionType[1];
            //request.Constraints.LocationContentOptions[0] = LocationContentOptionType.HOLIDAYS;
            //
            // This is [OR] logic.
            request.Constraints.LocationTypesToInclude = new FedExLocationType[3];
            request.Constraints.LocationTypesToInclude[0] = FedExLocationType.FEDEX_AUTHORIZED_SHIP_CENTER;
            request.Constraints.LocationTypesToInclude[1] = FedExLocationType.FEDEX_EXPRESS_STATION;
            request.Constraints.LocationTypesToInclude[2] = FedExLocationType.FEDEX_OFFICE; // This includes most of the first two types.

        }

        private static void ShowSearchLocationsReply(SearchLocationsReply reply)
        {
            Console.WriteLine("Total Locations Available: {0}", reply.TotalResultsAvailable);
            Console.WriteLine("Locations Returned: {0}", reply.ResultsReturned);
            Console.WriteLine();
            if (reply.AddressToLocationRelationships != null)
            {
                foreach (AddressToLocationRelationshipDetail location in reply.AddressToLocationRelationships)
                {
                    if (location.MatchedAddress != null)
                    {
                        Console.WriteLine("Address information used for search");
                        if (location.MatchedAddress.StreetLines != null)
                        {
                            foreach (String streetline in location.MatchedAddress.StreetLines)
                            {
                                Console.WriteLine("  Streetline: {0}", streetline);
                            }
                        }
                        if (location.MatchedAddress.City != null) Console.WriteLine("  City: {0}", location.MatchedAddress.City);
                        if (location.MatchedAddress.StateOrProvinceCode != null)
                        {
                            Console.WriteLine("  State or Province Code: {0}", location.MatchedAddress.StateOrProvinceCode);
                        }
                        if (location.MatchedAddress.PostalCode != null)
                        {
                            Console.WriteLine("  Postal Code: {0}", location.MatchedAddress.PostalCode);
                        }
                        if (location.MatchedAddress.CountryCode != null) Console.WriteLine("  Country Code: {0}", location.MatchedAddress.CountryCode);
                    }
                    Console.WriteLine();
                    ShowLocation(location);
                }
            }
        }

        private static void ShowLocation(AddressToLocationRelationshipDetail relationshipDetail)
        {
            if (relationshipDetail == null) return;
            if (relationshipDetail.DistanceAndLocationDetails != null)
            {
                int locationNumber = 1;
                foreach (DistanceAndLocationDetail distanceAndLocationDetail in relationshipDetail.DistanceAndLocationDetails)
                {
                    Console.WriteLine("Location {0} Information", locationNumber);
                    locationNumber += 1;
                    ShowLocationDetail(distanceAndLocationDetail);
                    Console.WriteLine();
                }
            }
        }

        private static void ShowLocationDetail(DistanceAndLocationDetail distanceAndLocationDetail)
        {
            if (distanceAndLocationDetail.Distance != null)
            {
                Console.WriteLine("Distance: {0} {1}", distanceAndLocationDetail.Distance.Value, distanceAndLocationDetail.Distance.Units);
            }
            if (distanceAndLocationDetail.LocationDetail != null)
            {
                LocationDetail locationDetail = distanceAndLocationDetail.LocationDetail;
                ShowLocationContactAddress(locationDetail.LocationContactAndAddress);
                Console.WriteLine("Geographic Coordinates: {0}", locationDetail.GeographicCoordinates);
                if (locationDetail.LocationTypeSpecified) Console.WriteLine("Location Type: {0}", locationDetail.LocationType);
                if (locationDetail.Attributes != null)
                {
                    Console.WriteLine("Location Attributes");
                    foreach (LocationAttributesType attribute in locationDetail.Attributes)
                    {
                        Console.WriteLine("  {0}", attribute);
                    }
                }
                //ShowHours(locationDetail.NormalHours, "Normal Hours");
                //ShowHours(locationDetail.HoursForEffectiveDate, "Hours for effective date");
                //ShowCarrierDetails(locationDetail.CarrierDetails);
            }
        }

        private static void ShowLocationContactAddress(LocationContactAndAddress locationContactAndAddress)
        {
            if (locationContactAndAddress == null) return;
            if (locationContactAndAddress.Contact != null)
            {
                Console.WriteLine("  Contact Information");
                Contact contact = locationContactAndAddress.Contact;
                if (contact.ContactId != null) Console.WriteLine("    Id: {0}", contact.ContactId);
                if (contact.CompanyName != null) Console.WriteLine("    Company: {0}", contact.CompanyName);
                if (contact.PersonName != null) Console.WriteLine("    Contact: {0}", contact.PersonName);
                if (contact.EMailAddress != null) Console.WriteLine("    Email: {0}", contact.EMailAddress);
            }
            if (locationContactAndAddress.Address != null)
            {
                Console.WriteLine("  Address Information");
                Address address = locationContactAndAddress.Address;
                if (address.StreetLines != null)
                {
                    foreach (String streetline in address.StreetLines)
                    {
                        Console.WriteLine("    Streetline: {0}", streetline);
                    }
                }
                if (address.City != null) Console.WriteLine("    City: {0}", address.City);
                if (address.StateOrProvinceCode != null) Console.WriteLine("    State or Province code: {0}", address.StateOrProvinceCode);
                if (address.PostalCode != null) Console.WriteLine("    Postal code: {0}", address.PostalCode);
                if (address.CountryCode != null) Console.WriteLine("    Country code: {0}", address.CountryCode);
                if (address.UrbanizationCode != null) Console.WriteLine("    Urbanization code: {0}", address.UrbanizationCode);
            }
        }

        private static void ShowHours(LocationHours[] locHours, String hoursDescription)
        {
            if (locHours == null) return;
            Console.WriteLine(hoursDescription);
            foreach (LocationHours locationHours in locHours)
            {
                if (locationHours.DayofWeekSpecified) Console.WriteLine("  Day of Week: {0}", locationHours.DayofWeek);
                if (locationHours.OperationalHoursSpecified) Console.WriteLine("  Operational Hours: {0}", locationHours.OperationalHours);
                if (locationHours.Hours != null)
                {
                    foreach (TimeRange timeRange in locationHours.Hours)
                    {
                        if (timeRange.BeginsSpecified) Console.WriteLine("    Begins: {0}", timeRange.Begins.TimeOfDay);
                        if (timeRange.EndsSpecified) Console.WriteLine("    Ends: {0}", timeRange.Ends.TimeOfDay);
                    }
                }
            }
        }

        private static void ShowCarrierDetails(CarrierDetail[] carrierDetails)
        {
            if (carrierDetails == null) return;
            foreach (CarrierDetail carrierDetail in carrierDetails)
            {
                Console.WriteLine("Carrier Details");
                if (carrierDetail.CarrierSpecified) Console.WriteLine("  Carrier: {0}", carrierDetail.Carrier);
                if (carrierDetail.NormalLatestDropOffDetails != null)
                {
                    foreach (LatestDropOffDetail dropOffDetail in carrierDetail.NormalLatestDropOffDetails)
                    {
                        ShowTimes("  Normal Dropoff Times", dropOffDetail.DayOfWeekSpecified, dropOffDetail.DayOfWeek, dropOffDetail.TimeSpecified, dropOffDetail.Time);
                    }
                }
                if (carrierDetail.EffectiveLatestDropOffDetails != null)
                {
                    ShowTimes("  Dropoff Times for date requested", carrierDetail.EffectiveLatestDropOffDetails.DayOfWeekSpecified, carrierDetail.EffectiveLatestDropOffDetails.DayOfWeek, carrierDetail.EffectiveLatestDropOffDetails.TimeSpecified, carrierDetail.EffectiveLatestDropOffDetails.Time);
                }
                if (carrierDetail.ExceptionalLatestDropOffDetails != null)
                {
                    foreach (LatestDropOffDetail dropOffDetail in carrierDetail.ExceptionalLatestDropOffDetails)
                    {
                        ShowTimes("  Exceptional Dropoff Times", dropOffDetail.DayOfWeekSpecified, dropOffDetail.DayOfWeek, dropOffDetail.TimeSpecified, dropOffDetail.Time);
                    }
                }
            }
        }

        private static void ShowTimes(String timeDescription, bool dayOfWeekSpecified, DayOfWeekType dayOfWeek,bool timeSpecified, DateTime time)
        {
            Console.WriteLine(timeDescription);
            if(dayOfWeekSpecified )
                Console.WriteLine("    Day of Week: {0}", dayOfWeek);
            if (timeSpecified)
                Console.WriteLine("    Time: {0}", time.TimeOfDay);
        }

        private static void ShowNotifications(SearchLocationsReply reply)
        {
            Console.WriteLine("Notifications");
            for (int i = 0; i < reply.Notifications.Length; i++)
            {
                Notification notification = reply.Notifications[i];
                Console.WriteLine("Notification no. {0}", i);
                Console.WriteLine(" Severity: {0}", notification.Severity);
                Console.WriteLine(" Code: {0}", notification.Code);
                Console.WriteLine(" Message: {0}", notification.Message);
                Console.WriteLine(" Source: {0}", notification.Source);
            }
        }

        private static bool usePropertyFile() //Set to true for common properties to be set with getProperty function.
        {
            return false; //getProperty("usefile").Equals("True");
        }

        private static String getProperty(String propertyname) //Sets common properties for testing purposes.
        {
            try
            {
                String filename = "C:\\filepath\\filename.txt";       
                if (System.IO.File.Exists(filename))
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(filename);
                    do
                    {
                        String[] parts = sr.ReadLine().Split(',');
                        if (parts[0].Equals(propertyname) && parts.Length == 2)
                        {
                            return parts[1];
                        }
                    }
                    while (!sr.EndOfStream);
                }
                Console.WriteLine("Property {0} set to default 'XXX'", propertyname);
                return "XXX";
            }
            catch (Exception e)
            {
                Console.WriteLine("Property {0} set to default 'XXX'", propertyname);
                return "XXX";
            }
        }
    }
}