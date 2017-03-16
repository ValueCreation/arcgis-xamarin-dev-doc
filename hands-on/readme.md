# Xamarin ハンズオン（ArcGIS を 利用した地図アプリ開発手順書）

これは、Xamarin.Forms と Esri のクラウドサービス [ArcGIS Online](http://www.arcgis.com/features/index.html) 使った簡単な地図アプリを作るハンズオンです。
地図アプリの開発には [ArcGIS Runtime SDK for .NET](https://developers.arcgis.com/net/latest/) を利用します。

## 今回 作るものは？

ArcGIS Online で作成した WebMap を Xamarin.Forms で表示するアプリを作成します。 
さらに作成したアプリに対して、ジオコーディングや空間検索などの機能を追加していきます。

![](https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/architecture.png)

[WebMap](https://www.esrij.com/gis-guide/web-gis/web-map/) は、道路地図・地形図・衛星画像といったベースマップ（背景地図）の上に、ユーザーが持つデータや Web 上に存在する地図サービスを重ね合わせ、データの表示スタイルやポップアップ（マップ上でポイントなどをクリックした際に表示される情報ウィンドウ）を設定することができます。

## 開発環境

Windows でも Mac でも良いです。

|OS|OS のバージョン|要インストール済|
|----|----|----|
|Windows|Windows 10|Xamarin インストール済みの Visual Studio (2015 Update 3｜2017 RC)|
|Mac OS X|10.11 ("El Capitan") 以降 |(Xamarin Studio もしくは VS for Mac) と 最新の Xcode |

詳細については[システム要件](https://developers.arcgis.com/net/latest/forms/guide/system-requirements.htm)を参照してください。

## 手を動かそう

それではここから実際に手を動かしながらやっていきましょう。

### 手順 1:プロジェクトの作成
最初にプロジェクトの作成からはじめます。Xamarin Staudio の新しいソリューションから新しいプロジェクト用のテンプレートを選択します。
今回は、Xamarin.Forms を利用して開発するため、Forms App を選択します。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_1.png" width="500px">

App Name には任意の名前（例：ArcGISXamarin）を入力して、Shard Code は、 『`Use Shared Libray`』を選択します。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_2.png" width="500px">

作成ボタンをクリックしてプロジェクトの作成を行います。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_3.png" width="500px">

次に ArcGIS Runtime SDK for .NET のインストールを行っていきます。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_4.png" width="500px">

### 手順 2: ArcGIS Runtime SDK NuGet パッケージのインストール

ArcGIS Runtime SDK for .NET は、NuGet パッケージからインストールすることができます。
NuGet パッケージのインストールは、Android、iOS とそれぞれに対してインストールを行います。

#### Android
ソリューションの『パッケージ』を右クリックして、『パッケージの追加』をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_4.png" height="400px">

以下のような「パッケージを追加画面」が起動しますので、検索項目の欄に esri と入力して検索を行います。
いくつかパッケージが表示されますが、ここでは 『ArcGISRuntime SDK for .NET - Xamarin.Forms』 を選択して、『Add Pakages』 をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_5.png" width="500px">

ライセンスの同意画面でライセンスに同意してパッケージの追加を行います。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_2.png" width="500px">

#### iOS
ソリューションの『パッケージ』を右クリックして、『パッケージの追加』をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_6.png" height="400px">

以下のような「パッケージを追加画面」が起動しますので、検索項目の欄に esri と入力して検索を行います。
いくつかパッケージが表示されますが、ここでは 『ArcGISRuntime SDK for .NET - Xamarin.Forms』 を選択して、『Add Pakages』 をクリックします。


インストールに関しては、以下の URL からでも確認することができます。
https://developers.arcgis.com/net/latest/forms/guide/install-the-sdk.htm



### 手順 3: WebMapの表示

#### ArcGISXamarinPage.xaml


```xml
  <Grid>
    <esriUI:MapView x:Name="MyMapView"/>
  </Grid>
```

```xml
  xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
  xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime" 
```

```xml
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" 
	         xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	         xmlns:local="clr-namespace:ArcGISXamarin"
	         xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
                 xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime" 
	         x:Class="ArcGISXamarin.ArcGISXamarinPage">

  <Grid>
    <esriUI:MapView x:Name="MyMapView"/>
  </Grid>

</ContentPage>
```
#### ArcGISXamarinPage.xaml.cs

```csharp
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

		}
	}
}
```

##### アプリの実行


### 手順 4: 機能追加

ここまで作成したアプリについて以下の機能を追加してみましょう。

### 1. ジオコーディング
店舗情報や顧客情報などに含まれる住所情報を XY 座標に変換し地図上にマッピングすることができます。

#### ArcGISXamarinPage.xaml

```xml
 <Grid>
	<Grid.RowDefinitions>
 	  <RowDefinition Height="25" />			
 	  <RowDefinition Height="auto" />
	  <RowDefinition Height="*" />
	</Grid.RowDefinitions>
		
    <StackLayout Orientation="Vertical" Grid.Row="1">
      <Entry x:Name="addressTextBox" Text="東京都千代田区平河町2-7-1" />
      <Button x:Name="geocoording" Text=" 検索 " Clicked="geocoording_Click" />
    </StackLayout>

	<esriUI:MapView x:Name="MyMapView" Grid.Row="2"/>

  </Grid>
```

```xml
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" 
	         xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	         xmlns:local="clr-namespace:ArcGISXamarin" 
	         xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
                 xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime" 
	         x:Class="ArcGISXamarin.ArcGISXamarinPage">

  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="25" />			
      <RowDefinition Height="auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>
		
    <StackLayout Orientation="Vertical" Grid.Row="1">
      <Entry x:Name="addressTextBox" Text="東京都千代田区平河町2-7-1" />
      <Button x:Name="geocoording" Text=" 検索 " Clicked="geocoording_Click" />
    </StackLayout>

    <esriUI:MapView x:Name="MyMapView" Grid.Row="2"/>

  </Grid>
</ContentPage>
```


#### ArcGISXamarinPage.xaml.cs

```csharp
using Xamarin.Forms;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using System;
using System.Linq;
using System.Threading.Tasks;

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

	}
}
```

### アプリの実行

実行後の画面はそれぞれ以下のようになります。

### iOS
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/geocoding_ios.png" height="500px">

### Android
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/geocoding_android.png" height="500px">

### 2. 空間解析
重複するエリアの抽出や空間的な分布傾向の把握など、多様な空間解析機能を利用することができます。

### アプリの実行


##　宿題
いったん本ハンズオンではここで終了ですが、お時間のある方は 3D の地図表現に挑戦してみましょう。 


### 1.宿題


### 2.宿題


