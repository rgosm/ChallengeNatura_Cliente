using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Text.RegularExpressions;

namespace AppNaturaCliente
{
    [Activity(Label ="Lista produtos selecionar")]
    class ListaProdutos : ListActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string[] products2 = Intent.GetStringArrayExtra("codigosProds");

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, products2); ;
            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                Regex regexRetorno = new Regex(@"([\d]{5})+");
                ColetaCodigo coletaCodigo = new ColetaCodigo();

                Toast.MakeText(Application, "Adicionando "+((TextView)args.View).Text+ " ao carrinho", ToastLength.Short).Show();
                Match codigo = regexRetorno.Match(((TextView)args.View).Text);

                coletaCodigo.BuscaBanco(this,codigo.ToString());
                //Finish();
            };
            
        }

    }
}