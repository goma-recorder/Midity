# Midity

[![Releases](https://img.shields.io/github/release/goma-recorder/Midity.svg)](https://github.com/goma-recorder/Midity/releases)
[![openupm](https://img.shields.io/npm/v/jp.goma_recorder.midity?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/jp.goma_recorder.midity/)
[![MIT License](http://img.shields.io/badge/license-MIT-green.svg?style=flat)](LICENSE)

**Midity** is a custom unity package for reading and writing Standard MIDI Files (SMF).

![file](https://user-images.githubusercontent.com/49276053/103522468-c375e100-4ebd-11eb-8a87-02bd6b275677.png)

![track](https://user-images.githubusercontent.com/49276053/103516434-e1d6df00-4eb3-11eb-8be0-d4f8f65df3ba.png)


## Installation
To import the package, please add the following sections to the package

### A. OpenUPM-CLI
```
$ openupm add jp.goma_recorder.midity
```
[OpenUPM](https://openupm.com/packages/jp.goma_recorder.midity)

### B. Git url

`Window -> Package Manager -> +▼ -> Add package from git url`
 - `https://github.com/goma-recorder/Midity.git?#upm`

### C. Release page
[here](https://github.com/goma-recorder/Midity/releases)

## Suport Events
- Midi Event
    - `OnNote`, `OffNote`, `PolyphonicKeyPressure`, `ControlChange`, `ProgramChange`, `ChannelPressure`, `PitchBend`
- Meta Event
    - `SequenceNumber`, `Text`, `Copyright`, `TrackName`, `InstrumentName`, `Lyric`, `Marker`, `Queue`, `ProgramName`, `DeviceName`, `ChannelPrefix`, `PortNumber`, `EndOfTrack`, `Tempo`, `SmpteOffset`, `TimeSignature`, `Key`, `SequencerUnique`
- SysEx Event

## Packages that depend on this
- [Playable Midi](https://github.com/goma-recorder/PlayableMidi)
