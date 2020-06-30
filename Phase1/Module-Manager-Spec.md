**Goal:**
 - To display information about all modules found in a directory using its dll and xml files and be able to save an enable/disable for each module.

**Overview:**
 - All public members of a module will be displayed under the module name in a TreeView control. The user can select each member which will then display all information about that module and the selected member in a different field.
 - Each module will have its own enable/disable check box to indicate whether the module will be used or not. It will have a "Load Modules" button to allow the user to select the location of the dll's.
 - There will be a "Save Configuration" button which will save the state of each modules enable/disable check box. On application startup, if the config file exists, the previously saved settings will be loaded. The last selected directory will also be saved to all modules in that directory will be loaded and displayed.

**Breakdown:**
 - A "Load Modules" button will allow the user to select a directory where dll files are located. The program will check that the directory contains at lease one dll file.
 - The TreeView area will display each module (dll file) with its public members (constructors, properties, and methods) in child nodes. Each module will have an associated enable/Disable check box.
 - The user can select any of the public members from the TreeView area. The information about the selected member will be displayed in a separate field next to the TreeView area.
 - There will be a "Save Configuration" button that will store the loaded file location and the enable/disable status for each module. If this file exists on startup, the application will load with this saved data.

**Link to C. Will's Onboarding page:**
 - https://github.com/precisionvalve/NextGen/wiki/Onboarding-CWill