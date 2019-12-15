import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellMap.dart';

class WellInfo extends StatefulWidget {
  WellInfo({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellInfoState createState() => _WellInfoState();
}

class _WellInfoState extends State<WellInfo> {
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
        body: Center(child: Container(child: DisplayWellsInfo())));
  }

  void choiceAction(String choice) {
    if (choice == wellMap) {
      Navigator.push(context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    } else if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else {
      Navigator.push(context,
          MaterialPageRoute(
              builder: (context) => AddWell(title: "Add new well")));
    }
  }
}

class DisplayWellsInfo extends StatefulWidget {
  @override
  DisplayWellsInfoState createState() => DisplayWellsInfoState();
}

class DisplayWellsInfoState extends State<DisplayWellsInfo> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(

        body: Center(
          child: Center(
          child: Column(
          children: [
            Text('More information about this well.\nIt is located in Kefole city.\nThe current status is: needs maintenance.'),
            Row(
              children: <Widget>[
//                FlatButton(onPressed: () => { this.choice }, child: const Text("Try these DIY fixes"),),
                FlatButton(onPressed: () => {},
                  child: const Text("Try these DIY fixes"),),

              ],
            ),
          ],
          ),
        ),

    ));
  }
}
