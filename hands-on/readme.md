# Xamarin ハンズオン（ArcGIS を 利用した地図アプリ開発手順書）

これは、Xamarin.Forms と Esri のクラウドサービス [ArcGIS Online](http://www.arcgis.com/features/index.html) 使った簡単な地図アプリを作るハンズオンです。
地図アプリの開発には [ArcGIS Runtime SDK for .NET](https://developers.arcgis.com/net/latest/) を利用します。

## 今回 作るものは？

ArcGIS Online で作成した [WebMap](https://www.arcgis.com/home/webmap/viewer.html?webmap=9a6a1c9f857a4a68a6e405bb5917e620) を Xamarin.Forms で表示するアプリを作成します。 
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

### 手順 1: プロジェクトの作成
最初にプロジェクトの作成からはじめます。Xamarin Staudio の新しいソリューションから新しいプロジェクト用のテンプレートを選択します。
今回は、Xamarin.Forms を利用して開発するため、Forms App を選択します。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_1.png" width="500px">

App Name には任意の名前（例：ArcGISXamarin）を入力して、Shared Code は、 『`Use Shared Libray`』を選択します。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_2.png" width="500px">

作成ボタンをクリックしてプロジェクトの作成を行います。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_3.png" width="500px">

プロジェクトが作成されます。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/project_4.png" width="500px">

次に ArcGIS Runtime SDK for .NET のインストールを行っていきます。

### 手順 2: ArcGIS Runtime SDK NuGet パッケージのインストール

ArcGIS Runtime SDK for .NET は、NuGet パッケージからインストールすることができます。
NuGet パッケージのインストールは、Android、iOS とそれぞれに対してインストールを行います。

#### Android
ソリューションの 『パッケージ』 を右クリックして、『パッケージの追加』 をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_4.png" height="500px">

以下のような「パッケージを追加画面」が起動しますので、検索項目の欄に esri と入力して検索を行います。
いくつかパッケージが表示されますが、ここでは 『ArcGISRuntime SDK for .NET - Xamarin.Forms』 を選択して、『Add Pakages』 をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_5.png" width="500px">

ライセンスの同意画面でライセンスに同意してパッケージの追加を行います。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_2.png" width="500px">

#### iOS
ソリューションの 『パッケージ』 を右クリックして、『パッケージの追加』 をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_6.png" height="500px">

以下のような「パッケージを追加画面」が起動しますので、検索項目の欄に esri と入力して検索を行います。
いくつかパッケージが表示されますが、ここでは 『ArcGISRuntime SDK for .NET - Xamarin.Forms』 を選択して、『Add Pakages』 をクリックします。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_5.png" width="500px">

ライセンスの同意画面でライセンスに同意してパッケージの追加を行います。

<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/nuget_7.png" width="500px">

これでArcGIS Runtime SDK NuGet パッケージのインストールは完了です。

インストール方法に関しては、以下の URL からでも確認することができます。
https://developers.arcgis.com/net/latest/forms/guide/install-the-sdk.htm

### 手順 3: WebMapの表示

ここから ArcGIS で作成済みの WebMap を Xamarin で表示していきます。

地図を表示する部分 Xamarin.Forms ユーザーインタフェースとして、**ArcGISXamarin/ArcGISXamarinPage.xaml** に UI を作成していきます。
地図表示(ユーザインタフェース)は **XAML**(ざむる)という、マークアップ言語で書いていきます。(Extensible Application Markup Language)

#### ArcGISXamarinPage.xaml

まず、MapView コントロールをページに追加するには、ArcGIS Runtime assembly のプロジェクト参照に加えて、XAML 名前空間を割り当てる必要があります。

以下の内容を追加します。

```xml
  xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
  xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime" 
```
次に、ContentPage の中に、次のように Grid の中に MapView クラスを追加します：

```xml
  <Grid>
    <esriUI:MapView x:Name="MyMapView"/>
  </Grid>
```

【確認】現在、`ArcGISXamarinPage.xaml`は、次のようになっているはずです。

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

次に WebMap を呼び出し部分を作成していきましょう。

1. プロジェクトの中の `ArcGISXamarin/ArcGISXamarinPage.xaml` ファイルを開きます。
2. 以下のような内容で WebMap を呼び出す部分を作成していきます。

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

		}
	}
}
```

ここで WebMap を呼び出していますが、`id=9a6a1c9f857a4a68a6e405bb5917e620` この id を変えることで ArcGIS で作成した様々な WebMap を呼び出すことが可能です。また、ArcGIS 側で WebMap の定義（例：新しいデータの追加など）を変えるだけですぐにアプリ側にも反映することが可能です。

```csharp
// Web マップの URL を指定してマップを作成
var webMap = await Map.LoadFromUriAsync(new Uri("https://arcgis.com/home/item.html?id=9a6a1c9f857a4a68a6e405bb5917e620"));
```

地図を表示する仕組みとして、Map オブジェクトと、それらを表示する MapView を利用します。Map オブジェクトには、操作レイヤー、ベースマップ、ブックマーク等の ArcGIS 固有のデータを設定することができてアプリケーションで利用することができます。MapView は、UI コンポーネントです。MapView クラスの map プロパティに、Map オブジェクトを設定することで地図を表示することができます。

### アプリの実行

次にアプリを実行して地図を表示してみます。

### iOS
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/webmap_1.png" height="500px">

### Android
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/webmap_3.png" height="500px">

以下のような ArcGIS であらかじめ作成した [WebMap](https://www.arcgis.com/home/webmap/viewer.html?webmap=9a6a1c9f857a4a68a6e405bb5917e620)
 がアプリ側でも表示されているのが分かるかと思います。
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/webmap_2.png" widht="500px">

### 手順 4: 機能追加

ここからモクモクタイムです。

ここまで作成したアプリについて以下の好きな機能をそれぞれ追加していきます。

### 1. ジオコーディング
ジオコーディングとは店舗情報や顧客情報などに含まれる住所情報を XY 座標に変換し地図上にマッピングすることができます。

今回は入力した任意の住所に対してジオコーディングを行い、その地点を XY 座標に変換して地図上にマッピングする機能を作成していきます。

先ほど作成した **ArcGISXamarin/ArcGISXamarinPage.xaml** にジオコーディング用の UI を作っていきましょう。

#### ArcGISXamarinPage.xaml

住所を入力する UI と入力された住所を検索するためのボタンを追加します。

```xml
 <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="25" />			
      <RowDefinition Height="auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>
		
    <StackLayout Orientation="Vertical" Grid.Row="1">
      <Entry x:Name="addressTextBox" Text="千葉県千葉市中央区新千葉1丁目" />
      <Button x:Name="geocoording" Text=" 検索 " Clicked="geocoording_Click" />
    </StackLayout>

    <esriUI:MapView x:Name="MyMapView" Grid.Row="2"/>

  </Grid>
