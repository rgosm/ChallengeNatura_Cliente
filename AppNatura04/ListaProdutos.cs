using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AppNaturaCliente
{
    [Activity(Label ="Lista produtos selecionar")]
    class ListaProdutos : ListActivity
    {
        bool varBool = false;

        private static string descricao = null;
        private static string preco = null;
        private static string quantidade = null;
        private static Bitmap imagemProduto = null;
        private static long numeroRegistro;

        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string[] products2 = Intent.GetStringArrayExtra("codigosProds");

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, products2); ;
            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                Regex regex2 = new Regex(@"([\d]{5})+");

                Console.WriteLine(((TextView)args.View).Text);
                Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();

                Match codigo = regex2.Match(((TextView)args.View).Text);

                if (codigo.ToString() != "" && !varBool)
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
                            byte[] imagem = (byte[])(produtoCarrinho["imagem"]);
                            imagemProduto = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
                            descricao = produtoCarrinho.GetString("descricao").ToString();
                            preco = produtoCarrinho.GetString("preco").ToString();
                            quantidade = produtoCarrinho.GetString("quantidade").ToString();
                            StartActivity(typeof(ExibeProduto));
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
                            byte[] imagem = (byte[])(retorno["imagem"]);
                            imagemProduto = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length);
                            descricao = retorno.GetString("descricao").ToString();
                            preco = retorno.GetString("preco").ToString();
                            quantidade = "1";
                            StartActivity(typeof(ExibeProduto));
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
            };
            
        }

       

    }
}