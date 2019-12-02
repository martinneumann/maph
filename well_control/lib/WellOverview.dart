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

  int _counter = 0;

  void _incrementCounter() {
    setState(() {
      _counter++;
    });
  }

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
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              'You have pushed the button this many times:',
            ),
            Text(
              '$_counter',
              style: Theme.of(context).textTheme.display1,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _incrementCounter,
        tooltip: 'Increment',
        child: Icon(Icons.add),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }

  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => WellMap(title: "Well Map")));
    } else {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    }
  }
}
