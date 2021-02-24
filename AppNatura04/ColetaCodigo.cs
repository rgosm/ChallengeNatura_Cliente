using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.Support.V4.App;
using Android;
using Android.Util;
using Android.Content.PM;
using static Android.Gms.Vision.Detector;
using Java.Lang;
using Java.IO;
using System.Text.RegularExpressions;
using Android.Support.V7.Widget;
using MySql.Data.MySqlClient;
using System.IO;
using System.Linq;
using System;
using System.Data;
using Android.Support.Design.Widget;
using Android.Content;
using System.Globalization;

namespace AppNaturaCliente {
    [Activity(Label = "Direcione para o código do produto")]
    public class ColetaCodigo : AppCompatActivity, ISurfaceHolderCallback, IProcessor {
        private SurfaceView cameraView;
        private TextView textView;
        private CameraSource cameraSource;
        private TextView campo;
        private FloatingActionButton btnUploadFoto;
        private const int RequestCameraPermissionID = 1001;
        bool varBool = false;
        public static string codigo = null;
        public static string descricao = null;
        public static string preco = null;
        public static string quantidade = null;
        public static Bitmap imagemProduto = null;
        public static long numeroRegistro;
        private string descricaoProduto;
        byte[] imagem;

        public  void OnRequestPermissionsResult(int requestCode, string[] permissions, string[] grantResults) {
            switch (requestCode) {
                case RequestCameraPermissionID: {
                        cameraSource.Start(cameraView.Holder);
                    }
                break;
            }  
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.ColetaCodigoView);

            btnUploadFoto = FindViewById<FloatingActionButton>(Resource.Id.btn_upload);
            btnUploadFoto.Click += BtnUpload_Click;

            cameraView = FindViewById<SurfaceView>(Resource.Id.surface_view);
            textView = FindViewById<TextView>(Resource.Id.text_view);

            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("Coleta Codigo", "Detector dependencies are not yet available");
            else {
                cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer)
                    .SetFacing(CameraFacing.Back)
                    .SetRequestedPreviewSize(1280, 1024)
                    .SetRequestedFps(2.0f)
                    .SetAutoFocusEnabled(true)
                    .Build();

