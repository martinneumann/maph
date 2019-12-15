import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellInfo.dart';
import 'package:well_control/WellMap.dart';

import 'WellMarkerLibary.dart' as wellList;

class WellOverview extends StatefulWidget {
  WellOverview({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellOverviewState createState() => _WellOverviewState();
}

class _WellOverviewState extends State<WellOverview> {
  static const settings = "Settings";
  static const wellMap = "Map Overview";
  static const addWell = "Add Well";

  static const List<String> menuChoices = <String>[settings, wellMap, addWell];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
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
        body: Center(child: Container(child: DisplayWells())));
  }

  void choiceAction(String choice) {
    if (choice == wellMap) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    } else if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => AddWell(title: "Add new well")));
    }
  }
}

class DisplayWells extends StatefulWidget {
  @override
  DisplayWellsState createState() => DisplayWellsState();
}

class DisplayWellsState extends State<DisplayWells> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ListView.separated(
          itemCount: wellList.wells.length,
          itemBuilder: (BuildContext context, int index) {
            return new ListTile(
              leading: ClipOval(
                child: Container(
                  color: wellList.wells[index].getMarkerStatus(),
                  height: 20.0,
                  width: 20.0,
                ),
              ),
              title: Text(wellList.wells[index].getMarkerName()),
              trailing: Row(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  Icon(Icons.arrow_right),
                ],
              ),
            onTap: () {
              Navigator.push(
                  context,
                  MaterialPageRoute(
                      builder: (context) =>
                          WellInfo(title: "Further Information")));
            },
            );
          },
          separatorBuilder: (context, index) {
            return Divider();
          }),
    );
  }
}
