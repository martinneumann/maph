import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellInfo.dart';

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
    Color color = Theme.of(context).primaryColor;

    Widget buttonSection = Container(
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          _buildButtonColumn(color, Icons.call, 'CALL'),
          _buildButtonColumn(color, Icons.near_me, 'ROUTE'),
          _buildButtonColumn(color, Icons.report, 'REPORT'),
        ],
      ),
    );

    return Scaffold(

        body: Column(
          children: [Image.asset(
            'images/well_number2.jpg',
            width: 600,
            height: 240,
           //fit: BoxFit.fitHeight,
          ),
            buttonSection,
          ],
        ),

    );


  }
}


Column _buildButtonColumn(Color color, IconData icon, String label) {
  return Column(
    mainAxisSize: MainAxisSize.min,
    mainAxisAlignment: MainAxisAlignment.center,
    children: [
      Icon(icon, color: color),
      Container(
        margin: const EdgeInsets.only(top: 8),
        child: Text(
          label,
          style: TextStyle(
            fontSize: 12,
            fontWeight: FontWeight.w400,
            color: color,
          ),
        ),
      ),
    ],
  );
}
