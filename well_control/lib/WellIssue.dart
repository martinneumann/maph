/// describes a well's parts
/// [id] is the specific part id, [name] is a name that can be non unique and [description]
/// describes the part in detail.
class Part {
  int id;
  String name;
  String description;
}

/// Class that represents an issue. Each well can have multiple issues.
/// [id] This issue's id.
/// [wellId] This issue's well.
/// [name] Name of this issue.
/// [description] Description of this issue.
/// [status] The current issue status. Default at creation is "Issue created". Can be
/// updated.
/// [creationDate] When this issue has been created.
/// [image] An image users can upload to describe the problem.
/// [open] Whether the issue is already solved (false) or still open (true).
/// [works] Whether the well is still operational (true) or not (false).
/// [brokenParts] A list of all the broken parts. Right now, only one broken part per issue
/// is allowed
/// [solvedDate] A date when the issue has been closed.
/// [repairedBy] The entity that closed this issue.
/// [bill] The financial information referring to this repair.
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
  String confirmedBy;
  String solvedDate;
  String repairedBy;
  String bill;

  /// Constructor for well issue.
  /// As of 26/01/2020, not all fields are set @todo add missing fields
  /// [id] id of the issue
  /// [wellId] the specific well
  /// [description] issue description
  /// [creationDate] when the issue was created
  /// [status] issue's status
  /// [open] whether the issue is still open
  /// [works] whether the well works or not
  /// [confirmedBy] who confirmed this issue to be true
  /// [brokenParts] List of the broken parts. Right now, only list with length of 1 is allowed.
  WellIssue(int id, int wellId, String description, String creationDate,
      String status, bool open, bool works, String confirmedBy, List<Part> brokenParts) {
    this.id = id;
    this.wellId = wellId;
    this.description = description;
    this.creationDate = creationDate;
    this.status = status;
    this.open = open;
    this.works = works;
    this.confirmedBy = confirmedBy;
    this.brokenParts = brokenParts;
  }
}

