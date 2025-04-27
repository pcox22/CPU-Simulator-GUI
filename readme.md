## CPU-Simulator GUI
A grahical user interface application with these components: CPU Simulator, QR code and Barcode Generator.


## Usage

```
Please install the font: "IDAutomationHC39M Free Version". You can find it in the project root folder.
```

## License
This project is licensed under the terms of the [MIT license](https://choosealicense.com/licenses/mit/).


# Modifications Begin Here

## CS3502_P2

### Description
A project to simulate 6 different CPU Scheduling Algorithms. This is a modified fork of Francis Nweke's CPU-Simulator GPU repository.

The solution is build in on Windows Visual Studio, utilizing Windows Forms App architecture and C#.

### Installation & Instructions
To install, open a terminal of choice and run the following to clone (replace end with the directory you wish to write to):
```bash
$ git clone https://github.com/pcox22/CPU-Simulator-GUI.git/your-local-repo
```

#### To run via CLI:
Navigate to the solution directory and run the following command on a terminal:
```bash
$ start CpuSchedulingWinForms.sln
```

Alternatively:
1. To run, open your IDE of choice (Microsoft Visual Studio 2022 Strongly Recommended)
2. Select Open a project or solution and select the CpuSchedulingWinForms.sln
3. VS should import everything as needed and successfully load files
4. Either select the start (green play button) in VS IDE, or press Ctrl+F5 to build the solution

7. If there are any errors when trying to run, ensure that .NET 8.0 is installed, if it was not installed along with the IDE. To do so:
   Consult: https://learn.microsoft.com/en-us/dotnet/core/install/windows


### Implementation
The implementation of the Algorithms I added was very simple. The program takes in a number of processes from the user and then initializes multiple arrays to that size. These arrays represent different attributes of the simulated processes, such as arrival times and burst times. A series of repetition and selection structures is then used to determine which processes are eligible for execution, and the process is carried out by incrementing the "completed time" while decrementing the "remaining time" of the current process.
