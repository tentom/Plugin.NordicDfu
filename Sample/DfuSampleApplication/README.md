# AndroidDfuSample 
The sample shows how to start the dfu service and set up notifications to let the user know the progress.

## Using the Library


### Permissions

#### Android
The forground service permission is needed for when the DFU service should run in the forground
``` 
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
```
We also need the Bluetooth_SCAN permission to scan for devices:
``` 
<uses-permission android:name="android.permission.BLUETOOTH_SCAN" android:usesPermissionFlags="neverForLocation" />
```

On older devices (android 11 or lower) theese permissions is needed:
``` 
<!-- Request legacy Bluetooth permissions on older devices. -->
    <uses-permission android:name="android.permission.BLUETOOTH"
                     android:maxSdkVersion="30" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN"
                     android:maxSdkVersion="30" />
```
And the maxSdkVersion is included so the manifest will not include them on higher api level.

More information on permissions needed for different DFU tasks can be found at the nordic DFU github repository [here](https://github.com/NordicSemiconductor/Android-DFU-Library/blob/main/lib/dfu/src/main/AndroidManifest.xml)
 and [here](https://github.com/NordicSemiconductor/Android-DFU-Library/blob/main/profile/main/src/main/AndroidManifest.xml) 

[Android Bluetooth documentation](https://developer.android.com/develop/connectivity/bluetooth/bt-permissions)

#### iOS


### Translations 

#### Android
To translate the text that apears in the notifications you can change the strings in the resources/values/strings.xml file.
[see this issue on the NordicSemiconductor github](https://github.com/NordicSemiconductor/Android-DFU-Library/issues/396)

If you just follow that comment you will get an error as described [here](https://github.com/NordicSemiconductor/Android-DFU-Library/issues/428#issuecomment-2125643579)

if you do you need to change 
```
<string name="dfu_status_uploading_part">Uploading part %d/%d&#8230;</string> 
```
to
``` 
<string name="dfu_status_uploading_part">Uploading part %1$d/%2$d&#8230;</string>
```

#### iOS 