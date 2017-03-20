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

