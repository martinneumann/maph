import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/WellInfo.dart';
import 'package:well_control/assets/water_icon.dart';

class WellMarker {
  String name;
  static final Icon icon = Icon(WaterIcon.water);
  static final double iconSize = 45.0;
  int id;
  Marker marker;
  Color markerColor = Color.fromARGB(255, 0, 255, 0);
  LatLng location;
  String type;
  String status;
  String fundingOrganisation;
  double costs;


  WellMarker(String wellName, String color, double latitude, double longitude) {
    name = wellName;
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

  void setId(int wellId) {
    this.id = wellId;
  }

  void setType(String type) {
    this.type = type;
  }

  void setFundingOrganisation(String organisation) {
    this.fundingOrganisation = organisation;
  }

  void setWellCosts(int costs) {
    this.costs = costs.toDouble();
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
        status = "Not working";
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
