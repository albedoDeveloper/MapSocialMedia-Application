using System;
using System.Collections.Generic;
using System.Text;
using Mapsui;

namespace Social_Media_Map_Application
{
    class Map
    {

       private Mapsui.Map map = new Mapsui.Map
        {
            CRS = "EPSG:3857"
        };

        public Mapsui.UI.Forms.MapView mapView { get; set; }

        public Map(Mapsui.UI.Forms.MapView mapView)
        {
            this.mapView = mapView;
            
            var tileLayer = Mapsui.Utilities.OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);

            mapView.Map = map;
            mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(31.9523, 115.8613), false);

        }


    }
}
