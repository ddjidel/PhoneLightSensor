using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Services;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using System.Device.Location;
using Phone_Light_Sensor.Resources;
using Phone_Light_Sensor.Helpers;

namespace Phone_Light_Sensor
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private LightSensor lightSensor = LightSensor.GetDefault();
        private double latitude = 0d;
        private double longitude = 0d;
        private string street = string.Empty;
        private string zipcode = string.Empty;
        private string city = string.Empty;
        private string country = string.Empty;

        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private void btnFindCoordinate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GetAddress();
            var CurrentlightSensorReading = lightSensor.GetCurrentReading();
            tbkLux.Text = CurrentlightSensorReading.IlluminanceInLux.ToString();

            if (CurrentlightSensorReading.IlluminanceInLux > 1000)
            {
                beep.Play();
                CRMGateway.CreateAutoCase(latitude, longitude, street, zipcode, city, country);
            }
        }
        public async void GetAddress()
        {;
            var locator = new Geolocator();
            var reverseGeocodeQuery = new ReverseGeocodeQuery();

            if (!locator.LocationStatus.Equals(PositionStatus.Disabled))
            {
                var position = await locator.GetGeopositionAsync();
                latitude = position.Coordinate.Point.Position.Latitude;
                longitude = position.Coordinate.Point.Position.Longitude;
                tbkLatitude.Text = latitude.ToString();
                tbkLongitude.Text = longitude.ToString();

                reverseGeocodeQuery.GeoCoordinate = new GeoCoordinate(latitude, longitude);
                reverseGeocodeQuery.QueryAsync();

                reverseGeocodeQuery.QueryCompleted += (sender, args) =>
                {
                    if (!args.Result.Equals(null))
                    {
                        var result = args.Result.FirstOrDefault();

                        tbkCity.Text = result.Information.Address.City;
                        tbkCountry.Text = result.Information.Address.Country;
                        tbkCountryCode.Text = result.Information.Address.CountryCode;
                        tbkHouseNumber.Text = result.Information.Address.HouseNumber;
                        tbkPostalCode.Text = result.Information.Address.PostalCode;
                        tbkState.Text = result.Information.Address.State;
                        tbkStreet.Text = result.Information.Address.Street;

                        street = result.Information.Address.HouseNumber + " " + result.Information.Address.Street;
                        zipcode = result.Information.Address.PostalCode;
                        city = result.Information.Address.City;
                        country = result.Information.Address.Country;
                    }
                };
            }
            else
            {
                MessageBox.Show("Geolocation not enabled!", AppResources.ApplicationTitle, MessageBoxButton.OK);
                return;
            }
        }
    }
}