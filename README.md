# 🥽 VR Object Sorting Study
### Does Visual Realism in VR Help or Hinder?

A Unity-based VR experiment investigating whether visual realism affects object recognition and categorisation performance in a simple sorting task. Built for Meta Quest 3 as part of a graduate thesis at Adelphi University.

---

## Overview

Participants sort everyday objects into labelled category bins (Food, Kitchen, Tools) in VR. The study compares **realistic high-fidelity models** against **low-poly simplified versions** of the same objects, measuring sorting accuracy and reaction time per object.

**Research Hypothesis:** Visual realism in VR may either improve recognition and categorisation speed, or increase cognitive load and hinder performance compared to low-poly models.

---

## Built With

| Tool | Purpose |
|------|---------|
| Unity 6 (6000.4.0f1) | Game engine |
| XR Interaction Toolkit + OpenXR | VR input and interaction |
| Meta Quest 3 | Target hardware |
| C# | In-app data logging |
| Python + Pandas | Data analysis |
| Qualtrics XM | Post-task survey |

---

## Features

- Automated CSV data logging per object placement
- Timestamped session files per participant
- Warm-up scene → main experiment scene transition
- Participant ID system via ADB file push
- Two conditions: Realistic and Low-Poly object models
- Post-task survey via Qualtrics XM

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── TrialManager.cs       # Central coordinator
│   ├── TrialTimer.cs         # Session timing
│   ├── BinDetector.cs        # Placement detection
│   ├── SortableObject.cs     # Object data component
│   ├── ObjectSpawner.cs      # Object queue manager
│   └── DataLogger.cs         # CSV writer
├── Scenes/
│   ├── Warm UP               # Scene 0 — practice
│   └── Environment           # Scene 1 — main experiment
└── Prefabs/
    └── [Object prefabs]      # Realistic + Low-Poly pairs
```

---

## Setup

1. Clone the repository
2. Open in Unity 6
3. Connect Meta Quest 3 via USB
4. File → Build Settings → confirm Warm UP is Scene 0, Environment is Scene 1
5. Build and Run

**Required Player Settings:**
- Scripting Backend: IL2CPP
- Target Architecture: ARM64
- Minimum API Level: Android 10 (API 29)
- Package Name: `com.shaniabrown.ResearchVR`

---

## Running the App
 
### First Time Setup
 
1. Enable Developer Mode on Meta Quest 3
   - Open the Meta Quest mobile app
   - Go to Menu → Devices → select your headset
   - Enable Developer Mode
2. Connect headset to your computer via USB
3. Put on the headset and select **Allow** when prompted for USB debugging
### Installing the APK
 
```bash
# Install via ADB
adb install path/to/ResearchVR.apk
```
 
Or use **Build and Run** directly from Unity which installs automatically.
 
### Launching the App
 
1. On the headset go to **App Library**
2. Filter by **Unknown Sources**
3. Select **ResearchVR**
---
## Participant Session Workflow

```bash
# 1. Push participant ID before each session
echo -n "P01" > participant.txt
adb push participant.txt /sdcard/Android/data/com.shaniabrown.ResearchVR/files/participant.txt

# 2. Launch app on headset
# Unknown Sources → ResearchVR

# 3. Pull data after session
adb pull /sdcard/Android/data/com.shaniabrown.ResearchVR/files/ ./Data
```

**Participant ID naming convention:**
- `P01`, `P02`, `P03` — real participants
- `TEST` or `DEMO` — test runs

---

## Data

CSV output per session:

```
ParticipantID, ObjectName, BinPlaced, Correct, ObjectTime, SessionTime, Realism
```

Saved to:
```
/sdcard/Android/data/com.shaniabrown.ResearchVR/files/trial_data_[timestamp].csv
```

Each session produces a uniquely timestamped file — no overwriting across participants.

---

## Findings

Pilot study with n=4 participants. Results indicated a **speed-accuracy trade-off** between conditions:

| Condition | Mean Accuracy | Mean Object Time |
|-----------|--------------|-----------------|
| Realistic | 66.7% | 5.59s |
| Low-Poly | 25.0% | 3.22s |

Realistic models were sorted more accurately but took longer. Low-poly models were sorted faster but with considerably more errors. Findings are exploratory and intended to inform a full-scale study.

---

## Photos

### Low-Poly vs Realistic Model Comparison
<img width="443" height="231" alt="Screenshot 2026-05-18 at 3 54 51 PM" src="https://github.com/user-attachments/assets/bf82f786-ce9f-4294-bfb6-65b8cbb80d81" />

### Labeled Sorting Bins
<img width="358" height="116" alt="Screenshot 2026-05-18 at 5 48 13 PM" src="https://github.com/user-attachments/assets/e65bde1a-4182-4112-baf7-9720b76dd432" />


### Participant Sorting in VR
<img width="1235" height="632" alt="Screenshot 2026-05-17 at 4 08 51 PM" src="https://github.com/user-attachments/assets/a270c23b-0134-472d-848a-915dfd83e87b" />

---

## Known Limitations

- Small sample size (n=4) — pilot study only
- Realistic and low-poly models more visually similar than intended
- No counterbalancing of object presentation order
- Verbal instructions rather than in-app training scene
- Object set reduced to 4 matched pairs due to development complications

---

## Future Work

- Larger participant sample with counterbalanced object order
- More visually distinct custom-modelled assets
- In-app training scene and post-task questionnaire
- NASA-TLX for standardised cognitive load measurement
- Randomised object presentation order

---

## Author

**Shania Brown**
Mathematics and Computer Science Department
Adelphi University, Garden City, New York


