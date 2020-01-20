import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';

import 'WellMarkerLibary.dart' as wellList;
import 'WellIssueLibrary.dart' as issueList;

class WellMap extends StatefulWidget {
  WellMap({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}

class _WellMapState extends State<WellMap> {
  static const mapMarkers = null;
  static const listofWells = "List of wells";
  static const addWell = "Add Well";
  static const settings = "Settings";
  static const report = "Report Malfunction";
  static const List<String> menuChoices = <String>[
    listofWells,
    addWell,
    report,
    settings
  ];

  @override
  void initState() {
    super.initState();
  }

  Future<List<Marker>> _markerList = wellList.getMarkers();


  @override
  Widget build(BuildContext context) {

    return FutureBuilder<List<Marker>>(
      future:_markerList,
      builder: (BuildContext context, AsyncSnapshot<List<Marker>> snapshot) {
        List<Widget> children;
        var list = snapshot.data;

        if (snapshot.hasData) {
          children = <Widget>[
            Icon(
              Icons.check_circle_outline,
              color: Colors.green,
              size: 60,
            ),
            Padding(
              padding: const EdgeInsets.only(top: 16),
              child: Text('Result: ${snapshot.data}'),
            )
          ];
        } else if (snapshot.hasError) {
          children = <Widget>[
            Icon(
              Icons.error_outline,
              color: Colors.red,
              size: 60,
            ),
            Padding(
              padding: const EdgeInsets.only(top: 16),
              child: Text('Error: ${snapshot.error}'),
            )
          ];
        } else {
          children = <Widget>[
            SizedBox(
              child: CircularProgressIndicator(),
              width: 60,
              height: 60,
            ),
            const Padding(
              padding: EdgeInsets.only(top: 16),
              child: Text('Awaiting result...'),
            )
          ];
        }
      print("Well list: " + list.toString());
      var mapMarkers = list;
      if (list != null) {
        return MaterialApp(
          home: Scaffold(
            appBar: AppBar(
              title: Text(widget.title),
              actions: <Widget>[
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
                  options: MapOptions(
                    center: LatLng(6.071891, 38.785878),
                    zoom: 12.0,
                  ),
                  layers: [
                    TileLayerOptions(
                      urlTemplate:
                      'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                      subdomains: ['a', 'b', 'c'],
                      additionalOptions: {'access_token': '', 'id': ''},
                    ),
                    MarkerLayerOptions(markers: mapMarkers),
                  ],
                ),
              ),
            ),
          ),
        );
      } else {
        return MaterialApp(
          home: Scaffold(
            appBar: AppBar(
              title: Text(widget.title),
              actions: <Widget>[
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
                  options: MapOptions(
                    center: LatLng(6.071891, 38.785878),
                    zoom: 12.0,
                  ),
                  layers: [
                    TileLayerOptions(
                      urlTemplate:
                      'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                      subdomains: ['a', 'b', 'c'],
                      additionalOptions: {'access_token': '', 'id': ''},
                    ),
                  ],
                ),
              ),
            ),
          ),
        );

      }
    });
  }

    void choiceAction(String choice) {
      if (choice == settings) {
        Navigator.push(context,
            MaterialPageRoute(
                builder: (context) => Settings(title: "Settings")));
      } else if (choice == addWell) {
        Navigator.push(context,
            MaterialPageRoute(
                builder: (context) => AddWell(title: "Add Well")));
      } else if (choice == listofWells) {
        Navigator.push(context,
            MaterialPageRoute(
                builder: (context) => WellOverview(title: "List of Wells")));
      } else {
        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (context) => ReportWell(title: "Report Malfunction")));
      }
      }
    }

