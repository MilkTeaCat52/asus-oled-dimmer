# G-OLED - Lightweight control for ASUS's flicker-free dimming for OLED laptop panels based on Seerge's G-Helper.
  
**⭐ If you like the app - consider checking out the [original app](https://github.com/seerge/g-helper) and making a donation to support its development!**

### Features:
- Lightweight and Quick-to-access System Tray App
- Run On Startup
- Always On Top
- Hide When Focus Lost


G-OLED is intended to be used alongside Armory Crate as a quick-to-access toggle for flicker-free dimming. G-OLED can be used with G-Helper, however in this case either app's slider will not update when the other is toggled. If you want to use G-OLED with G-Helper, it is recommended to [turn off the Visual Modes section in G-Helper](https://github.com/seerge/g-helper/wiki/Power-user-settings#visual-modes-section).


# Setup and Requirements
**How to Start**
- Download [latest release]((https://github.com/MilkTeaCat52/asus-oled-dimmer/releases/latest/download/GOLED.zip))
- Unzip to a folder of your choice (don't run exe from zip directly, as windows will put it into temp folder and delete after)
- Run GOLED.exe
- If you get a warning from Windows on launch (Windows Protected your PC), click More Info -> Run anyway.
- If you get dialog to Search for app in the Store, it's a bug of Windows Defender. Right click on GOLED.exe -> select Properties -> select Unblock checkbox

**Requirements (mandatory)**
- [Microsoft .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.14-windows-x64-installer)
------------------

### 🔖 Important Notice

G-Heler/G-OLED is **NOT** an operating system, firmware, or driver. It **DOES NOT** "run" your hardware in real-time anyhow. 

It's an app that lets you select one of the predefined operating modes created by manufacturer (and stored in BIOS) and optionally(!) set some settings that already exist on your device same as Armoury Crate can. It does it by using the Asus System Control Interface "driver" that Armoury uses for it.

If you use equivalent mode/settings as in Armoury Crate - the performance or the behavior of your device won't be different.

The role of G-Helper/G-OLED for your laptop is similar to the role of a remote control for your TV.

### Libraries and projects used
- [Linux Kernel](https://github.com/torvalds/linux/blob/master/include/linux/platform_data/x86/asus-wmi.h) for some basic endpoints in ASUS ACPI/WMI interface
- [AsusCtl](https://gitlab.com/asus-linux/asusctl) for inspiration and some reverse engineering

### Disclaimers
"Asus", "ROG", "TUF", and "Armoury Crate" are trademarked by and belong to AsusTek Computer, Inc. I make no claims to these or any assets belonging to AsusTek Computer and use them purely for informational purposes only.

THE SOFTWARE IS PROVIDED “AS IS” AND WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. MISUSE OF THIS SOFTWARE COULD CAUSE SYSTEM INSTABILITY OR MALFUNCTION.
