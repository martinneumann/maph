import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:latlong/latlong.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';

class WellMap extends StatefulWidget {
  WellMap({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}

class _WellMapState extends State<WellMap> {
  static const addWell = "Add Well";
  static const wellOverview = "List of Wells";
  static const settings = "Settings";
  static const report = "Report Malfunction";
  static const List<String> menuChoices = <String>[
    wellOverview,
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
            //width: 200,
            //height: 200,
            child: FlutterMap(
              options: MapOptions(
                center: LatLng(45.5231, -122.6765),
                zoom: 13.0,
              ),
              layers: [
                TileLayerOptions(
                  urlTemplate:
                  'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                  subdomains: ['a', 'b', 'c'],
                  additionalOptions: {
                    'access_token': '',
                    'id': ''
                  },
                ),
                MarkerLayerOptions (
                    markers: [
                      Marker(
                          //width: 80.0,
                          //height: 80.0,
                          point: LatLng(45.5231, -122.6765),
                          builder: (ctx) =>
                              Container(
                                  child: IconButton(
                                    icon: Icon(FontAwesomeIcons.mapMarkerAlt),
                                    color: Color.fromARGB(255, 250, 0, 0),
                                    iconSize: 45.0,
                                    onPressed: (){},
                                  )
                              )
                      )
                    ]
                ),
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
          MaterialPageRoute(
              builder: (context) => Settings(title: "Settings")));
    } else if(choice == wellOverview) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    }
    else if (choice == addWell) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => AddWell(title: "List of Wells")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => ReportWell(title: "Report Malfunction")));
    }
  }

  void addWellMarker(String status) {

  }
}
