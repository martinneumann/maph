import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:location/location.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';

import 'WellMarkerLibary.dart' as wellList;

class WellMap extends StatefulWidget {
  WellMap({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}

class _WellMapState extends State<WellMap> {
  MapController mapController = new MapController();
  Location userLocation = new Location();

  static const addWell = "Add Well";
  static const settings = "Settings";
  static const report = "Report Malfunction";
  static const List<String> menuChoices = <String>[
    addWell,
    report,
    settings
  ];

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          title: Text(widget.title),
          actions: <Widget>[
            RaisedButton(
              onPressed: () {
                _getLocation().then((value) {
                  setState(() {
                    setUserLocation(value);
                  });
                });
              },
            ),
            PopupMenuButton<String>(
              onSelected: choiceAction,
              itemBuilder: (BuildContext context) {
                return menuChoices.map((String choice) {
                  return PopupMenuItem<String>(
                    value: choice,
                    child: Text(choice),
                  );
                }).toList();
              },
            )
          ],
        ),
        body: Center(
          child: Container(
            child: FlutterMap(
              mapController: mapController,
              options: MapOptions(
                center: LatLng(7.071891, 38.785878),
                zoom: 13.0,
              ),
              layers: [
                TileLayerOptions(
                  urlTemplate:
                  'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                  subdomains: ['a', 'b', 'c'],
                  additionalOptions: {'access_token': '', 'id': ''},
                ),
                MarkerLayerOptions(markers: wellList.getMarkers()),
              ],
            ),
          ),
        ),
      ),
    );
  }

  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else if (choice == addWell) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => AddWell(title: "Add Well")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => ReportWell(title: "Report Malfunction")));
    }
  }

  Future<Map<String, double>> _getLocation() async {
    var currentLocation = <String, double>{};
    try {
      currentLocation = await userLocation.getLocation();
    } catch (e) {
      currentLocation = null;
    }
    return currentLocation;
  }

  void setUserLocation(Map<String , double> userLocation) {
    if(userLocation == null) {
      mapController.move(LatLng(7.071891, 38.785878 ) , 14);
    }
    else {
      mapController.move(LatLng(userLocation['latitude'] , userLocation['longitude']), 14);
    }
  }
}
