# tping-gui
tping, or TrackPing, is a utility that tracks &amp; monitors ping times for analyzing latency or network issues

tping accepts an IP or Hostname, and the number of pings to be ran. It runs a ping every 2 seconds, and logs the results with detailed information, including the exact time the ping was ran. You can export the ping results to a CSV file for additional analysis.

tping was designed so you can leave it running for an extended period of time, and review the logs at a later time. When reviewing the logs, you are looking for groups of pings with high latency or groups of timeouts. You can run multiple instances of tping on an internet IP, your gateway, server, or any other network device to help you troubleshoot the exact time and device that is having trouble on your network. tping was built in Visual Studio 2013, and is designed to run with .NET Framework 4.5 installed.

Here is the executable: https://github.com/ingram1987/tping-gui/blob/master/Executable/tping-gui.exe?raw=true

Here is a screenshot:
![0ynNZ07](https://user-images.githubusercontent.com/4342930/156279128-5e7af98e-a3e4-43cd-8ae2-0bbc9e8bab27.png)
