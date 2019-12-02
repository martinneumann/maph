import 'package:flutter/material.dart';
import 'package:well_control/WellMap.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Well Controll',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: WellMap(title: 'Well Map'),
    );
  }
}

