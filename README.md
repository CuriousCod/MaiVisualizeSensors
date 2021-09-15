# MaiVisualizeSensors #
Draws maimai (SDEY) sensors on the screen based on the config values  
Used to debug sensor positions for MaiSense

Config works with the MaiSense fork: https://github.com/CuriousCod/MaiSense

## How To Use ## 

1. Edit sensor region values and window resolution in the sensor.config
2. Load the application
3. If the config is not loaded automatically, press F2 to load the config
4. Application will resize the window and draw the sensors upon loading the config

- Default configs are located in SensorConfigs folder
- Sensors can be drawn off-screen, if the width or height exceeds the monitor width
- The config resolution should match the maimai window resolution in order to position the sensors correctly
  - Only 1P is supported
  - If you have enabled 2P mode (1p_only = 0), you should halve the window width in the config

## Keyboard shortcuts ##

| Key  | Action |
| ------------- | ------------- |
| Escape  | Closes the application  |
| Spacebar  | Redraws all the sensors  |
| F1  | Toggle always on top  |
| F2  |  Load new config  |
| F5  | Reload current config  |
| F11  | Resize and reposition the window according to the config  |
| M  | Moves the canvas position to mouse cursor, useful if the sensors are drawn off-screen  |
| T  | Makes the window transparent  |
| Delete  | Clears the current sensors from the screen  |
| Number keys (1-8 and 0)  | Draws the matching sensor 1 -> sensor A1, 0 -> sensor C  |
| Number keys + Ctrl  | Draws the B sensor (B1-B8) instead of A  |

## Preview ##

<img src="https://www.dropbox.com/s/r1arn9e1da8swhw/Github_VisualizeSensors.png?raw=1" alt="preview" width="800" height="800"/>
