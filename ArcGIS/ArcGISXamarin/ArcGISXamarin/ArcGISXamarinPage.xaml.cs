using Xamarin.Forms;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else
using Colors = System.Drawing.Color;
#endif

namespace ArcGISXamarin
{
	public partial class ArcGISXamarinPage : ContentPage
	{

		//ArcGIS Online ジオコーディングサービスの URL
		private const string WORLD_GEOCODE_SERVICE_URL = "https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer";

		//住所検索結果表示用のグラフィックスオーバーレイ
		private GraphicsOverlay geocodeResultGraphicsOverlay;

		//住所検索用のジオコーディング タスク  
		private LocatorTask onlineLocatorTask;

		//マップが操作可能であるかどうかを示す変数
		private bool isMapReady;

		// フィーチャーレイヤの定義
		private FeatureLayer myFeatureLayer;

		// 属性データを呼び出すためのフィーチャーテーブルの定義
		private ServiceFeatureTable myFeatureTable;

		// 検索結果のフィーチャのリスト
		private List<Feature> myFeatures = new List<Feature>();

		//空間検索表示用のグラフィックスオーバーレイ
		private GraphicsOverlay myGraphicsOverlay = new GraphicsOverlay();


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

			//住所検索用のジオコーディング タスクを初期化
			onlineLocatorTask = await LocatorTask.CreateAsync(new Uri(WORLD_GEOCODE_SERVICE_URL));

			// グラフィックス オーバーレイが存在しない場合は、新規に追加
			if (MyMapView.GraphicsOverlays.Count == 0)
			{
				geocodeResultGraphicsOverlay = new GraphicsOverlay()
				{
					Renderer = createGeocoordingSymbol(),
				};
				MyMapView.GraphicsOverlays.Add(geocodeResultGraphicsOverlay);
			}

			isMapReady = true;

			// マップがロードされた際の処理
			if (MyMapView.Map.LoadStatus == LoadStatus.Loaded)
			{
				// マップビューのタップ イベントを登録
				MyMapView.GeoViewTapped += OnMapViewTapped;

				// Web マップに含まれる最上位のレイヤーを取得
				myFeatureLayer = (FeatureLayer)MyMapView.Map.OperationalLayers[1];

				// フィーチャ レイヤからフィーチャ テーブルを定義
				myFeatureTable = (ServiceFeatureTable)myFeatureLayer.FeatureTable;

				// リクエスト モードの設定
				myFeatureTable.FeatureRequestMode = FeatureRequestMode.ManualCache;

				// フィーチャの検索用のパラメーターを作成
				var queryParams = new QueryParameters();

				// すべてのフィーチャを取得するように条件を設定
				queryParams.WhereClause = "1=1";

				// 検索結果にフィーチャのすべての属性情報（outFields の配列に "*" を指定）を含める
				var outputFields = new string[] { "*" };

				// クエリの条件に基づいてフィーチャ テーブルにデータを設定する
				await myFeatureTable.PopulateFromServiceAsync(queryParams, true, outputFields);

				// マップビューにグラフィック表示用のオーバレイを追加
				MyMapView.GraphicsOverlays.Add(myGraphicsOverlay);
			}

		}

