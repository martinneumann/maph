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
  StatusHistory _statusHistory;
  DateTime creationDate;
  String image;
  bool open;
  bool works;
  List<Part> brokenParts;
  WellType wellType;
  String confirmedBy;
  FundingInfo fundingInfo;
  DateTime solvedDate;
  String repairedBy;
  String bill;

  WellIssue(this.id, this.wellId, this.description, this.creationDate,
      this.status, this.open);

  WellIssue.detailed(this.brokenParts, this.confirmedBy, this.solvedDate,
      this.repairedBy, this.works);


}