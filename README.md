# LocoVR
This repository contains the Unity project used to conduct a user study on the impact of different locomotion methods on distance perception.

---

## 📥 Downloading the Project

```bash
git clone https://github.com/RazeenHussain/LocoVR.git
```
This project requires Git LFS.

Install Git LFS if you have not already:
```bash
git lfs install
```


## 🧩 Dependencies

- Unity Game Engine — 2022.3.4f1 or higher
- XR Interaction Toolkit — v2.4.3


## 🖥 Hardware Requirements

- HTC Vive Pro Eye headset
- 4 base station tracking configuration
- An empty room of at least 8m × 4.5m


## ▶️ Running the Project

- Open the Unity project.
- Load the Sample Scene.
- Select the Game Manager GameObject.
- In the Game Controller component, configure:
  - Locomotion method
  - User ID
  - Whether to record data
- Press Play in the Unity Editor.

| Action                       | Key |
| ---------------------------- | --- |
| Transition to Practice Scene | `p` |
| Transition to Task Scene     | `t` |


## 📁 Data Storage

All recorded experimental data is saved in:
```bash
StreamingAssets/
```
