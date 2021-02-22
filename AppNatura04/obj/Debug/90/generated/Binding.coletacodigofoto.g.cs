using global::System;
using global::Android.App;
using global::Android.Widget;
using global::Android.Views;
using global::Android.OS;

namespace Binding
{
	sealed class coletacodigofoto : global::Xamarin.Android.Design.LayoutBinding
	{

		[global::Android.Runtime.PreserveAttribute (Conditional=true)]
		public coletacodigofoto (
			global::Android.App.Activity client,
			global::Xamarin.Android.Design.OnLayoutItemNotFoundHandler itemNotFoundHandler = null)
				: base (client, itemNotFoundHandler)
		{}

		[global::Android.Runtime.PreserveAttribute (Conditional=true)]
		public coletacodigofoto (
			global::Android.Views.View client,
			global::Xamarin.Android.Design.OnLayoutItemNotFoundHandler itemNotFoundHandler = null)
				: base (client, itemNotFoundHandler)
		{}


		#line 8 "Resources\layout\coletacodigofoto.xml"

		ImageView __fotoView;

		#line default
		#line hidden

		#line 8 "Resources\layout\coletacodigofoto.xml"

		// Declared in: Resources\layout\coletacodigofoto.xml:(8:6)
		// Declared in: Resources\layout\coletacodigofoto.xml:(8:6)
		public ImageView fotoView => FindView (global::AppNaturaCliente.Resource.Id.fotoView, ref __fotoView);

		#line default
		#line hidden


		#line 15 "Resources\layout\coletacodigofoto.xml"

		Button __btn_abrir;

		#line default
		#line hidden

		#line 15 "Resources\layout\coletacodigofoto.xml"

		// Declared in: Resources\layout\coletacodigofoto.xml:(15:6)
		// Declared in: Resources\layout\coletacodigofoto.xml:(15:6)
		public Button btn_abrir => FindView (global::AppNaturaCliente.Resource.Id.btn_abrir, ref __btn_abrir);

		#line default
		#line hidden


		#line 21 "Resources\layout\coletacodigofoto.xml"

		Button __btn_ler;

		#line default
		#line hidden

		#line 21 "Resources\layout\coletacodigofoto.xml"

		// Declared in: Resources\layout\coletacodigofoto.xml:(21:6)
		// Declared in: Resources\layout\coletacodigofoto.xml:(21:6)
		public Button btn_ler => FindView (global::AppNaturaCliente.Resource.Id.btn_ler, ref __btn_ler);

		#line default
		#line hidden


		#line 28 "Resources\layout\coletacodigofoto.xml"

		TextView __txt_result;

		#line default
		#line hidden

		#line 28 "Resources\layout\coletacodigofoto.xml"

		// Declared in: Resources\layout\coletacodigofoto.xml:(28:6)
		// Declared in: Resources\layout\coletacodigofoto.xml:(28:6)
		public TextView txt_result => FindView (global::AppNaturaCliente.Resource.Id.txt_result, ref __txt_result);

		#line default
		#line hidden


		#line 34 "Resources\layout\coletacodigofoto.xml"

		ListView __lista_produtos;

		#line default
		#line hidden

		#line 34 "Resources\layout\coletacodigofoto.xml"

		// Declared in: Resources\layout\coletacodigofoto.xml:(34:6)
		// Declared in: Resources\layout\coletacodigofoto.xml:(34:6)
		public ListView lista_produtos => FindView (global::AppNaturaCliente.Resource.Id.lista_produtos, ref __lista_produtos);

		#line default
		#line hidden

	}
}
