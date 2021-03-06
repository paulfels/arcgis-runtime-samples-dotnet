// Copyright 2017 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
// language governing permissions and limitations under the License.

using System;
using System.Drawing;
using CoreGraphics;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Foundation;
using UIKit;

namespace ArcGISRuntime.Samples.SimpleRenderers
{
    [Register("SimpleRenderers")]
    [ArcGISRuntime.Samples.Shared.Attributes.Sample(
        "Simple renderer",
        "Symbology",
        "This sample demonstrates how to create a simple renderer and add it to a graphics overlay. Renderers define the symbology for all graphics in a graphics overlay (unless they are overridden by setting the symbol directly on the graphic). Simple renderers can also be defined on feature layers using the same code.",
        "")]
    public class SimpleRenderers : UIViewController
    {
        // Create and hold a reference to the used MapView.
        private readonly MapView _myMapView = new MapView();

        public SimpleRenderers()
        {
            Title = "Simple renderer";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create the UI, setup the control references and execute initialization.
            CreateLayout();
            Initialize();
        }

        public override void ViewDidLayoutSubviews()
        {
            try
            {
                nfloat topMargin = NavigationController.NavigationBar.Frame.Height + UIApplication.SharedApplication.StatusBarFrame.Height;

                // Reposition controls.
                _myMapView.Frame = new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height);
                _myMapView.ViewInsets = new UIEdgeInsets(topMargin, 0, 0, 0);

                base.ViewDidLayoutSubviews();
            }
            // Needed to prevent crash when NavigationController is null. This happens sometimes when switching between samples.
            catch (NullReferenceException)
            {
            }
        }

        private async void Initialize()
        {
            // Create new map with labeled imagery basemap.
            Map myMap = new Map(Basemap.CreateImageryWithLabels());

            // Create several map points using the WGS84 coordinates (latitude and longitude).
            MapPoint oldFaithfulPoint = new MapPoint(-110.828140, 44.460458, SpatialReferences.Wgs84);
            MapPoint cascadeGeyserPoint = new MapPoint(-110.829004, 44.462438, SpatialReferences.Wgs84);
            MapPoint plumeGeyserPoint = new MapPoint(-110.829381, 44.462735, SpatialReferences.Wgs84);

            // Use the two points farthest apart to create an envelope.
            Envelope initialEnvelope = new Envelope(oldFaithfulPoint, plumeGeyserPoint);

            // Create a graphics overlay.
            GraphicsOverlay myGraphicOverlay = new GraphicsOverlay();

            // Create graphics based upon the map points.
            Graphic oldFaithfulGraphic = new Graphic(oldFaithfulPoint);
            Graphic cascadeGeyserGraphic = new Graphic(cascadeGeyserPoint);
            Graphic plumeGeyserGraphic = new Graphic(plumeGeyserPoint);

            // Add the graphics to the graphics overlay.
            myGraphicOverlay.Graphics.Add(oldFaithfulGraphic);
            myGraphicOverlay.Graphics.Add(cascadeGeyserGraphic);
            myGraphicOverlay.Graphics.Add(plumeGeyserGraphic);

            // Create a simple marker symbol - red, cross, size 12.
            SimpleMarkerSymbol mySymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Cross, Color.Red, 12);

            // Create a simple renderer based on the marker symbol. It will be applied to all graphics in the overlay.
            myGraphicOverlay.Renderer = new SimpleRenderer(mySymbol);

            // Add the graphics overlay to the map view.
            _myMapView.GraphicsOverlays.Add(myGraphicOverlay);

            // Add the map to the map view.
            _myMapView.Map = myMap;

            // Set the viewpoint to the envelope with padding.
            await _myMapView.SetViewpointGeometryAsync(initialEnvelope, 50);
        }

        private void CreateLayout()
        {
            // Add the mapview to the view.
            View.AddSubviews(_myMapView);
        }
    }
}