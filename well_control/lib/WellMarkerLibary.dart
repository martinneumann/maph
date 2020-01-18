library well_control.well_list;

import 'package:flutter_map/flutter_map.dart';
import 'package:well_control/WellMarker.dart';
import 'package:latlong/latlong.dart';
import 'package:flutter/material.dart';

List<WellMarker> wells = <WellMarker>[
  WellMarker("Mockup1", "green", 7.071891, 38.785878),
  WellMarker("Mockup2", "yellow", 7.084114, 38.783217)
];

List<Marker> markers = List<Marker>();

List<Marker> getMarkers() {
  for (var i = 0; i < wells.length; i++) {
    markers.add(wells[i].marker);
  }

  return markers;
}

void setUserPositionMarker(LatLng location) {
  Marker userMarker = Marker(
      point: location,
      builder: (ctx) =>
          Container(
              child: IconButton(
                icon: new Icon(Icons.gps_fixed),
                color: Color.fromARGB(255, 0, 0, 0),
                iconSize: 45.0,
                onPressed: () {},
              )
          )
  );

  markers.add(userMarker);
}
