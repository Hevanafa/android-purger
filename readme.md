# Android Purger

Used in React Native projects.  This tool will help clean drawable & res folders after bundling Android assets.

## Feature list

### Scan res

Scan res folder if there is any drawable or raw folders after bundling.

### Purge drawable & raw

Remove drawable & raw folders recursively (only inside `res` folder)

### Reset asset bundle

Use this only when you want to reset `index.android.bundle` -- useful to deflate the APK sometimes.

### Visit release folder

Open the release folder with Windows Explorer.  Use this after using `gradlew assembleRelease`.