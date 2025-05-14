# Powerscan
Powerscan is a WPF app to manage electricity meters. Add entries with full address data, simulate live consumption, calculate energy costs, reset counters, search/filter meters, and store everything in a local JSON file. Includes real-time updates and lightning UI effects.
# ⚡ Powerscan – Smart Electricity Meter Management App

Powerscan is a Windows desktop application (WPF) designed to help manage and monitor electricity meters in buildings. It allows users to store detailed location information, simulate live energy usage, calculate costs, and filter or search entries easily.

---

## 📸 Screenshots

| Entry Form                          | Meter List                          |
|------------------------------------|-------------------------------------|
| ![EntryWindow](screenshots/entry.png) | ![ListWindow](screenshots/list.png) |

---

## 🧩 Features

- ✅ Add new meter entries with full address & location details (ZIP, building, room, etc.)
- 📦 Data persistence via local JSON file
- 📈 Live power consumption simulation
- 💰 Cost calculation based on user-defined price per kWh
- 🔁 Manual reset of meter to start new consumption tracking
- 🔍 Search by various fields (e.g., Meter ID, City, Room)
- 📋 Toggle visibility of data columns with checkboxes
- ⚡ Dynamic lightning animation in UI for visual effect

---

## 🛠 Technologies Used

- C# (.NET Framework)
- WPF (Windows Presentation Foundation)
- MVVM architecture (partially applied)
- `System.Text.Json` for local data storage
- `DispatcherTimer` for simulating live meter usage

---

## 📂 File Storage

All data is stored locally in the following path:

```plaintext
%LocalAppData%\Powerscan\Data.json
