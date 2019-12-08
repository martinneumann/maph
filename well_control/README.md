# well_control

A new Flutter application.

## Getting Started

This project is a starting point for a Flutter application.

A few resources to get you started if this is your first Flutter project:

- [Lab: Write your first Flutter app](https://flutter.dev/docs/get-started/codelab)
- [Cookbook: Useful Flutter samples](https://flutter.dev/docs/cookbook)

For help getting started with Flutter, view our
[online documentation](https://flutter.dev/docs), which offers tutorials,
samples, guidance on mobile development, and a full API reference.

## Technical Documentation with dartdoc

The [dartdoc] tool is a part of the Flutter SDK.
It is located in the following directory.

```
$FLUTTER_ROOT/bin/cache/dart-sdk/bin/dartdoc (on macOS or Linux)
%FLUTTER_ROOT%\bin\cache\dart-sdk\bin\dartdoc.bat (on Windows)

FLUTTER_ROOT=PATH_TO_YOUR_FLUTTER_SDK
```


### generate documentation

If you run dartdoc you get the api documentation for Dart.
To generate the documentation for Flutter you have to activate dartdoc globally.

```
$FLUTTER_ROOT/bin/cache/dart-sdk/bin/pub global activate dartdoc (on macOS or Linux)
%FLUTTER_ROOT%\bin\cache\dart-sdk\bin\pub.bat global activate dartdoc (windows)
```

By default, the documentation is generated to the doc/api directory as static HTML files.

To generate the documentation run dartdoc from the root directory of the package.

```
$ cd well_control/
$FLUTTER_ROOT/bin/cache/dart-sdk/bin/dartdoc  --exclude 'dart:async,dart:collection,dart:convert,dart:core,dart:developer,dart:ffi,dart:html,dart:io,dart:isolate,dart:js,dart:js_util,dart:math,dart:typed_data,dart:ui'
```

To avoid that dartdoc generates documentation for the packages and the dart-sdk you have to
exclude the dart-sdk with this option **--exclude**

```
'dart:async,dart:collection,dart:convert,dart:core,dart:developer,dart:ffi,dart:html,dart:io,dart:isolate,dart:js,dart:js_util,dart:math,dart:typed_data,dart:ui'
```

This is a bug in flutter:
<https://github.com/dart-lang/dartdoc/issues/1949>

### View documentation
```
$FLUTTER_ROOT/bin/cache/dart-sdk/bin/pub global activate dhttpd
$FLUTTER_ROOT/bin/cache/dart-sdk/bin/pub global run dhttpddhttpd --path doc/api
```

Navigate to http://localhost:8080 in your browser.
Use this way to view the documentation for using the search function.

### Write documentation

Using a doc comment instead of a regular comment enables dartdoc
to find the comment and generate documentation for it.

```
// Single-line comment
/* Multi-line/In-line comment */
/// Dartdoc comment
```

In the comments you able to use markdown formatting.
A general [dartdoc convention] shuold be hold by writing documentation.

[dartdoc convention]: https://dart.dev/guides/language/effective-dart/documentation

[dartdoc]: https://pub.dev/packages/dartdoc



