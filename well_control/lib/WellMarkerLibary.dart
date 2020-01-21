library well_control.well_list;

import 'dart:convert';

import 'package:flutter_map/flutter_map.dart';
import 'package:well_control/WellMarker.dart';
import 'package:latlong/latlong.dart';
import 'package:flutter/material.dart';

import 'Functions.dart';

/// Well list
List<WellMarker> wells = <WellMarker>[];
List<Marker> markers = List<Marker>();

/// Returns all markers requested from DB and saves them in the global
/// well list.
Future<List<Marker>> getMarkers() {
  // request wells
  return getAllWells().then((response) {
    print(response.statusCode);
    Iterable result = json.decode(response.body);
    var resultList = result.toList();
    //print('Wells Result ' + resultList.toString());

    bool receivedCheck = false;

    for (var i = 0; i < resultList.length; i++) {

      for (int j = 0; j < wells.length; j++) {
        if (wells[j].wellId.compareTo(resultList[i]["id"]) == 0) {
          receivedCheck = true;
          break;
        } else {
          receivedCheck = false;
        }
      }
      if (!receivedCheck) {
        wells.add(WellMarker(
            resultList[i]["name"].toString(),
            resultList[i]["id"],
            resultList[i]["status"].toString(),
            resultList[i]["location"]["latitude"].toDouble(),
            resultList[i]["location"]["longitude"].toDouble()));
      }
    }

    //print("Wells Markers " + markers[0].toString());
    for (var i = 0; i < wells.length; i++) {
      markers.add(wells[i].marker);
    }
    print("Markers " + markers.toString());

    return markers;
  }).catchError((error) {
    print(error.toString());
  });
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
