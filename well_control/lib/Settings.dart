import 'package:flutter/material.dart';
import 'package:well_control/PrivacyPolicy.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellOverview.dart';

class Settings extends StatefulWidget {
  Settings({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _SettingsState createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  static const wellOverview = "List of Wells";
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
        body: Center(child: Container(child: DisplaySettings())));
  }

  void choiceAction(String choice) {
    if (choice == wellMap) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    }
  }
}

class DisplaySettings extends StatefulWidget {
  @override
  DisplaySettingsState createState() => DisplaySettingsState();
}

class DisplaySettingsState extends State<DisplaySettings> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(body: _buildRow());
  }

  ListView _buildRow() {
    bool isSwitched = true;

    return ListView(
        children: ListTile.divideTiles(context: context, tiles: [
          ListTile(
              leading: Icon(Icons.volume_up),
              title: Text("Sound"),
              trailing: Switch(
                value: isSwitched,
                onChanged: (value) {
                  setState(() {
                    isSwitched = value;
                  });
                },
              )),
          ListTile(
              leading: Icon(Icons.vibration),
              title: Text("Vibration"),
              trailing: Switch(
                  value: isSwitched,
                  onChanged: (value) {
                    setState(() {
                      isSwitched = value;
                    });
                  })),
          ListTile(
            leading: Icon(Icons.developer_board),
            title: Text("Privacy Policy"),
            trailing: Icon(Icons.arrow_right),
            onTap: () {
              Navigator.push(
                  context,
                  MaterialPageRoute(
                      builder: (context) =>
                          PrivacyPolicy(title: "Privacy Policy")));
            },
          )
        ]).toList());
  }
}
