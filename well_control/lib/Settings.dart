import 'package:flutter/material.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellOverview.dart';

class Settings extends StatefulWidget {
  Settings({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _SettingsState createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  static const wellOverview = "List of all wells";
  static const wellMap = "Map Overview";

  static const List<String> menuChoices = <String>[wellOverview, wellMap];

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
      body: Center(
        child: Column(
            mainAxisAlignment: MainAxisAlignment.center, children: <Widget>[]),
      ),
    );
  }

  void choiceAction(String choice) {
    if (choice == wellMap) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => WellMap(title: "Well Map")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "Well Overview")));
    }
  }
}
