using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Xamarin.Essentials;

namespace AppNaturaCliente
{
    public class DetectShake : AppCompatActivity
    {
        SensorSpeed speed = SensorSpeed.Game;
        public DetectShake()
        {
            Accelerometer.ShakeDetected += ShakeDetected;
        }

        public void ShakeDetected (object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StartActivity(typeof(chat));
            });
            
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch(FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}