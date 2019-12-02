import 'package:flutter/material.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellMap.dart';

class WellOverview extends StatefulWidget {
  WellOverview({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellOverviewState createState() => _WellOverviewState();
}

class _WellOverviewState extends State<WellOverview> {
  static const settings = "Settings";
  static const wellMap = "Map Overview";

  static const List<String> menuChoices = <String>[settings, wellMap];

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
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    } else {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
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
    return Scaffold(body: _buildRow());
  }

  ListView _buildRow() {
    return ListView(
      children: ListTile.divideTiles(context: context, tiles: [
        ListTile(
            leading: ClipOval(
              child: Container(
                color: Colors.red,
                height: 20.0, // height of the button
                width: 20.0, // width of the button))
              ),
            ),
            title: Text("Well Number 1"),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: <Widget>[
                Icon(Icons.arrow_right),
              ],
            )),
        ListTile(
            leading: ClipOval(
              child: Container(
                color: Colors.green,
                height: 20.0, // height of the button
                width: 20.0, // width of the button))
              ),
            ),
            title: Text("Well Number 1"),
            trailing: Row(mainAxisSize: MainAxisSize.min, children: <Widget>[
              Icon(Icons.arrow_right),
            ])),
      ]).toList(),
    );
  }
}


