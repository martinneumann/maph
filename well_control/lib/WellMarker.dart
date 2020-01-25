import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/WellInfo.dart';
/// Class to define wells as object including Marker object for map.
class WellMarker {
  /// Stores id of well.
  int wellId;
  /// Stores name of well.
  String name;
  /// Defines icon of marker on map.
  static final Icon icon = new Icon(Icons.local_drink);
  /// Defines size of icon.
  static final double iconSize = 45.0;
  /// Stores marker object to show the well on map.
  Marker marker;
  /// Stores color of marker. Default value is green.
  Color markerColor = Color.fromARGB(255, 0, 255, 0);
  /// Stores location as [LatLng] object, including longitude and latitude.
  LatLng location;
  /// Stores type of well.
  String type;
  /// Stores status of well.
  String status;
  /// Stores organisation, that donates the well.
  String fundingOrganisation;

  /// Stores building costs of the well.
  String costs;

  ///Stores all parts of a well.
  Map<String, int> wellParts = Map<String, int>();


  /// Constructor initializes minimum of variables to define it by [wellName],
  /// [wellId], [color], which defines the status on map, and the location
  /// data by [latitude] and [longitude].
  ///
  /// Also all definitions of map marker is initialized by constructor.
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
  /// Method returns name of well.
  String getWellName() {
    return name;
  }
  /// Method returns color of well marker.
  Color getMarkerColor() {
    return markerColor;
  }
  /// Method sets [type] of well.
  void setType(String type) {
    this.type = type;
  }
  /// Method sets funding [organisation] of well.
  void setFundingOrganisation(String organisation) {
    this.fundingOrganisation = organisation;
  }
  /// Method sets [costs] of well.
  void setWellCosts(String costs) {
    this.costs = costs;
  }
  /// Method redesign the well marker.
  ///
  /// It recreate marker with new [color] and
  /// new position by [latitude] and [longitude].
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
  /// Method sets color and status by given [color] name.
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


