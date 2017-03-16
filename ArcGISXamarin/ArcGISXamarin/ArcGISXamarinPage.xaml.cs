using Xamarin.Forms;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using System;

namespace ArcGISXamarin
{
	public partial class ArcGISXamarinPage : ContentPage
	{
		public ArcGISXamarinPage()
		{
			InitializeComponent();

			Initialize();
		}

		private async void Initialize()
		{

			// Web マップの URL を指定してマップを作成
			var webMap = await Map.LoadFromUriAsync(new Uri("https://arcgis.com/home/item.html?id=9a6a1c9f857a4a68a6e405bb5917e620"));

			// マップビューのマップに設定 
			MyMapView.Map = webMap;

			await MyMapView.Map.LoadAsync();

			// マップがロードされた際の処理
			if (MyMapView.Map.LoadStatus == LoadStatus.Loaded)
			{
				// マップビューのタップ イベントを登録
				//MyMapView.GeoViewTapped += OnMapViewTapped;

			}

		}
	}
}
