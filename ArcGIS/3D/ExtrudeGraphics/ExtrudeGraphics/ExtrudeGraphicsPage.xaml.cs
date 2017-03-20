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