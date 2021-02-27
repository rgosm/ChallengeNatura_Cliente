using System;
using System.Collections.Generic;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace AppNaturaCliente
{
    [Activity(Label = "ListaCarrinho")]

    #region Lista
    /*
    public class ListaCarrinho : Activity {
        private ListView listView;
        private List<string> lista;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaCarrinho);

            listView = FindViewById<ListView>(Resource.Id.listView);

            listView.ItemClick += ListView_ItemClick;

            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand verificaProduto = new MySqlCommand(ComandosSQL.exibeListaCarrinho, conexao);

            MySqlDataReader retorno;

            verificaProduto.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";

            conexao.Open();
            retorno = verificaProduto.ExecuteReader();

            lista = new List<string>();

            while (retorno.Read()) {
                Lista produtoCarrinho = new Lista();
                produtoCarrinho.descricao = retorno.GetString("descricao").ToString();
                lista.Add(produtoCarrinho.descricao);
            }
            conexao.Close();

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lista);
            listView.Adapter = adapter;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
           using(var dialog = new AlertDialog.Builder(this)) {
                int posicao = e.Position;
                string valor = lista[posicao];
                dialog.SetTitle("descricao");
                dialog.SetMessage(valor);
                dialog.Show();
            }
        }
    }
    */
    #endregion

    public class ListaCarrinho : Activity {
        private ListView listView;
        private List<Lista> lista;
        private TextView txtTotal;
        private Button btnExcluir;
        private Button btnEnviar;
        private static string codigo = null;
        private static string descricao = null;
        private static string preco = null;
        private static string quantidade = null;
        private static Bitmap imagemProduto = null;
        private static long numeroRegistro;
        private static byte[] imagem;

        public static string Codigo { get => codigo; set => codigo = value; }
        public static Bitmap ImagemProduto { get => imagemProduto; set => imagemProduto = value; }
        public static string Descricao { get => descricao; set => descricao = value; }
        public static string Preco { get => Preco1; set => Preco1 = value; }
        public static string Preco1 { get => preco; set => preco = value; }
        public static string Quantidade { get => quantidade; set => quantidade = value; }
        public static long NumeroRegistro { get => numeroRegistro; set => numeroRegistro = value; }
        public static byte[] Imagem { get => imagem; set => imagem = value; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaCarrinho);

            listView = FindViewById<ListView>(Resource.Id.listView);
            txtTotal = FindViewById<TextView>(Resource.Id.txtTotal);
            btnExcluir = FindViewById<Button>(Resource.Id.btnExcluir);
            btnEnviar = FindViewById<Button>(Resource.Id.btnEnviar);

            listView.ItemClick += ListView_ItemClick;
            btnExcluir.Click += BtnExcluir_ItemClick;
            btnEnviar.Click += BtnEnviar_ItemClick;

            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand verificaProduto = new MySqlCommand(ComandosSQL.exibeListaCarrinho, conexao);

            MySqlDataReader retorno;

            verificaProduto.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";

            conexao.Open();
            retorno = verificaProduto.ExecuteReader();

            lista = new List<Lista>();

            while (retorno.Read()) {
                lista.Add(new Lista() {
                    codigo = "Cod. " + retorno.GetString("idProduto").ToString(),
                    descricao = retorno.GetString("descricao").ToString(),
                    quantidade = retorno.GetString("quantidade").ToString() + " unid.",
                    valorTotal = "Valor Total " + (Convert.ToInt32(retorno.GetString("quantidade")) * Convert.ToDouble(retorno.GetString("preco"))).ToString("C", CultureInfo.CurrentCulture),
                    somaValorTotal = (Convert.ToInt32(retorno.GetString("quantidade")) * Convert.ToDouble(retorno.GetString("preco"))).ToString()
                });

                double total = 0;
                foreach (var s in lista) {
                    total = Convert.ToDouble(s.somaValorTotal) + total;
                }

                txtTotal.Text = "Total: " + total.ToString("C", CultureInfo.CurrentCulture);
            }

            conexao.Close();

            ListViewAdapter adapter = new ListViewAdapter(this, lista);
            listView.Adapter = adapter;
        }

        private void BtnEnviar_ItemClick(object sender, EventArgs e) {
            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand enviaCarrinho = new MySqlCommand(ComandosSQL.enviaCarrinho, conexao);

            enviaCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";

            conexao.Open();

            enviaCarrinho.ExecuteNonQuery();

            conexao.Close();

            Finish();
            Toast.MakeText(Application.Context, "O carrinho foi enviado.", ToastLength.Long).Show();
        }

        private void BtnExcluir_ItemClick(object sender, EventArgs e) {
            MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
            MySqlCommand excluiTodosProdutoCarrinho = new MySqlCommand(ComandosSQL.excluiTodosProdutoCarrinho, conexao);

            excluiTodosProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";
            
            conexao.Open();

            excluiTodosProdutoCarrinho.ExecuteNonQuery();

            conexao.Close();

            Finish();
            Toast.MakeText(Application.Context, "O carrinho foi excluído.", ToastLength.Long).Show();
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
            using (var dialog = new AlertDialog.Builder(this)) {
                MySqlConnection conexao = new MySqlConnection(Conexao.strConexao);
                MySqlCommand selecionaProdutoCarrinho = new MySqlCommand(ComandosSQL.selecionaProdutoCarrinho, conexao);
                MySqlDataReader produtoCarrinho;

                selecionaProdutoCarrinho.Parameters.Add("@idProduto", MySqlDbType.VarChar, 60).Value = String.Join("", System.Text.RegularExpressions.Regex.Split(lista[e.Position].codigo, @"[^\d]")).ToString();
                selecionaProdutoCarrinho.Parameters.Add("@idCliente", MySqlDbType.VarChar, 60).Value = "1";

                conexao.Open();

                produtoCarrinho = selecionaProdutoCarrinho.ExecuteReader();


                if (produtoCarrinho.Read()) {
                    Imagem = (byte[])(produtoCarrinho["imagem"]);
                    Codigo = String.Join("", System.Text.RegularExpressions.Regex.Split(lista[e.Position].codigo, @"[^\d]")).ToString();
                    Descricao = produtoCarrinho.GetString("descricao").ToString();
                    Preco = produtoCarrinho.GetString("preco").ToString();
                    Quantidade = produtoCarrinho.GetString("quantidade").ToString();
                    NumeroRegistro = 2;
                    IniciaExibeProduto();
                    Finish();
                }

            }
        }

        public void IniciaExibeProduto()
        {
            var exibirProdutos = new Intent(this, typeof(ExibeProduto));
            exibirProdutos.PutExtra("txtDescricao", Descricao.ToString());
            exibirProdutos.PutExtra("txtCodigo", Codigo.ToString());
            exibirProdutos.PutExtra("txtPrecoUnitario", "Valor unitário: " + (Convert.ToDouble(Preco)).ToString("C", CultureInfo.CurrentCulture));
            exibirProdutos.PutExtra("txtQuantidade", Quantidade.ToString());
            exibirProdutos.PutExtra("txtPrecoTotal", (Convert.ToInt32(Quantidade.ToString()) * Convert.ToDouble(Preco)).ToString("C", CultureInfo.CurrentCulture));
            exibirProdutos.PutExtra("imgProduto", Imagem);
            exibirProdutos.PutExtra("preco", Preco);
            exibirProdutos.PutExtra("numRegistro", NumeroRegistro);
            exibirProdutos.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(exibirProdutos);
            Finish();
        }
    }

}