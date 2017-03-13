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


### 手順1:プロジェクトの作成


### 手順2:ArcGIS Runtime SDK NuGet パッケージのインストール


### 手順3:WebMapの表示


##### アプリの実行


### 手順4:機能追加

ここまで作成したアプリについて以下の機能を追加してみましょう。

#### 1. ジオコーディング
店舗情報や顧客情報などに含まれる住所情報を XY 座標に変換し地図上にマッピングすることができます。

##### アプリの実行


#### 2. 空間解析
重複するエリアの抽出や空間的な分布傾向の把握など、多様な空間解析機能を利用することができます。

##### アプリの実行


##　宿題
いったん本ハンズオンではここで終了ですが、お時間のある方は 3D の地図表現に挑戦してみましょう。 


### 1.宿題


### 2.宿題


