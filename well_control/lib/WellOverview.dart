import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellInfo.dart';
import 'package:well_control/WellMap.dart';

import 'WellMarkerLibary.dart' as wellList;

/// Class provides overview of all existing wells.
///
/// View shows all well in a [DisplayWells] list.
class WellOverview extends StatefulWidget {
  WellOverview({Key key, this.title}) : super(key: key);

  /// Title of view.
  final String title;

  @override
  _WellOverviewState createState() => _WellOverviewState();
}

/// State provides view of [WellOverview].
class _WellOverviewState extends State<WellOverview> {

  /// Stores menu item title for settings.
  static const settings = "Settings";

  /// Stores menu item title for map.
  static const wellMap = "Map Overview";

  /// Stores menu item title for adding well.
  static const addWell = "Add Well";

  /// Stores menu item titles.
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

  /// Methods defines action of clicked menu item.
  ///
  /// Opens certain view by comparing clicked [choice] with menu list names.
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

/// Class provides view of well as list item.
class DisplayWells extends StatefulWidget {
  @override
  DisplayWellsState createState() => DisplayWellsState();
}

/// State create view of well item list.
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
                  color: wellList.wells[index].getMarkerColor(),
                  height: 20.0,
                  width: 20.0,
                ),
              ),
              title: Text(wellList.wells[index].getWellName()),
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
                            WellInfo(
                                title: wellList.wells[index].getWellName(),
                                well: wellList.wells[index])));
              },
            );
          },
          separatorBuilder: (context, index) {
            return Divider();
          }),
    );
  }
}
