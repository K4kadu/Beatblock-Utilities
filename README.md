# About Beatblock Utilities
Beatblock Utilities is a collection of external Windows Forms apps to help with Beatblock custom map creation.
<br /> Upon running an app you might get a popup like "Windows protected your PC" which is not triggered by the file containing possibly malicious code, but instead it does that for all programs that aren't specifically registered at Microsoft. To still be able to use the tool, just click on "More info" and then "Run anyway".

# How to install
## If you don't want to install anything:
Download the .exe files larger than 100MB.
<br /> -> They already come with all dependencies that are needed for them to run.

## For smaller file sizes:
1. Download the smaller .exe files and try to run them to see if you maybe already have the framework installed.
2. If you don't, Windows should automatically suggest a download link for the correct version.
3. With the framework installed properly, the apps should run on at least all Windows computers.
<br /> (If not, consider posting it on the issues page or DMing me on Discord @k4kadu ).

# List of tools
## Text to Image
Generates a strictly two colored (black & transparent) .png from any text you input. The default font and size are DigitalDisco-Thin at size 12, which is the settings Beatblock uses. Alternatively you can choose any other font installed on your computer.
![Text to Image demonstration](https://github.com/user-attachments/assets/47b81fa1-3b95-4cd3-ba8b-babf15f7d238)

## Event Randomizer
Takes an event string from a Beatblock .json as input and lets you randomize one of the values in it + optionally repeat the process for different timings.
<br /><br /> Let's say for example that you want to place mines at a random angle from 45° to 135°, every 1/8 beats from time 0 to 10. You would then place a mine ingame **and save**, go to your chart file and copy something like {"type":"mine","time":1,"angle":90} over to the Event Randomizer and then all that is left is for you to set your other parameters, based on what numbers you want as possible outputs. In this case that would be:
- what you want to randomize: angle
- minimum: 45 <t/> maximum: 135
- repeat in time for: "every 1/8 beats" -> 0.125
- from beat 0 to 10
![Event Randomizer demonstration](https://github.com/user-attachments/assets/d5741455-5a4c-40f9-8fa7-c2ae9694ebce)
