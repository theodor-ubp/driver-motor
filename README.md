# DC Motor Control System - Arduino + C# GUI

This project is part of my engineering diploma work, focused on controlling a DC motor using an Arduino Uno and a Windows-based C# GUI. It demonstrates real-time serial communication between a desktop interface and a microcontroller to manage motor speed and direction.

---

## Features

- GUI developed in **C# (Windows Form)
- Communication via **Serial Port** (COM)
- Supports:
  - Motor **start/stop**
  - **Speed control** (PWM)
- Clean and portable UI executable

---

## Tech Stack

| Layer        | Tech Used           |
|--------------|---------------------|
| Microcontroller | Arduino UNO (ATmega328P) |
| Embedded Code   | Arduino C++ (PWM, Serial) |
| Desktop App     | C# (.NET WinForms / WPF) |
| Communication   | Serial Port (9600 baud)   |

---

## Getting Started

### Hardware Requirements

- Arduino UNO
- L298N Motor Driver Module (or H-bridge)
- 12V DC Motor
- External Power Supply
- USB cable for programming
- Breadboard + jumper wires (depending on project)

### Software Setup

1. Clone the repo:
   ```bash
   git clone https://github.com/theodor-ubp/driver-motor
