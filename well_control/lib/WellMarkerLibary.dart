library well_control.well_list;

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/WellMarker.dart';

import 'Functions.dart';
/// Defines name of marker, that shows user location on map.
final String userLocationMarkerName = "user";
/// Stores list of all existing wells as [WellMarker] object.
List<WellMarker> wells = <WellMarker>[];
/// Map stores wells as key value pair of name and marker.
/// This map is necessary to update UI of map.
Map<String,Marker> wellMarkersMap = Map<String,Marker>();

///Lists to store all current [wellTypeNames] and [wellTypeIds] from server.
List<String> wellTypeNames = List<String>();
List<int> wellTypeIds = List<int>();

bool loadTypeOnce = false;

/// Loads existing wells from external database.
///
/// Loads wells async because data comes from webservice-api.
Future<bool> loadWellList() async {
  return getAllWells().then((response) {
    print("Load all wells status is " + response.statusCode.toString());
    if(response.statusCode == 200) {
      wells.clear();
      Iterable result = json.decode(response.body);
      var resultList = result.toList();

      for (var i = 0; i < resultList.length; i++) {
        wells.add(WellMarker(
            resultList[i]["name"].toString(),
            resultList[i]["id"],
            resultList[i]["status"].toString(),
            resultList[i]["location"]["latitude"].toDouble(),
            resultList[i]["location"]["longitude"].toDouble()));
      }

      return true;
    }
    else {
      return false;
    }
  }).catchError((error) {
    print(error.toString());
    return false;
  });
}

/// Function loads well marker and updates well list.
///
/// Returns well markers as map to update this list on open street map.
Future<Map<String, Marker>> getMarkersMap() {
  return loadWellList().then((ready) {

    if(ready) {
      if(!loadTypeOnce) {
        loadAllWellTypes();
        loadTypeOnce = true;
      }

      wellMarkersMap.clear();

      for(int i = 0 ; i < wells.length ;  i++) {
        wellMarkersMap[wells[i].getWellName()] = wells[i].marker;
      }
    }

    return wellMarkersMap;
  });
}

/// Add user location marker to map marker.
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

  wellMarkersMap[userLocationMarkerName] = userMarker;
}

/// Function loads well marker and updates well list by radius search.
///
/// Gets well marker by given [latitude], [longitude] and radius in meter.
Future<Map<String, Marker>> getWellMarkersByRadius(double latitude,
    double longitude , int searchRadius) {

  var data = {};
  data["searchRadius"] = searchRadius;
  data["location"] = {};
  data["location"]["latitude"] = latitude;
  data["location"]["longitude"] = longitude;


  return getWellsByRadius(json.encode(data)).then((response) {
    if(response.statusCode == 200) {
      wellMarkersMap.clear();

      Iterable result = json.decode(response.body);
      var resultList = result.toList();

      for (var i = 0; i < resultList.length; i++) {
        for (int j = 0; j < wells.length; j++) {
          if (wells[j].wellId.compareTo(resultList[i]["id"]) == 0) {
            wellMarkersMap[resultList[i]["name"].toString()] = wells[i].marker;
            break;
          }
        }
      }

      print("WellRadius - found: " + wellMarkersMap.toString());
    }
    return wellMarkersMap;
  }).catchError((error) {
    print(error.toString());
  });
}

/// Loads well types from database.
///
/// Stores all existing well types in two lists [wellTypeIds]
/// and [wellTypeNames].
void loadAllWellTypes() {
  getAllWellTypes().then((response) {
    Iterable result = json.decode(response.body);
    var resultList = result.toList();

    for (int i = 0; i < resultList.length; i++) {
      wellTypeIds.add(resultList[i]["id"]);
      wellTypeNames.add(resultList[i]["name"]);
    }
  });
}