                cameraView.Holder.AddCallback(this);
                textRecognizer.SetProcessor(this);
            }
        }
        
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) {
        }

        public void SurfaceCreated(ISurfaceHolder holder) {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted) {
                ActivityCompat.RequestPermissions(this, new string[] {
                    Android.Manifest.Permission.Camera
                }, RequestCameraPermissionID);
                return;
            }
            //Toast.MakeText(this, "Teste1", ToastLength.Short).Show();
            //cameraSource.Start(cameraView.Holder); 
            // TODO: DESCOMENTAR A LINHA ACIMA ANTES DE CONCLUIR 
            
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {
            cameraSource.Stop();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ColetaCodigoFoto));
        }

        public void ReceiveDetections(Detections detections) {
            SparseArray items = detections.DetectedItems;
            if (items.Size() != 0) {
                textView.Post(() => {
                    StringBuilder strBuilder = new StringBuilder();
                    for (int i = 0; i < items.Size(); ++i) {
                        strBuilder.Append(((TextBlock)items.ValueAt(i)).Value);
                        strBuilder.Append("\n");
                    }

                    string source = strBuilder.ToString();

                    Regex regex = new Regex("\\((?<output>[a-zA-Z0-9]+)/*\\).*");
                    GroupCollection capturas = regex.Match(source).Groups;

                    codigo = capturas["output"].ToString();

                    BuscaBanco(codigo);
                });
            }
        }

     public void BuscaBanco(string codigo)
        {
            if (codigo != "" && !varBool)
            {
                MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
                MySqlCommand verificaProduto = new MySqlCommand(ComandosSQL.verificaProduto, conexao);
                MySqlCommand contaProdutoCarrinho = new MySqlCommand(ComandosSQL.contaProdutoCarrinho, conexao);
                MySqlCommand selecionaProdutoCarrinho = new MySqlCommand(ComandosSQL.selecionaProdutoCarrinho, conexao);

                MySqlDataReader retorno;
                MySqlDataReader produtoCarrinho;

                verificaProduto.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = codigo.ToString();
                contaProdutoCarrinho.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = codigo.ToString();
                contaProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";
                selecionaProdutoCarrinho.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = codigo.ToString();
                selecionaProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";

                conexao.Open();
                contaProdutoCarrinho.CommandType = CommandType.Text;

                numeroRegistro = (long)contaProdutoCarrinho.ExecuteScalar();
                contaProdutoCarrinho.Dispose();

                if (numeroRegistro > 0)
                {
                    conexao.Close();
                    Toast.MakeText(Application.Context, "Este produto já está em seu carrinho.", ToastLength.Long).Show();

                    conexao.Open();
                    produtoCarrinho = selecionaProdutoCarrinho.ExecuteReader();

                    if (produtoCarrinho.Read())
                    {
                        imagem = (byte[])(produtoCarrinho["imagem"]);
                        imagemProduto = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
                        descricao = produtoCarrinho.GetString("descricao").ToString();
                        preco = produtoCarrinho.GetString("preco").ToString();
                        quantidade = produtoCarrinho.GetString("quantidade").ToString();
                        IniciaExibeProduto();
                        //StartActivity(typeof(ExibeProduto));
                        Finish();
                    }

                }
                else
                {
                    conexao.Close();

                    conexao.Open();
                    retorno = verificaProduto.ExecuteReader();

                    if (retorno.Read())
                    {
                        imagem = (byte[])(retorno["imagem"]);
                        imagemProduto = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
                        descricao = retorno.GetString("descricao").ToString();
                        preco = retorno.GetString("preco").ToString();
                        quantidade = "1";
                        IniciaExibeProduto();
                        //StartActivity(typeof(ExibeProduto));
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "O Produto não consta no banco de dados!", ToastLength.Long).Show();
                        Finish();
                    }
                }

                conexao.Close();
                varBool = true;
            }
        }

        public string BuscaDescricao(string codigo)
        {

            if (codigo != "")
            {
                MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
                MySqlCommand verificaProduto = new MySqlCommand(ComandosSQL.verificaProduto, conexao);

                MySqlDataReader descricao;

                verificaProduto.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = codigo.ToString();

                conexao.Open();
                descricao = verificaProduto.ExecuteReader();

                if (descricao.Read())
                {
                    descricaoProduto = descricao.GetString("descricao").ToString();
                    conexao.Close();
                    //return descricaoProduto;
                }
                else
                {
                    descricaoProduto = "Produto não encontrado";
                    //return descricaoProduto;
                }
            }
            return descricaoProduto;
        }

        

        public void IniciaExibeProduto()
        {

            System.Console.WriteLine(descricao.ToString());
            System.Console.WriteLine(codigo.ToString()); // TODO: VERIFICAR ERRO DE NULL POINTER NESSA LINHA
            System.Console.WriteLine(preco);
            System.Console.WriteLine(quantidade.ToString());
            System.Console.WriteLine(numeroRegistro);

            var exibirProdutos = new Intent(this, typeof(ExibeProduto));
            exibirProdutos.PutExtra("txtDescricao", descricao.ToString());
            exibirProdutos.PutExtra("txtCodigo", codigo.ToString());
            exibirProdutos.PutExtra("txtPrecoUnitario", "Valor unitário: " + (Convert.ToDouble(preco)).ToString("C", CultureInfo.CurrentCulture));
            exibirProdutos.PutExtra("txtQuantidade", quantidade.ToString());
            exibirProdutos.PutExtra("txtPrecoTotal", (Convert.ToInt32(quantidade.ToString()) * Convert.ToDouble(preco)).ToString("C", CultureInfo.CurrentCulture));
            exibirProdutos.PutExtra("imgProduto", imagem);
            exibirProdutos.PutExtra("preco", preco);
            exibirProdutos.PutExtra("numRegistro", numeroRegistro);
            StartActivity(exibirProdutos);

            /*
             txtDescricao.Text =  ColetaCodigo.descricao.ToString();
            txtCodigo.Text = "Cód. " + ColetaCodigo.codigo.ToString();
            txtPrecoUnitario.Text = "Valor unitário: " + (Convert.ToDouble(ColetaCodigo.preco)).ToString("C", CultureInfo.CurrentCulture);
            txtQuantidade.Text = ColetaCodigo.quantidade.ToString();
            txtPrecoTotal.Text = (Convert.ToInt32(ColetaCodigo.quantidade.ToString()) * Convert.ToDouble(ColetaCodigo.preco)).ToString("C", CultureInfo.CurrentCulture);
             */
        }

        public void Release() {
        }
    }
}