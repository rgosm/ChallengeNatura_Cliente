using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Globalization;

namespace AppNaturaCliente
{
    [Activity(Label = "Exibe Produto")]
    public class ExibeProduto : Activity {

        private long registro;

        private Button btnIncluir;
        private Button btnCancelar;
        private Button btnAcrescentaUnidade;
        private Button btnTiraUnidade;

        private TextView txtDescricao;
        private TextView txtCodigo;
        private TextView txtQuantidade;
        private TextView txtPrecoUnitario;
        private TextView txtPrecoTotal;

        private Byte[] imagem;
        public static Bitmap imagemProduto;
        private ImageView imgProduto;

        private string textoDescricao;
        private string textoCodigo;
        private string textoPrecoUnitario;
        private string textoQuantidade;
        private string textoPrecoTotal;

        public IntPtr Context { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.exibeProduto);
            
            imgProduto = FindViewById<ImageView>(Resource.Id.imgProduto);

            txtDescricao = FindViewById<TextView>(Resource.Id.txtDescricao);
            txtCodigo = FindViewById<TextView>(Resource.Id.txtCodigo);
            txtQuantidade = FindViewById<TextView>(Resource.Id.txtQuantidade);
            txtPrecoUnitario = FindViewById<TextView>(Resource.Id.txtPrecoUnitario);
            txtPrecoTotal = FindViewById<TextView>(Resource.Id.txtPrecoTotal);

            btnIncluir = FindViewById<Button>(Resource.Id.btnIncluir);
            btnCancelar = FindViewById<Button>(Resource.Id.btnCancelar);
            btnAcrescentaUnidade = FindViewById<Button>(Resource.Id.btnAcrescentaUnidade);
            btnTiraUnidade = FindViewById<Button>(Resource.Id.btnTiraUnidade);
            
            btnIncluir.Click += BtnIncluir_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnAcrescentaUnidade.Click += BtnAcrescentaUnidade_Click;
            btnTiraUnidade.Click += BtnTiraUnidade_Click;

            textoDescricao = Intent.GetStringExtra("txtDescricao");
            textoCodigo = "Cód. " + Intent.GetStringExtra("txtCodigo");
            textoPrecoUnitario = "Valor unitário: " + Intent.GetStringExtra("txtPrecoUnitario");
            textoQuantidade = Intent.GetStringExtra("txtQuantidade");
            textoPrecoTotal = Intent.GetStringExtra("txtPrecoTotal");

            txtDescricao.Text = textoDescricao;
            txtCodigo.Text = textoCodigo;
            txtPrecoUnitario.Text = textoPrecoUnitario;
            txtQuantidade.Text = textoQuantidade;
            txtPrecoTotal.Text = textoPrecoTotal;

            imagem = Intent.GetByteArrayExtra("imgProduto");
            imagemProduto = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
            imgProduto.SetImageBitmap(imagemProduto);
        }
        
        private void BtnAcrescentaUnidade_Click(object sender, EventArgs e) {
            txtQuantidade.Text = (Convert.ToInt32(txtQuantidade.Text) + 1).ToString();
            txtPrecoTotal.Text = (Convert.ToInt32(txtQuantidade.Text) * (Convert.ToDouble(Intent.GetStringExtra("preco")))).ToString("C", CultureInfo.CurrentCulture);
        }

        private void BtnTiraUnidade_Click(object sender, EventArgs e) {
            txtQuantidade.Text = (Convert.ToInt32(txtQuantidade.Text) - 1).ToString();
            txtPrecoTotal.Text = (Convert.ToInt32(txtQuantidade.Text) * (Convert.ToDouble(Intent.GetStringExtra("preco")))).ToString("C", CultureInfo.CurrentCulture);

            if (Convert.ToInt32(txtQuantidade.Text) < 1){
                MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
                MySqlCommand excluiProdutoCarrinho = new MySqlCommand(ComandosSQL.excluiProdutoCarrinho, conexao);

                excluiProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";
                excluiProdutoCarrinho.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = Intent.GetStringExtra("txtCodigo");

                conexao.Open();
                excluiProdutoCarrinho.ExecuteNonQuery();
                conexao.Close();
                Finish();
                Toast.MakeText(Application.Context, "O produto foi excluido.", ToastLength.Long).Show();
            }
        }
        
        private void BtnCancelar_Click(object sender, EventArgs e) {
            Finish();
        }

        private void BtnIncluir_Click(object sender, System.EventArgs e) {
            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand insereDados = new MySqlCommand(ComandosSQL.insereDados, conexao);
            MySqlCommand atualizaProdutoCarrinho = new MySqlCommand(ComandosSQL.atualizaProdutoCarrinho, conexao);

            insereDados.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";
            insereDados.Parameters.Add("@idRevendedor", MySqlDbType.VarChar, 60).Value = "2";
            insereDados.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = Intent.GetStringExtra("txtCodigo");
            insereDados.Parameters.Add("@quantidade", MySqlDbType.VarChar, 60).Value = txtQuantidade.Text;

            atualizaProdutoCarrinho.Parameters.Add("@quantidade", MySqlDbType.VarChar, 60).Value = txtQuantidade.Text;
            atualizaProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";
            atualizaProdutoCarrinho.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = Intent.GetStringExtra("txtCodigo");

            registro = Intent.GetLongExtra("numRegistro", 0);

            if (registro > 0) {
                conexao.Open();
                atualizaProdutoCarrinho.ExecuteNonQuery();
                conexao.Close();
                Toast.MakeText(Application.Context, "O produto foi atualizado.", ToastLength.Long).Show();
            } else {
                conexao.Open();
                insereDados.ExecuteNonQuery();
                conexao.Close();
                Toast.MakeText(Application.Context, "O produto foi incluído ao carrinho.", ToastLength.Long).Show();
          }
            
            Finish();
        }

        #region EXEMPLO EXIBE IMAGEM
        /*
        private void BtnIncluir_Click(object sender, System.EventArgs e) {

            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand comando = new MySqlCommand(ComandosSQL.verificaProduto, conexao);
            MySqlDataReader reader;

            comando.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = ColetaCodigo.codigo.ToString();

            conexao.Open();
            reader = comando.ExecuteReader();

            while (reader.Read()) {
                byte[] imagem = (byte[])(reader["imagem"]);
                if (imagem == null) {
                    imgProduto.ImageMatrix = null;
                } else {
                    Bitmap bitmap = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
                    imgProduto.SetImageBitmap(bitmap);
                }
            }
            conexao.Close();
        }*/
        #endregion

    }
}