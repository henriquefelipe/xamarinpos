﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinPOS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }      

        protected override void OnStart()
        {
            // Handle when your app starts
            //CrossCurrentActivity.Current.Init(this);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
