library well_control.well_list;

import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';

import 'package:well_control/WellMarker.dart';


List<WellMarker> wells = <WellMarker> [
  WellMarker("Mockup1" , "green" , 7.071891, 38.785878) ,
  WellMarker("Mockup2" , "yellow" , 7.084114, 38.783217)
];

List<Marker> getMarkers() {
  List<Marker> markers = new List(wells.length);
  for(var i = 0 ; i < wells.length ; i++) {
    markers[i] = wells[i].marker;
  }

  return markers;
}