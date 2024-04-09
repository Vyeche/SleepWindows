# Windows Sleep
- A console app that lets the computer track mouse movements and sleeps the PC when no activity is present.

## Goal
- Have you ever wanted to take control of sleep when drivers or some Windows program are preventing sleep from being triggered?
- Try the Windows Sleep Console app

## Setup
- Compile with Visual Studio 22
- Use a task scheduler to run the executable.
- You can pass a parameter to specify the time before sleep is triggered in milliseconds
  - `WindowsSleep 10000`
  - Trigger sleep check every 10 seconds
