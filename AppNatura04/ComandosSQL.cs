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

namespace AppNaturaCliente {
    public static class ComandosSQL { 
        public static string insereDados = "insert into tblCarrinhoCompra(idCarrinhoCompra, idCliente, idRevendedor, idProduto, quantidade, status) values(null, @idCliente, @idRevendedor, @idProduto, @quantidade, 'Processando')";
        public static string verificaProduto = "SELECT * FROM tblProduto WHERE idProduto = @idProduto";
        public static string exibeListaCarrinho = "SELECT * FROM tblCarrinhoCompra c, tblProduto p WHERE c.idProduto = p.idProduto and c.idCliente=@idCliente";
        public static string contaProdutoCarrinho = "SELECT count(*) FROM tblCarrinhoCompra WHERE idProduto = @idProduto and idCliente = @idCliente";
        public static string selecionaProdutoCarrinho = "SELECT * FROM tblCarrinhoCompra c, tblProduto p WHERE c.idProduto = p.idProduto and c.idCliente = @idCliente and c.idProduto = @idProduto";
        public static string atualizaProdutoCarrinho = "UPDATE tblCarrinhoCompra SET quantidade = @quantidade WHERE idCliente = @idCliente and idProduto = @idProduto";
        public static string excluiProdutoCarrinho = "DELETE FROM tblCarrinhoCompra WHERE idProduto = @idProduto and idCliente = @idCliente and status = 'Processando'";
        public static string excluiTodosProdutoCarrinho = "DELETE FROM tblCarrinhoCompra WHERE idCliente = @idCliente and status = 'Processando'";
        public static string enviaCarrinho = "UPDATE tblCarrinhoCompra SET status = 'Enviado' WHERE idCliente = @idCliente and status = 'Processando'";

    }
}
