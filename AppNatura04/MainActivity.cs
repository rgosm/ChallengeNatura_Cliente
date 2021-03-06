﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Webkit;
using Xamarin.Essentials;
using System;

namespace AppNaturaCliente {
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity {
        private Button btnComprar;
        private Button btnCarrinho;
        private Button btnAjuda;
        private bool chatOn = false;
        
        private WebView webViewInicial;

        public bool ChatOn { get => chatOn; set => chatOn = value; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Accelerometer.ShakeDetected += ShakeDetected;

            btnComprar = FindViewById<Button>(Resource.Id.btnComprar);
            btnCarrinho = FindViewById<Button>(Resource.Id.btnCarrinho);
            webViewInicial = FindViewById<WebView>(Resource.Id.webViewInicial);

            btnComprar.Click += BtnComprar_Click;
            btnCarrinho.Click += BtnCarrinho_Click;

            btnAjuda = FindViewById<Button>(Resource.Id.btnAjuda);
            btnAjuda.Click += BtnAjuda_Click;

            webViewInicial.SetWebViewClient(new ExtendWebViewClient());
            webViewInicial.LoadUrl("https://www.natura.com.br/c/tudo-em-promocoes");

            webViewInicial.Settings.DomStorageEnabled = true;

            WebSettings webSettings = webViewInicial.Settings;
            webSettings.JavaScriptEnabled = true;

            ToggleAccelerometer();

        }

        internal class ExtendWebViewClient : WebViewClient {
            [System.Obsolete]
            public override bool ShouldOverrideUrlLoading(WebView view, string url) {
                view.LoadUrl(url);
                return true;
            }
        }

        private void BtnCarrinho_Click(object sender, System.EventArgs e) {
            StartActivity(typeof(ListaCarrinho));
        }

        private void BtnAjuda_Click(object sender, System.EventArgs e) {
            if (!ChatOn)
            {
                ChatOn = true;
                StartActivity(typeof(Chat));
                new MainActivity().ToggleAccelerometer();
            }
            
        }

        private void BtnComprar_Click(object sender, System.EventArgs e) {
            StartActivity(typeof(ColetaCodigo));
        }


        public void ShakeDetected(object sender, EventArgs e)
        {
           MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!ChatOn)
                {
                    StartActivity(typeof(Chat));
                    ToggleAccelerometer();
                }
                    
            });

        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(SensorSpeed.Game);
            }
            catch (FeatureNotSupportedException)
            {
                Toast.MakeText(Application.Context, "Recurso não suportado pelo aparelho", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "Other error has occurred" + ex.Message, ToastLength.Short).Show();
            }
        }
    }
}