		// ジオコーディングの実行
		private async void geocoording_Click(object sender, EventArgs e)
		{
			//マップが準備できていなければ処理を行わない
			if (!isMapReady) return;

			//住所検索用のパラメータを作成
			var geocodeParams = new GeocodeParameters
			{
				MaxResults = 5,
				OutputSpatialReference = SpatialReferences.WebMercator,
				CountryCode = "Japan",
				OutputLanguage = new System.Globalization.CultureInfo("ja-JP"),
			};

			try
			{
				//住所の検索
				var resultCandidates = await onlineLocatorTask.GeocodeAsync(addressTextBox.Text, geocodeParams);

				//住所検索結果に対する処理（1つ以上候補が返されていれば処理を実行）
				if (resultCandidates != null && resultCandidates.Count > 0)
				{
					//現在の結果を消去
					geocodeResultGraphicsOverlay.Graphics.Clear();

					//常に最初の候補を採用
					var candidate = resultCandidates.FirstOrDefault();

					//最初の候補からグラフィックを作成
					Graphic locatedPoint = new Graphic()
					{
						Geometry = candidate.DisplayLocation,
					};

					//住所検索結果表示用のグラフィックスオーバーレイにグラフィックを追加
					geocodeResultGraphicsOverlay.Graphics.Add(locatedPoint);

					//追加したグラフィックの周辺に地図を拡大
					await MyMapView.SetViewpointCenterAsync((MapPoint)locatedPoint.Geometry, 66112);
				}
				//候補が一つも見つからない場合の処理
				else
				{
					await DisplayAlert("住所検索","該当する場所がみつかりません。", "OK");
				}
			}
			//エラーが発生した場合の処理
			catch (Exception ex)
			{
				await DisplayAlert("住所検索", string.Format("{0}", ex.Message), "OK");
			}
		}

		// 住所検索結果用のシンボル作成
		private SimpleRenderer createGeocoordingSymbol()
		{
			SimpleMarkerSymbol resultGeocoordingSymbol = new SimpleMarkerSymbol()
			{
				Style = SimpleMarkerSymbolStyle.Circle,
				Size = 12,
				Color = Colors.Blue,
			};

			SimpleRenderer resultRenderer = new SimpleRenderer() { Symbol = resultGeocoordingSymbol };

			return resultRenderer;
		}

		private async void OnMapViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
		{
			try
			{

				Console.WriteLine("タップしました");

				// 2回目以降の検索時でフィーチャが選択されている場合は、選択を解除
				if (myFeatures != null && myFeatures.Count() != 0)
				{
					myFeatureLayer.UnselectFeatures(myFeatures);
				}

				// グラフィック オーバレイに追加したグラフィックを削除
				myGraphicsOverlay.Graphics.Clear();

				// タップした地点から1000メートルのバッファーの円を作成し、グラフィックとして表示する
				var buffer = GeometryEngine.Buffer(e.Location, 1000);
				var outLineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.DashDot, System.Drawing.Color.Yellow, 5);
				var fillSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Null, System.Drawing.Color.White, outLineSymbol);
				var graphic = new Graphic(buffer, null, fillSymbol);

				myGraphicsOverlay.Graphics.Add(graphic);

				// フィーチャの検索用のパラメーターを作成
				var queryParams = new QueryParameters();
				// 検索範囲を作成したバファーの円に指定
				queryParams.Geometry = buffer;

				// 検索範囲とフィーチャの空間的な関係性を指定（バファーの円の中にフィーチャが含まれる）
				queryParams.SpatialRelationship = SpatialRelationship.Contains;
				// フィーチャの検索を実行
				FeatureQueryResult queryResult = await myFeatureLayer.FeatureTable.QueryFeaturesAsync(queryParams);

				var alertString = "";

				// 検索結果のフィーチャのリストを取得
				myFeatures = queryResult.ToList();
				// 検索結果のフィーチャを選択（ハイライト表示）
				myFeatureLayer.SelectFeatures(myFeatures);

				for (int i = 0; i < myFeatures.Count; ++i)
				{
					Feature feature = myFeatures[i];
					// フィーチャの"Name"フィールドの属性値を取得
					var nameStr = feature.GetAttributeValue("物件名");
					alertString = alertString + Environment.NewLine + nameStr;
					Console.WriteLine(nameStr);
				}

				// 取得した属性値をアラート表示
				await DisplayAlert("検索結果", alertString, "OK");

			}

			catch (Exception ex)
			{
				await DisplayAlert("検索のエラー", ex.ToString(), "OK");
			}

		}

	}
}
