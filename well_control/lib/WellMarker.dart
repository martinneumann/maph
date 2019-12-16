import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/assets/water_icon.dart';

class WellMarker {
  String name;
  static final Icon icon = Icon(WaterIcon.water);
  static final double iconSize = 45.0;
  Marker marker;
  Color markerColor = Color.fromARGB(255, 0, 255, 0);

  WellMarker(String wellName, String color, double latitude, double longitude) {
    name = wellName;
    setColor(color);

    marker = Marker(
        point: LatLng(latitude, longitude),
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: () {},
                )
            )
    );
  }

  String getMarkerName() {
    return name;
  }

  Color getMarkerStatus() {
    return markerColor;
  }

  void setMarker(String color, double latitude, double longitude) {
    setColor(color);

    marker = Marker(
        point: LatLng(latitude, longitude),
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: () {},
                )
            )
    );
  }

  void setColor(String color) {
    switch (color) {
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
