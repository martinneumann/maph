import 'package:flutter/material.dart';
import 'package:latlong/latlong.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';

class WellMarker {

  static final Icon icon = Icon(FontAwesomeIcons.mapMarkerAlt);
  static final double iconSize = 45.0;
  Marker marker;
  Color markerColor = Color.fromARGB(255, 0, 255, 0);

  WellMarker(String color , double latitude , double longitude) {
    setColor(color);

    marker = Marker(
        point: LatLng(latitude, longitude),
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: (){},
                )
            )
    );
  }


  void setMarker(String color , double latitude , double longitude) {
    setColor(color);

    marker = Marker(
        point: LatLng(latitude, longitude),
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: (){},
                )
            )
    );
  }

  void setColor(String color) {
    switch(color) {
      case "red":
        markerColor = Color.fromARGB(255, 255, 0, 0);
        break;
      case "yellow":
        markerColor = Color.fromARGB(255, 255, 255, 0);
        break;
      case "green":
        markerColor = Color.fromARGB(255, 0, 255, 0);
        break;
      default:
        markerColor = Color.fromARGB(255, 0, 255, 0);
    }
  }
}