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

	}
}
