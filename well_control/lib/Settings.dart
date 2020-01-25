import 'package:flutter/material.dart';
import 'package:well_control/PrivacyPolicy.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellOverview.dart';

import 'UserLibrary.dart' as users;

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
            body: new Container(
              margin: EdgeInsets.all(18.0),
              child: SingleChildScrollView(
                child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      Row(
                        children: <Widget>[
                          Container(
                            padding:
                            const EdgeInsets.only(left: 10.0, right: 15.0),
                            child: Icon(
                              Icons.volume_up,
                              color: Colors.grey,
                            ),
                          ),
                          Text('Sound'),
                          Spacer(),
                          Switch(
                            value: isSwitched,
                            onChanged: (value) {
                              setState(() {
                                isSwitched = value;
                              });
                            },
                          )
                        ],
                      ),
                      Divider(color: Colors.black87),
                      Row(children: <Widget>[
                        Container(
                          padding:
                          const EdgeInsets.only(left: 10.0, right: 15.0),
                          child: Icon(
                            Icons.vibration,
                            color: Colors.grey,
                          ),
                        ),
                        Text('Vibration'),
                        Spacer(),
                        Switch(
                          value: isSwitched,
                          onChanged: (value) {
                            setState(() {
                              isSwitched = value;
                            });
                          },
                        ),
                      ]),
                      Divider(color: Colors.black87),
                      Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: <Widget>[
                            Container(
                              padding: const EdgeInsets.only(
                                  left: 10.0, right: 15.0),
                              child: Icon(
                                Icons.developer_board,
                                color: Colors.grey,
                              ),
                            ),
                            Text('Privacy Policy'),
                            Spacer(),
                            Container(
                              padding: const EdgeInsets.only(
                                  left: 10.0, right: 15.0),
                              child: IconButton(
                                icon: Icon(Icons.arrow_forward),
                                color: Colors.grey,
                                onPressed: () {
                                  Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                          builder: (context) =>
                                              PrivacyPolicy(
                                                  title: "Privacy Policy")));
                                },
                              ),
                            ),
                          ]),
                      Divider(color: Colors.black87),
                      Container(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: <Widget>[
                              Column(
                                children: <Widget>[
                                  Row(
                                    children: <Widget>[
                                      Radio(
                                        value: 0,
                                        groupValue: radioValue,
                                        onChanged: _handleRadioChange,
                                      ),
                                      Text(
                                        'Admin',
                                      ),
                                    ],
                                  ),
                                  Column(
                                    children: <Widget>[
                                      Row(
                                        children: <Widget>[
                                          Radio(
                                            value: 1,
                                            groupValue: radioValue,
                                            onChanged: _handleRadioChange,
                                          ),
                                          Text(
                                            'Standard user',
                                          ),
                                        ],
                                      ),
                                    ],
                                  ),
                                  Column(
                                    children: <Widget>[
                                      Row(
                                        children: <Widget>[
                                          Radio(
                                            value: 2,
                                            groupValue: radioValue,
                                            onChanged: _handleRadioChange,
                                          ),
                                          Text(
                                            'Technician',
                                          ),
                                        ],
                                      ),
                                    ],
                                  ),
                                ],
                              )
                            ],
                          ))
                    ]),
              ),
            )));
  }

  int radioValue = 0;
  bool isSwitched = true;

  void _handleRadioChange(int val) {
    setState(() {
      radioValue = val;

      switch (radioValue) {
        case 0:
          users.admin = true;
          users.technician = false;
          users.basicUser = false;
          break;
        case 1:
          users.admin = false;
          users.technician = false;
          users.basicUser = true;
          break;
        case 2:
          users.admin = false;
          users.technician = true;
          users.basicUser = false;
          break;
      }
    });
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

/*




                      ]).toList()),

 */
