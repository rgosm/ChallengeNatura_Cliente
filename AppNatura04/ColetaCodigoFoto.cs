using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppNaturaCliente
{
    [Activity(Label = "Codigo em fotos")]
    public class ColetaCodigoFoto : AppCompatActivity
    {
        private ImageView imageView;
        private Button btnProcess;
        private Button btnAbrir;
        private TextView txtResult;
        private Bitmap bitmap;

        readonly string[] permissionGroup =
       {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.coletacodigofoto);

            imageView = FindViewById<ImageView>(Resource.Id.fotoView);

        }
    }
}