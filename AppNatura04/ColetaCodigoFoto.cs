﻿using Android;
using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        private String[] codigoProduto;
        private List<string> listaString;

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
            btnProcess = FindViewById<Button>(Resource.Id.btn_ler);
            btnAbrir = FindViewById<Button>(Resource.Id.btn_abrir);
            txtResult = FindViewById<TextView>(Resource.Id.txt_result);

            RequestPermissions(permissionGroup, 0);

            btnAbrir.Click += delegate
            {
                try
                {
                    UploadFoto();
                }
                catch (NullReferenceException e)
                {
                    Toast.MakeText(this, "Escolha uma imagem", ToastLength.Long).Show();
                }
            };

            btnProcess.Click += delegate
            {
                TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
                ColetaCodigo codigo1 = new ColetaCodigo();
                Regex regexBusca = new Regex(@"(\([\d]{5}\))+");
                Regex regexRetorno = new Regex(@"([\d]{5})+");
                listaString = new List<string>();

                if (textRecognizer.IsOperational)
                {
                    Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
                    SparseArray itens = textRecognizer.Detect(frame);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < itens.Size(); ++i)
                    {
                        TextBlock item = (TextBlock)itens.ValueAt(i);
                        MatchCollection matches = regexBusca.Matches(item.Value);

                        foreach(Match match in matches)
                        {
                            Match product = regexRetorno.Match(match.Value);
                            if(codigo1.BuscaDescricao(product.ToString()) != "Produto não encontrado")
                            {
                               listaString.Add(product.ToString() + " - " + codigo1.BuscaDescricao(product.ToString()));
                            }
                        }
                    }

                    txtResult.Text = String.Join(",", listaString);
                    codigoProduto = listaString.ToArray();

                    if(codigoProduto.Count() > 0)
                    {
                        txtResult.Text = String.Join(",", listaString);
                        IniciaLista();
                        Finish();
                    }
                    else
                    {
                        txtResult.Text = "Nenhum produto encontrado";
                    }
                    
                }
                else
                {
                    Toast.MakeText(this, "Operação não suportada pelo aparelho", ToastLength.Short).Show();
                    return;
                }
            };
        }

        private async void UploadFoto()
        {
            await CrossMedia.Current.Initialize();
            listaString = null;

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Operação não suportada pelo aparelho", ToastLength.Short).Show();
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions { 
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40
            });
            if(file != null)
            {
                byte[] imagemArray = System.IO.File.ReadAllBytes(file.Path);
                bitmap = BitmapFactory.DecodeByteArray(imagemArray, 0, imagemArray.Length);
                imageView.SetImageBitmap(bitmap);
                btnProcess.Visibility = ViewStates.Visible;
            }
            else
            {
                Toast.MakeText(this, "Escolha uma foto", ToastLength.Short).Show();
                imageView.SetImageDrawable(null);
                btnProcess.Visibility = ViewStates.Invisible;
            }
        }

        private void IniciaLista()
        {
            var listaProdutos = new Intent(this, typeof(ListaProdutos));
            listaProdutos.PutExtra("codigosProds", codigoProduto);
            StartActivity(listaProdutos);
            Finish();
        }

        public string[] GetCodigos()
        {
            return codigoProduto;
        }

        public void Teste()
        {
            foreach(string prod in codigoProduto)
            Console.WriteLine(prod);
        }

    }
  
}