```

【確認】現在、`ArcGISXamarinPage.xaml`は、次のようになっているはずです。

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
      <Entry x:Name="addressTextBox" Text="千葉県千葉市中央区新千葉1丁目" />
      <Button x:Name="geocoording" Text=" 検索 " Clicked="geocoording_Click" />
    </StackLayout>

    <esriUI:MapView x:Name="MyMapView" Grid.Row="2"/>

  </Grid>
</ContentPage>
```

#### ArcGISXamarinPage.xaml.cs

C#側では、住所からジオコーディングを実行して、その住所から XY 座標に変換してポイントして地図上にマッピングする機能を作成していきます。

ジオコーディングは ArcGIS の [ArcGIS Online World Geocoding](https://developers.arcgis.com/rest/geocode/api-reference/overview-world-geocoding-service.htm) サービスを利用しています。
ArcGIS の REST サービスを利用していますので Web サービスなどでも利用することができます。

```csharp
var geocodeServiceUrl = @"http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer";
LocatorTask geocodeTask = await LocatorTask.CreateAsync(new Uri(geocodeServiceUrl));
```

住所情報と住所検索に必要なパラメータを設定してジオコーディングサービスを実行します。

```csharp
//住所検索用のパラメータを作成
var geocodeParams = new GeocodeParameters
{
	MaxResults = 5,
	OutputSpatialReference = SpatialReferences.WebMercator,
	CountryCode = "Japan",
	OutputLanguage = new System.Globalization.CultureInfo("ja-JP"),
};

//住所の検索
var resultCandidates = await onlineLocatorTask.GeocodeAsync(addressTextBox.Text, geocodeParams);
```

以下のコードを参考にしてジオコーディングサービスを実装していきます。

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

			// グラフィックス オーバーレイの新規追加
			geocodeResultGraphicsOverlay = new GraphicsOverlay()
			{
				Renderer = createGeocoordingSymbol(),
			};
			MyMapView.GraphicsOverlays.Add(geocodeResultGraphicsOverlay);

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
					await MyMapView.SetViewpointCenterAsync((MapPoint)locatedPoint.Geometry, 36112);
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
空間解析とは、重複するエリアの抽出や空間的な分布傾向の把握など、多様な空間解析機能を利用することができます。

２つ目の機能として地図上で任意の円（バッファー）を表示し、その円（バッファー）内に含まれるデータを取得して表示する機能を作成していきます。
GIS などでは空間検索や空間解析としてよく利用される機能のひとつです。

今回は新規・中古物件の情報を取得して物件名を表示する機能です。

**ArcGISXamarin/ArcGISXamarinPage.xaml** に関してはこれまでの作成したものを使用します。今回 UI に関しては何も手を加えていません。

#### ArcGISXamarinPage.xaml

【確認】現在、ジオコーディング機能を実装している方は `ArcGISXamarinPage.xaml`は、次のようになっているはずです。

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

C#側では、地図をタップし、タップした地点から1000メートルのバッファーの円を作成して、その円に含まれるデータを取得する処理を実装していきます。

地図をタップした時のイベントを登録
```csharp
// マップビューのタップ イベントを登録
MyMapView.GeoViewTapped += OnMapViewTapped;
```
検索の対象となる新規・中古物件用のレイヤーを取得

```csharp
// Web マップに含まれる最上位のレイヤーを取得
myFeatureLayer = (FeatureLayer)MyMapView.Map.OperationalLayers[1];
```
新規・物件用のフィーチャの検索はフィーチャ テーブル （ServiceFeatureTable）に対して行います。フィーチャ サービスから作成したフィーチャ テーブル（ServiceFeatureTable）の場合、フィーチャ テーブルのフィーチャは、マップ上にレンダリングするために必要最小限の情報だけを含むように最適化されています。これにより、フィーチャを表示するための待機時間と帯域幅の消費が削減されます。フィーチャの編集やすべての属性情報を表示するような場合は完全な情報を取得するために、ローダブル パターン等を使用して、フィーチャを明示的にロードしておく必要があります。

```csharp
// フィーチャ レイヤからフィーチャ テーブルを定義
myFeatureTable = (ServiceFeatureTable)myFeatureLayer.FeatureTable;
```

フィーチャ サービスからフィーチャを取得する場合は、 リクエスト モードの設定によってフィーチャの取得頻度とや端末上でのデータのキャッシュ方法を制御します。リクエスト モードには、OnInteractionCache、 OnInteractionNoCache、ManualCache があります。リクエスト モードはフィーチャ テーブルが初期化される前に、ServiceFeatureTable の FeatureRequestMode プロパティを使用して設定できます。今回は、ManualCache を設定しています。ManualCache は、ユーザーによるマップ操作では、フィーチャは自動的にリクエストされません。このモードを使用する場合は、ServiceFeatureTable の PopulateFromServiceAsync メソッドを使用して明示的にデータをリクエストする必要があります。

```csharp
// リクエスト モードの設定
myFeatureTable.FeatureRequestMode = FeatureRequestMode.ManualCache;
// フィーチャの検索用のパラメーターを作成
var queryParams = new QueryParameters();
// すべてのフィーチャを取得するように条件を設定
queryParams.WhereClause = "1=1";
// 検索結果にフィーチャのすべての属性情報（outFields の配列に "*" を指定）を含める
var outputFields = new string[] { "*" };
// クエリの条件に基づいてフィーチャ テーブルにデータを設定
await myFeatureTable.PopulateFromServiceAsync(queryParams, true, outputFields);
```

詳細については[フィーチャの操作](http://esrijapan.github.io/arcgis-dev-resources/dotnet/migration-dotnet-100.x/#フィーチャの操作)をご参照ください。

以下のコードを参考にして空間解析を実装していきます。				

```csharp
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
				// クエリの条件に基づいてフィーチャ テーブルにデータを設定
				await myFeatureTable.PopulateFromServiceAsync(queryParams, true, outputFields);
				// マップビューにグラフィック表示用のオーバレイを追加
				MyMapView.GraphicsOverlays.Add(myGraphicsOverlay);
			}
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
```

### アプリの実行

実行後の画面はそれぞれ以下のようになります。

### iOS
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/geometry_ios.png" height="500px">

### Android
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/geometry_android.png" height="500px">

##　その他

いったん本ハンズオンではここで終了ですが、時間のある方は 3D の地図表現に挑戦してみましょう。 

プロジェクトを新規に作成してから行う必要があります。

### 1. 3Dアプリ

#### iOS
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/3D_1.png" height="500px">

#### SceneSymbolsPage.xaml

```xml
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" 
	     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	     xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
             xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime"
	     xmlns:local="clr-namespace:SceneSymbols" 
             x:Class="SceneSymbols.SceneSymbolsPage">
	     
    <Grid>
     <esriUI:SceneView x:Name="MySceneView" />
   </Grid>

</ContentPage>
```

#### SceneSymbolsPage.xaml.cs

```csharp
using Xamarin.Forms;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Geometry;
using System;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else
using Colors = System.Drawing.Color;
#endif

namespace SceneSymbols
{

	public partial class SceneSymbolsPage : ContentPage
	{

		private GraphicsOverlay graphicsOverlay;

		public SceneSymbolsPage()
		{
			InitializeComponent();
			Initialize();

		}

		private void Initialize()
		{

			var myScene = new Scene(Basemap.CreateTopographic());
			MySceneView.Scene = myScene;

			var surface = new Surface();

			var camera = new Camera(48.973, 4.92, 2082, 60, 75, 0);
			MySceneView.SetViewpointCamera(camera);

			var url = new System.Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");

			var elevationSource = new ArcGISTiledElevationSource(url);
			surface.ElevationSources.Add(elevationSource);
			myScene.BaseSurface = surface;

			graphicsOverlay = new GraphicsOverlay();
			graphicsOverlay.SceneProperties.SurfacePlacement = SurfacePlacement.Absolute;
			MySceneView.GraphicsOverlays.Add(graphicsOverlay);

			addGraphics();

		}

		private void addGraphics()
		{

			var x = 4.975;
			var y = 49.0;
			var z = 500.0;

			//create symbols for all the available 3D symbols
			SimpleMarkerSceneSymbol[] symbols = new SimpleMarkerSceneSymbol[6];

			var coneSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Cone,
				Color = Colors.Red,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			var cubeSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Cube,
				Color = Colors.Aqua,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			var cylinderSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Cylinder,
				Color = Colors.Yellow,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			var diamondSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Diamond,
				Color = Colors.Blue,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			var sphereSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Sphere,
				Color = Colors.Green,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			var tetrahedronSymbol = new SimpleMarkerSceneSymbol
			{
				Style = SimpleMarkerSceneSymbolStyle.Tetrahedron,
				Color = Colors.Lime,
				Width = 200,
				Height = 200,
				Depth = 200,
				AnchorPosition = SceneSymbolAnchorPosition.Center
			};

			symbols[0] = coneSymbol;
			symbols[1] = cubeSymbol;
			symbols[2] = cylinderSymbol;
			symbols[3] = diamondSymbol;
			symbols[4] = sphereSymbol;
			symbols[5] = tetrahedronSymbol;

			//create graphics for each symbol
			var i = 0;

			foreach (SimpleMarkerSceneSymbol symbol in symbols)
			{
				var point = new MapPoint(x + 0.01 * i, y, z, SpatialReferences.Wgs84);
				var graphic = new Graphic(point, symbol);
				graphicsOverlay.Graphics.Insert(i, graphic);
				i = i + 1;
			}

		}

	}
}
```

### 2. 3Dアプリ

#### iOS
<img src="https://github.com/ValueCreation/arcgis-xamarin-dev-doc/blob/master/hands-on/images/3D_2.png" height="500px">

#### ExtrudeGraphicsPage.xaml

```xml
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" 
	     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	     xmlns:esriUI="clr-namespace:Esri.ArcGISRuntime.Xamarin.Forms;assembly=Esri.ArcGISRuntime.Xamarin.Forms"
             xmlns:mapping="clr-namespace:Esri.ArcGISRuntime.Mapping;assembly=Esri.ArcGISRuntime" 				
	     xmlns:local="clr-namespace:ExtrudeGraphics" 
	     x:Class="ExtrudeGraphics.ExtrudeGraphicsPage">
	     
   <Grid>
     <esriUI:SceneView x:Name="MySceneView" />
   </Grid>

</ContentPage>
```

#### ExtrudeGraphicsPage.xaml.cs

```csharp
using Xamarin.Forms;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else
using Colors = System.Drawing.Color;
#endif

namespace ExtrudeGraphics
{
	public partial class ExtrudeGraphicsPage : ContentPage
	{
		public ExtrudeGraphicsPage()
		{
			InitializeComponent();

			Initialize();
		}

		private double squareSize = 0.01;
		private double spacing = 0.01;
		private int maxHeight = 10000;

		private GraphicsOverlay graphicsOverlay;

		private void Initialize()
		{

			var myScene = new Scene(Basemap.CreateTopographic());

			MySceneView.Scene = myScene;

			var camera = new Camera(28.4, 83, 20000, 10, 70, 300);
			MySceneView.SetViewpointCamera(camera);

			graphicsOverlay = new GraphicsOverlay();
			graphicsOverlay.SceneProperties.SurfacePlacement = SurfacePlacement.Draped;
			MySceneView.GraphicsOverlays.Add(graphicsOverlay);

			var renderer = new SimpleRenderer();
			var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Colors.White, 1);
			renderer.Symbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Solid, Colors.Blue, lineSymbol);
			renderer.SceneProperties.ExtrusionMode = ExtrusionMode.BaseHeight;
			renderer.SceneProperties.ExtrusionExpression = "[height]";
			graphicsOverlay.Renderer = renderer;

			var surface = new Surface();
			var url = new System.Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");

			var elevationSource = new ArcGISTiledElevationSource(url);
			surface.ElevationSources.Add(elevationSource);
			myScene.BaseSurface = surface;

			addGraphics();

		}

		private void addGraphics()
		{

			//var camera = new Camera(28.4, 83, 20000, 10, 70, 300)
			//var x = camera.Location.X - 0.01;
			//var y = camera.Location.Y- 0.25;
			var type = new ViewpointType();
			var extent = MySceneView.GetCurrentViewpoint(type).TargetGeometry.Extent;

			var x = extent.XMin - 0.01;
			var y = extent.YMax + 0.25;

			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 4; j++)
				{

					double valueX = x + i * (squareSize + spacing);
					double valueY = y + j * (squareSize + spacing);
					MapPoint mapPoint = new MapPoint(valueX, valueY);
					Geometry polygon = polygonForStartingPoint(mapPoint);
					addGraphicsForPolygon(polygon);

				}
			}

		}

		private Geometry polygonForStartingPoint(MapPoint mapPoint)
		{
			var polygon = new PolygonBuilder(SpatialReferences.Wgs84);

			polygon.AddPoint(mapPoint.X, mapPoint.Y);
			polygon.AddPoint(mapPoint.X, mapPoint.Y + squareSize);
			polygon.AddPoint(mapPoint.X + squareSize, mapPoint.Y + squareSize);
			polygon.AddPoint(mapPoint.X + squareSize, mapPoint.Y);

			return polygon.ToGeometry();

		}

		private void addGraphicsForPolygon(Geometry polygon)
		{
			var graphic = new Graphic(polygon, null, null);

			Random rand = new Random();
			var randv = rand.Next() % maxHeight;

			graphic.Attributes.Add("height", randv);

			graphicsOverlay.Graphics.Add(graphic);
		}
	}

}
```
