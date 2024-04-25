﻿using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASemiFinalACT2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
        static SQLiteHelper db;
        public static SQLiteHelper SQLiteDb
        {
            get
            {
                if (db == null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SemiFinalAct2.db3"));
                }
                return db;
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
