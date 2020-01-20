import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/WellInfo.dart';
import 'package:well_control/assets/water_icon.dart';

class WellMarker {
  int wellId;
  String name;
  static final Icon icon = Icon(WaterIcon.water);
  static final double iconSize = 45.0;
  Marker marker;
  Color markerColor = Color.fromARGB(255, 0, 255, 0);
  LatLng location;
  String type;
  String status;
  String fundingOrganisation;
  String costs;


  WellMarker(String wellName, int wellId, String color, double latitude, double longitude) {
    this.name = wellName;
    this.wellId = wellId;
    location = LatLng(latitude, longitude);
    setColor(color);

    marker = Marker(
        point: location,
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: () {
                    Navigator.push(
                        ctx,
                        MaterialPageRoute(
                            builder: (context) =>
                                WellInfo(
                                    title: name , well: this)));
                  },
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

  void setType(String type) {
    this.type = type;
  }

  void setFundingOrganisation(String organisation) {
    this.fundingOrganisation = organisation;
  }

  void setWellCosts(String costs) {
    this.costs = costs;
  }

  void setMarker(String color, double latitude, double longitude) {
    setColor(color);
    location = LatLng(latitude, longitude);

    marker = Marker(
        point: location,
        builder: (ctx) =>
            Container(
                child: IconButton(
                  icon: icon,
                  color: markerColor,
                  iconSize: iconSize,
                  onPressed: () {
                    Navigator.push(
                        ctx,
                        MaterialPageRoute(
                            builder: (context) =>
                                WellInfo(
                                    title: name , well: this)));
                  },
                )
            )
    );
  }

  void setColor(String color) {
    switch (color) {
      case "red":
        markerColor = Color.fromARGB(255, 255, 0, 0);
        status = "Not Working";
        break;
      case "yellow":
        markerColor = Color.fromARGB(255, 255, 255, 0);
        status = "Maintenance";
        break;
      case "green":
        markerColor = Color.fromARGB(255, 0, 255, 0);
        status = "Working";
        break;
      default:
        markerColor = Color.fromARGB(255, 0, 255, 0);
        status = "Working";
    }
  }
}
