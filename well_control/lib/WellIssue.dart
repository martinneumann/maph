import 'package:well_control/Functions.dart';

class Location {
  int id;
  double longitude;
  double latitude;
}

class FundingInfo {
  int id;
  String organisation;
  String openingDate;
  double price;
}

class StatusHistory {
  int id;
  String description;
  bool works;
  bool confirmed;
  String statusChangedDate;
}

class Part {
  int id;
  String name;
  String description;
}

class WellType {
  int id;
  String name;
  String particularity;
  double depth;

}

/// Class that represents an issue. Each well can have multiple issues.
class WellIssue {
  int id;
  int wellId;
  String name;
  String description;
  String status;
  String creationDate;
  String image;
  bool open;
  bool works;
  List<Part> brokenParts;
  WellType wellType;
  String confirmedBy;
  String solvedDate;
  String repairedBy;
  String bill;

  WellIssue(int id, int wellId, String description, String creationDate,
      String status, bool open, bool works, String confirmedBy) {
    this.id = id;
    this.wellId = wellId;
    this.description = description;
    this.creationDate = creationDate;
    this.status = status;
    this.open = open;
    this.works = works;
    this.confirmedBy = confirmedBy;
  }

  WellIssue.detailed(this.brokenParts, this.confirmedBy, this.solvedDate,
      this.repairedBy, this.works);

  WellIssue.fromJson(Map<String, dynamic> json)
      : this.id = json['id'],
        this.description = json['description'],
        this.creationDate = json['creationDate'],
        this.status = json['status'],
        this.open = json['open'],
        this.confirmedBy = json['confirmedBy'],
        this.solvedDate = json['solvedDate'],
        this.repairedBy = json['repairedBy'],
        this.works = json['works'],
        this.brokenParts = json['brokenParts'];
}

