﻿using P04WeatherForecastAPI.Client.Commands;
using P04WeatherForecastAPI.Client.DataSeeders;
using P04WeatherForecastAPI.Client.Models;
using P04WeatherForecastAPI.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P04WeatherForecastAPI.Client.ViewModels
{
    public class MainViewModelV2 : BaseViewModel
    {
    
        private CityViewModel _selectedCity;
        private Weather _weather;
        private readonly IAccuWeatherService _accuWeatherService;

        public ICommand LoadCitiesCommand { get;  }

        public MainViewModelV2(IAccuWeatherService accuWeatherService)
        {
            LoadCitiesCommand = new RelayCommand(x => LoadCities(x as string));
            _accuWeatherService = accuWeatherService;
            Cities = new ObservableCollection<CityViewModel>(); // podejście nr 2 

            //_weather = MainViewDataseeder.GenerateWeather;
            //_selectedCity = MainViewDataseeder.GenerateSelectedCity;
            //_cities = MainViewDataseeder.GenerateCities;
        }


        private Weather Weather
        {
            get { return _weather; }
            set { 
                _weather = value;
                //OnPropertyChanged("Weather");
                OnPropertyChanged();
                OnPropertyChanged("CurrentTemperature");
            }
        }

        public string CurrentTemperature => Weather?.Temperature.Metric.Value.ToString();

        public CityViewModel SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged();
                LoadWeather();
            }
        }

        private async void LoadWeather()
        {
            if(SelectedCity != null)
            {
                Weather= await _accuWeatherService.GetCurrentConditions(SelectedCity.Key);
            }
        }


 

        // podajście nr 2 do przechowywania kolekcji obiektów:
        public ObservableCollection<CityViewModel> Cities { get; set; }


        public async void LoadCities(string locationName)
        {
            // podejście nr 1
            //Cities= await _accuWeatherService.GetLocations(locationName);

            // podejście nr 2:
            var cities = await _accuWeatherService.GetLocations(locationName);
            Cities.Clear();
            foreach (var city in cities) 
                Cities.Add(new CityViewModel(city));
        }
    }
}
