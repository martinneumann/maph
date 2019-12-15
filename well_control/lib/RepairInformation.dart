import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'Settings.dart';
import 'WellMap.dart';
import 'WellOverview.dart';

///
/// Class that represents a single repair information step in a DIY action.
///
class RepairInformationStep {
  var title = "";
  var subtitle = "";
  var image = "";
  var status = false;
}

///
/// Repair Information widget.
///
class RepairInformation extends StatefulWidget {
  RepairInformation({Key key, this.title}) : super(key: key);

  final String title;

  var wellId = ""; // the well Id this information refers to

  @override
  _RepairInformationState createState() => _RepairInformationState();
}

/// Repair information stateful widget.
class _RepairInformationState extends State<RepairInformation> {
  static const wellOverview = "List of Wells";
  static const wellMap = "Map Overview";
  static const settings = "Settings";

  static const List<String> menuChoices = <String>[
    wellOverview,
    wellMap,
    settings
  ];

  @override
  Widget build(BuildContext context) {
    Image(image: AssetImage('assets/repair-info/pressure-switch.jpg'));
    Image(image: AssetImage('assets/repair-info/replace-pressure-switch.jpg'));
    Image(
        image: AssetImage('assets/repair-info/quick-fix-pressure-switch.jpg'));
    return MaterialApp(
        home: Scaffold(
          resizeToAvoidBottomPadding: true,
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
          body: SingleChildScrollView(
              child: Center(
                  child: Column(
                    children: <Widget>[
                      Card(
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: <Widget>[
                            const ListTile(
                              leading: Icon(Icons.album),
                              title: Text('Step 1: Check the pressure switch.'),
                              subtitle: Text(
                                  'You’ll find the pressure switch mounted on a 1/4-in. tube near the pressure tank. '
                                      'It’s what senses when water pressure has dropped to the point where the pressure tank requires more water. '
                                      'The switch then powers up the well pump.'),
                            ),
                            Image(
                                image:
                                AssetImage(
                                    'assets/repair-info/pressure-switch.jpg')),
                            ButtonBar(
                              children: <Widget>[
                                FlatButton(
                                  child: const Text('Step 1 done!'),
                                  onPressed: () {
                                    /* ... */
                                  },
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                      Card(
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: <Widget>[
                            const ListTile(
                              leading: Icon(Icons.album),
                              title: Text(
                                  'Step 2: If the switch is bad, replace it.'),
                              subtitle: Text(
                                  'If you find the pressure switch is bad, test the pressure tank '
                                      'to make sure it isn’t waterlogged (see ‘Problem: Pulsing Water’). To replace the switch, start by '
                                      'removing the wires to the old switch (be sure to label them) and unscrew the switch. Coat the tubing threads with pipe dope or Teflon '
                                      'tape and screw on the new switch so it sits in the same orientation. Then reconnect the wires.'),
                            ),
                            Image(
                                image: AssetImage(
                                    'assets/repair-info/replace-pressure-switch.jpg')),
                            ButtonBar(
                              children: <Widget>[
                                FlatButton(
                                  child: const Text('Step 2 done!'),
                                  onPressed: () {
                                    /* ... */
                                  },
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                      Card(
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: <Widget>[
                            const ListTile(
                              leading: Icon(Icons.album),
                              title: Text(
                                  'If you can\'t replace it: Temporary quick fix'),
                              subtitle: Text(
                                  'If banging on the tube under the pressure switch kicked on the well pump, it'
                                      ' means the contact surfaces of the electrical contacts are pitted or burned, causing a poor connection. You can '
                                      'temporarily restore the surfaces to keep it going until you can buy a replacement switch.'
                                      'First, turn off the power and double-check with a voltage tester. Pull the contacts open and file'
                                      ' off the burned and pitted areas using an ordinary nail file or emery board. Replace the'
                                      ' pressure switch as soon as possible because this fix won’t last long.'),
                            ),
                            Image(
                                image: AssetImage(
                                    'assets/repair-info/quick-fix-pressure-switch.jpg')),
                            ButtonBar(
                              children: <Widget>[
                                FlatButton(
                                  child: const Text('Temporary fix done!'),
                                  onPressed: () {
                                    Navigator.pop(context);
                                  },
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    ],
                  ))),
        ));
  }

  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else if (choice == wellOverview) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    } else {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => WellMap(title: "Well Map")));
    }
  }
}
